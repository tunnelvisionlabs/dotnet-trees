// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public partial class SortedTreeSet<T> : ISet<T>, IReadOnlyCollection<T>, ICollection
    {
        private readonly SortedTreeList<T> _sortedList;

        public SortedTreeSet()
        {
            _sortedList = new SortedTreeList<T>();
        }

        public SortedTreeSet(IEnumerable<T> collection)
        {
            _sortedList = new SortedTreeList<T>();
            UnionWith(collection);
        }

        public SortedTreeSet(IComparer<T> comparer)
        {
            _sortedList = new SortedTreeList<T>(comparer);
        }

        public SortedTreeSet(IEnumerable<T> collection, IComparer<T> comparer)
        {
            _sortedList = new SortedTreeList<T>(comparer);
            UnionWith(collection);
        }

        public SortedTreeSet(int branchingFactor)
        {
            _sortedList = new SortedTreeList<T>(branchingFactor);
        }

        public SortedTreeSet(int branchingFactor, IComparer<T> comparer)
        {
            _sortedList = new SortedTreeList<T>(branchingFactor, comparer);
        }

        public SortedTreeSet(int branchingFactor, IEnumerable<T> collection, IComparer<T> comparer)
        {
            _sortedList = new SortedTreeList<T>(branchingFactor, comparer);
            UnionWith(collection);
        }

        public IComparer<T> Comparer => _sortedList.Comparer;

        public int Count => _sortedList.Count;

        public T Max => _sortedList.Count == 0 ? default : _sortedList[Count - 1];

        public T Min => _sortedList.Count == 0 ? default : _sortedList[0];

        bool ICollection<T>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => ((ICollection)_sortedList).SyncRoot;

        public static IEqualityComparer<SortedTreeSet<T>> CreateSetComparer()
        {
            return SortedTreeSetEqualityComparer.Default;
        }

        public static IEqualityComparer<SortedTreeSet<T>> CreateSetComparer(IEqualityComparer<T> memberEqualityComparer)
        {
            if (memberEqualityComparer == null || memberEqualityComparer == EqualityComparer<T>.Default)
                return CreateSetComparer();

            return new SortedTreeSetEqualityComparer(memberEqualityComparer);
        }

        public bool Add(T item) => _sortedList.Add(item, addIfPresent: false);

        public void Clear() => _sortedList.Clear();

        public bool Contains(T item) => _sortedList.Contains(item);

        internal int IndexOf(T item) => _sortedList.IndexOf(item);

        internal int FindIndex(Predicate<T> match) => _sortedList.FindIndex(match);

        public void CopyTo(T[] array) => _sortedList.CopyTo(array);

        public void CopyTo(T[] array, int arrayIndex) => _sortedList.CopyTo(array, arrayIndex);

        public void CopyTo(T[] array, int arrayIndex, int count) => _sortedList.CopyTo(0, array, arrayIndex, count);

        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
            {
                return;
            }

            if (other == this)
            {
                Clear();
                return;
            }

            foreach (T item in other)
            {
                Remove(item);
            }
        }

        public Enumerator GetEnumerator() => new Enumerator(_sortedList.GetEnumerator());

        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
                return;

            if (this == other)
                return;

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                int i = 0;
                int j = 0;
                while (i < Count && j < sortedSet.Count)
                {
                    int comparison = Comparer.Compare(_sortedList[i], sortedSet._sortedList[j]);
                    if (comparison == 0)
                    {
                        // Keep the item
                        i++;
                        j++;
                    }
                    else if (comparison < 0)
                    {
                        _sortedList.RemoveAt(i);
                    }
                    else
                    {
                        // The item may be present, but it's not the current item
                        j++;
                    }
                }

                _sortedList.RemoveRange(i, Count - i);
            }
            else
            {
                var toSave = new SortedTreeSet<T>(Comparer);
                foreach (T item in other)
                {
                    if (Contains(item))
                        toSave.Add(item);
                }

                IntersectWith(toSave);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
                return other.Any();

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                if (Count >= sortedSet.Count)
                    return false;

                foreach (T item in this)
                {
                    if (!sortedSet.Contains(item))
                        return false;
                }

                return true;
            }

            (int uniqueCount, int unfoundCount) = CheckUniqueAndUnfoundElements(other, returnIfUnfound: false);
            return uniqueCount == Count && unfoundCount > 0;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
                return false;

            if (this == other)
                return false;

            if (other is ICollection collection && collection.Count == 0)
            {
                Debug.Assert(Count > 0, $"Assertion failed: {nameof(Count)} > 0");
                return true;
            }

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                if (sortedSet.Count >= Count)
                    return false;

                foreach (T item in sortedSet)
                {
                    if (!Contains(item))
                        return false;
                }

                return true;
            }

            (int uniqueCount, int unfoundCount) = CheckUniqueAndUnfoundElements(other, returnIfUnfound: true);
            return uniqueCount < Count && unfoundCount == 0;
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
                return true;

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                if (Count > sortedSet.Count)
                    return false;

                foreach (T item in this)
                {
                    if (!sortedSet.Contains(item))
                        return false;
                }

                return true;
            }

            (int uniqueCount, int unfoundCount) = CheckUniqueAndUnfoundElements(other, returnIfUnfound: false);
            return uniqueCount == Count && unfoundCount >= 0;
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (this == other)
                return true;

            if (other is ICollection collection && collection.Count == 0)
                return true;

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                if (Count < sortedSet.Count)
                    return false;

                foreach (T item in sortedSet)
                {
                    if (!Contains(item))
                        return false;
                }

                return true;
            }

            foreach (T item in other)
            {
                if (!Contains(item))
                    return false;
            }

            return true;
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
            {
                return false;
            }

            foreach (T item in other)
            {
                if (Contains(item))
                    return true;
            }

            return false;
        }

        public bool Remove(T item) => _sortedList.Remove(item);

        public int RemoveWhere(Predicate<T> match) => _sortedList.RemoveAll(match);

        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (this == other)
                return true;

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                if (Count != sortedSet.Count)
                    return false;

                Enumerator x = GetEnumerator();
                Enumerator y = sortedSet.GetEnumerator();
                while (true)
                {
                    bool hasX = x.MoveNext();
                    bool hasY = y.MoveNext();
                    Debug.Assert(hasX == hasY, $"Assertion failed: {nameof(hasX)} == {nameof(hasY)}");
                    if (!hasX)
                    {
                        return true;
                    }

                    if (Comparer.Compare(x.Current, y.Current) != 0)
                        return false;
                }
            }

            (int uniqueCount, int unfoundCount) = CheckUniqueAndUnfoundElements(other, returnIfUnfound: true);
            return uniqueCount == Count && unfoundCount == 0;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
            {
                UnionWith(other);
                return;
            }

            if (other == this)
            {
                Clear();
                return;
            }

            if (other is SortedTreeSet<T> sortedSet && Comparer.Equals(sortedSet.Comparer))
            {
                foreach (T item in other)
                {
                    if (!Remove(item))
                        Add(item);
                }

                return;
            }

            sortedSet = new SortedTreeSet<T>(other, Comparer);
            SymmetricExceptWith(sortedSet);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            foreach (T item in other)
            {
                Add(item);
            }
        }

        public void TrimExcess() => _sortedList.TrimExcess();

        public bool TryGetValue(T equalValue, out T actualValue)
        {
            int index = _sortedList.BinarySearch(equalValue);
            if (index < 0)
            {
                actualValue = default;
                return false;
            }

            actualValue = _sortedList[index];
            return true;
        }

        void ICollection<T>.Add(T item) => Add(item);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_sortedList).CopyTo(array, index);

        private (int uniqueCount, int unfoundCount) CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound)
        {
            if (Count == 0)
            {
                if (other.Any())
                    return (uniqueCount: 0, unfoundCount: 1);

                return (uniqueCount: 0, unfoundCount: 0);
            }

            const int StackAllocThreshold = 100;
            int originalLastIndex = Count;
            int intArrayLength = ((originalLastIndex - 1) / 32) + 1;
            Span<int> span = intArrayLength <= StackAllocThreshold
                ? stackalloc int[intArrayLength]
                : new int[intArrayLength];
            BitHelper bitHelper = new BitHelper(span);

            // count of items in other not found in this
            int unfoundCount = 0;

            // count of unique items in other found in this
            int uniqueCount = 0;

            foreach (T item in other)
            {
                int index = _sortedList.IndexOf(item);
                if (index >= 0)
                {
                    if (!bitHelper.IsMarked(index))
                    {
                        bitHelper.MarkBit(index);
                        uniqueCount++;
                    }
                }
                else
                {
                    unfoundCount++;
                    if (returnIfUnfound)
                        return (uniqueCount, unfoundCount);
                }
            }

            return (uniqueCount, unfoundCount);
        }

        internal void Validate(ValidationRules validationRules)
        {
            _sortedList.Validate(validationRules);

            for (int i = 0; i < Count - 1; i++)
            {
                Debug.Assert(Comparer.Compare(_sortedList[i], _sortedList[i + 1]) < 0, "Assertion failed: Comparer.Compare(_sortedList[i], _sortedList[i + 1]) < 0");
                Debug.Assert(Comparer.Compare(_sortedList[i + 1], _sortedList[i]) > 0, "Assertion failed: Comparer.Compare(_sortedList[i + 1], _sortedList[i]) > 0");
            }
        }

