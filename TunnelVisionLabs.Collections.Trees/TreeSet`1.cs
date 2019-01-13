// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public partial class TreeSet<T> : ISet<T>, IReadOnlyCollection<T>, ICollection
    {
        private readonly IEqualityComparer<T> _comparer;
        private readonly SortedTreeList<(int hashCode, T value)> _sortedList;

        public TreeSet()
            : this(default(IEqualityComparer<T>))
        {
        }

        public TreeSet(IEnumerable<T> collection)
            : this(collection, null)
        {
        }

        public TreeSet(IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _sortedList = new SortedTreeList<(int hashCode, T value)>(SetHelper.WrapperComparer<T>.Instance);
        }

        public TreeSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            UnionWith(collection);
        }

        public TreeSet(int branchingFactor)
            : this(branchingFactor, null)
        {
        }

        public TreeSet(int branchingFactor, IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
            _sortedList = new SortedTreeList<(int hashCode, T value)>(branchingFactor, SetHelper.WrapperComparer<T>.Instance);
        }

        public TreeSet(int branchingFactor, IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : this(branchingFactor, comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            UnionWith(collection);
        }

        public IEqualityComparer<T> Comparer => _comparer;

        public int Count => _sortedList.Count;

        bool ICollection<T>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public static IEqualityComparer<TreeSet<T>> CreateSetComparer()
        {
            return TreeSetEqualityComparer.Default;
        }

        public bool Add(T item)
        {
            if (Contains(item))
                return false;

            _sortedList.Add((_comparer.GetHashCode(item), item));
            return true;
        }

        public void Clear() => _sortedList.Clear();

        public bool Contains(T item)
        {
            return TryGetValue(item, out _);
        }

        public void CopyTo(T[] array) => CopyTo(array, 0, Count);

        public void CopyTo(T[] array, int arrayIndex) => CopyTo(array, arrayIndex, Count);

        public void CopyTo(T[] array, int arrayIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (Count < count)
                throw new ArgumentException();
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("Not enough space is available in the destination array.", string.Empty);

            for (int i = 0; i < count; i++)
            {
                array[arrayIndex + i] = _sortedList[i].value;
            }
        }

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

            if (other is TreeSet<T> set && Comparer.Equals(set.Comparer))
            {
                int i = 0;
                int j = 0;
                while (i < Count && j < set.Count)
                {
                    (int hashCode, T value) left = _sortedList[i];
                    (int hashCode, T value) right = set._sortedList[j];

                    if (left.hashCode == right.hashCode)
                    {
                        if (_comparer.Equals(left.value, right.value))
                        {
                            i++;
                            j++;
                        }
                        else
                        {
                            // Need to compare all items with the same hash code
                            bool foundMatch = false;
                            for (int k = j + 1; k < set.Count; k++)
                            {
                                (int hashCode, T value) altRight = set._sortedList[k];
                                if (altRight.hashCode != left.hashCode)
                                    break;

                                if (_comparer.Equals(left.value, altRight.value))
                                {
                                    foundMatch = true;
                                    break;
                                }
                            }

                            if (foundMatch)
                            {
                                // Only increment i since set._sortedList[j] might match _sortedList[i+1]
                                i++;
                            }
                            else
                            {
                                _sortedList.RemoveAt(i);
                            }
                        }
                    }
                    else if (left.hashCode < right.hashCode)
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
                var toSave = new TreeSet<T>(Comparer);
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

            if (other is TreeSet<T> set && Comparer.Equals(set.Comparer))
            {
                if (Count >= set.Count)
                    return false;

                foreach (T item in this)
                {
                    if (!set.Contains(item))
                        return false;
                }

                return true;
            }

            (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, Comparer, other, returnIfUnfound: false);
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

            if (other is TreeSet<T> set && Comparer.Equals(set.Comparer))
            {
                if (set.Count >= Count)
                    return false;

                foreach (T item in set)
                {
                    if (!Contains(item))
                        return false;
                }

                return true;
            }

            (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, Comparer, other, returnIfUnfound: true);
            return uniqueCount < Count && unfoundCount == 0;
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Count == 0)
                return true;

            if (other is TreeSet<T> set && Comparer.Equals(set.Comparer))
            {
                if (Count > set.Count)
                    return false;

                foreach (T item in this)
                {
                    if (!set.Contains(item))
                        return false;
                }

                return true;
            }

            (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, Comparer, other, returnIfUnfound: false);
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

            if (other is TreeSet<T> set && Comparer.Equals(set.Comparer))
            {
                if (Count < set.Count)
                    return false;

                foreach (T item in set)
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

        public bool Remove(T item)
        {
            int hashCode = _comparer.GetHashCode(item);

            // Find the index of the first item with this hash code
            int index = _sortedList.IndexOf((hashCode, item));
            if (index >= 0)
            {
                // Find the item if it exists
                for (int i = index; i < Count; i++)
                {
                    (int hashCode, T value) bucket = _sortedList[i];
                    if (bucket.hashCode != hashCode)
                        return false;

                    if (_comparer.Equals(bucket.value, item))
                    {
                        _sortedList.RemoveAt(i);
                        return true;
                    }
                }
            }

            return false;
        }

        public int RemoveWhere(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            return _sortedList.RemoveAll(bucket => match(bucket.value));
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (this == other)
                return true;

            (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, Comparer, other, returnIfUnfound: true);
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

            if (other is TreeSet<T> set && Comparer.Equals(set.Comparer))
            {
                foreach (T item in other)
                {
                    if (!Remove(item))
                        Add(item);
                }

                return;
            }

            set = new TreeSet<T>(other, Comparer);
            SymmetricExceptWith(set);
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
            int hashCode = _comparer.GetHashCode(equalValue);

            // Find the index of the first item with this hash code
            int index = _sortedList.IndexOf((hashCode, equalValue));
            if (index >= 0)
            {
                // Find the duplicate value if it exists
                for (int i = index; i < Count; i++)
                {
                    (int hashCode, T value) bucket = _sortedList[i];
                    if (bucket.hashCode != hashCode)
                        break;

                    if (_comparer.Equals(bucket.value, equalValue))
                    {
                        actualValue = bucket.value;
                        return true;
                    }
                }
            }

            actualValue = default;
            return false;
        }

        void ICollection<T>.Add(T item) => Add(item);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", nameof(array));
            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException();
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (array.Length - index < Count)
                throw new ArgumentException("Not enough space is available in the destination array.", nameof(index));

            try
            {
                for (int i = 0; i < Count; i++)
                {
                    array.SetValue(_sortedList[i].value, i + index);
                }
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("Invalid array type");
            }
        }

        internal void Validate(ValidationRules validationRules)
        {
            _sortedList.Validate(validationRules);

            for (int i = 0; i < Count - 1; i++)
            {
                Debug.Assert(_sortedList[i].hashCode <= _sortedList[i + 1].hashCode, "Assertion failed: _sortedList[i].hashCode <= _sortedList[i + 1].hashCode");
                Debug.Assert(!Comparer.Equals(_sortedList[i].value, _sortedList[i + 1].value), "Assertion failed: !Comparer.Equals(_sortedList[i].value, _sortedList[i + 1].value)");
            }
        }

        private class TreeSetEqualityComparer : IEqualityComparer<TreeSet<T>>
        {
            public static readonly IEqualityComparer<TreeSet<T>> Default = new TreeSetEqualityComparer();

            private readonly IEqualityComparer<T> _comparer = EqualityComparer<T>.Default;

            private TreeSetEqualityComparer()
            {
            }

            public override bool Equals(object obj)
            {
                if (!(obj is TreeSetEqualityComparer comparer))
                    return false;

                return _comparer == comparer._comparer;
            }

            public override int GetHashCode()
            {
                return _comparer.GetHashCode();
            }

            public bool Equals(TreeSet<T> x, TreeSet<T> y)
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
                        if (_comparer.Equals(item1, item2))
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

            public int GetHashCode(TreeSet<T> obj)
            {
                if (obj == null)
                    return 0;

                int hashCode = 0;
                foreach (T item in obj)
                {
                    hashCode = hashCode ^ _comparer.GetHashCode(item);
                }

                return hashCode;
            }
        }
    }
}