#pragma warning disable SA1206 // Declaration keywords should follow order
        private ref struct BitHelper
#pragma warning restore SA1206 // Declaration keywords should follow order
        {
            private readonly Span<int> _span;

            public BitHelper(Span<int> span)
            {
                _span = span;
            }

            internal void MarkBit(int bitPosition)
            {
                Debug.Assert(bitPosition >= 0, $"Assertion failed: {nameof(bitPosition)} >= 0");

                int bitArrayIndex = bitPosition / 32;
                Debug.Assert(bitArrayIndex < _span.Length, $"Assertion failed: {nameof(bitArrayIndex)} < {nameof(_span)}.Length");

                // Note: Using (bitPosition & 31) instead of (bitPosition % 32)
                _span[bitArrayIndex] |= 1 << (bitPosition & 31);
            }

            internal bool IsMarked(int bitPosition)
            {
                Debug.Assert(bitPosition >= 0, $"Assertion failed: {nameof(bitPosition)} >= 0");

                int bitArrayIndex = bitPosition / 32;
                Debug.Assert(bitArrayIndex < _span.Length, $"Assertion failed: {nameof(bitArrayIndex)} < {nameof(_span)}.Length");

                // Note: Using (bitPosition & 31) instead of (bitPosition % 32)
                return (_span[bitArrayIndex] & (1 << (bitPosition & 31))) != 0;
            }
        }

        private class SortedTreeSetEqualityComparer : IEqualityComparer<SortedTreeSet<T>>
        {
            public static readonly IEqualityComparer<SortedTreeSet<T>> Default = new SortedTreeSetEqualityComparer();

            private readonly IComparer<T> _comparer = Comparer<T>.Default;
            private readonly IEqualityComparer<T> _equalityComparer;

            private SortedTreeSetEqualityComparer()
                : this(null)
            {
            }

            public SortedTreeSetEqualityComparer(IEqualityComparer<T> memberEqualityComparer)
            {
                _equalityComparer = memberEqualityComparer ?? EqualityComparer<T>.Default;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is SortedTreeSetEqualityComparer comparer))
                    return false;

                return _comparer == comparer._comparer;
            }

            public override int GetHashCode()
            {
                return _comparer.GetHashCode() ^ _equalityComparer.GetHashCode();
            }

            public bool Equals(SortedTreeSet<T> x, SortedTreeSet<T> y)
            {
                if (x is null)
                {
                    return y is null;
                }
                else if (y is null)
                {
                    return false;
                }

                if (x.Comparer.Equals(y.Comparer))
                {
                    return x.SetEquals(y);
                }

                bool found = false;
                foreach (T item1 in x)
                {
                    found = false;
                    foreach (T item2 in y)
                    {
                        if (_comparer.Compare(item1, item2) == 0)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        return false;
                    }
                }

                return true;
            }

            public int GetHashCode(SortedTreeSet<T> obj)
            {
                if (obj == null)
                    return 0;

                int hashCode = 0;
                foreach (T item in obj)
                {
                    hashCode = hashCode ^ _equalityComparer.GetHashCode(item);
                }

                return hashCode;
            }
        }
    }
}
