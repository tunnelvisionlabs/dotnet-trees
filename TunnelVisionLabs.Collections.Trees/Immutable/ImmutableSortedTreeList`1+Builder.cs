// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed partial class ImmutableSortedTreeList<T>
    {
        public sealed class Builder : IList<T>, IReadOnlyList<T>, IList
        {
            private ImmutableSortedTreeList<T> _immutableList;
            private readonly ImmutableTreeList<T>.Builder _treeList;

            internal Builder(ImmutableSortedTreeList<T> immutableList)
            {
                _immutableList = immutableList;
                _treeList = immutableList._treeList.ToBuilder();
            }

            public IComparer<T> Comparer => _immutableList.Comparer;

            public int Count => _treeList.Count;

            bool ICollection<T>.IsReadOnly => false;

            bool IList.IsFixedSize => false;

            bool IList.IsReadOnly => false;

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot => this;

            public T this[int index] => _treeList[index];

            T IList<T>.this[int index]
            {
                get => this[index];
                set => throw new NotSupportedException();
            }

            object IList.this[int index]
            {
                get => this[index];
                set => throw new NotSupportedException();
            }

            public void Add(T item)
                => Add(item, addIfPresent: true);

            internal bool Add(T item, bool addIfPresent)
            {
                IComparer<T> comparer = addIfPresent ? new CoercingComparer(Comparer, -1) : Comparer;
                int result = _treeList.BinarySearch(item, comparer);
                if (result >= 0)
                    return false;

                _treeList.Insert(~result, item);
                return true;
            }

            public void AddRange(IEnumerable<T> items)
            {
                if (items == null)
                    throw new ArgumentNullException(nameof(items));

                foreach (T item in items)
                    Add(item);
            }

            public int BinarySearch(T item)
                => _treeList.BinarySearch(item, Comparer);

            public int BinarySearch(int index, int count, T item)
                => _treeList.BinarySearch(index, count, item, Comparer);

            public void Clear()
                => _treeList.Clear();

            public bool Contains(T item)
                => BinarySearch(item) >= 0;

            public void CopyTo(T[] array)
                => CopyTo(0, array, 0, Count);

            public void CopyTo(T[] array, int arrayIndex)
                => CopyTo(0, array, arrayIndex, Count);

            public void CopyTo(int index, T[] array, int arrayIndex, int count)
                => _treeList.CopyTo(index, array, arrayIndex, count);

            public bool Exists(Predicate<T> match)
                => FindIndex(match) >= 0;

            public T Find(Predicate<T> match)
                => _treeList.Find(match);

            public ImmutableSortedTreeList<T> FindAll(Predicate<T> match)
                => new ImmutableSortedTreeList<T>(_treeList.FindAll(match), Comparer);

            public int FindIndex(Predicate<T> match)
                => FindIndex(0, Count, match);

            public int FindIndex(int startIndex, Predicate<T> match)
                => FindIndex(startIndex, Count - startIndex, match);

            public int FindIndex(int startIndex, int count, Predicate<T> match)
                => _treeList.FindIndex(startIndex, count, match);

            public T FindLast(Predicate<T> match)
                => _treeList.FindLast(match);

            public int FindLastIndex(Predicate<T> match)
                => FindLastIndex(Count - 1, Count, match);

            public int FindLastIndex(int startIndex, Predicate<T> match)
                => FindLastIndex(startIndex, startIndex + 1, match);

            public int FindLastIndex(int startIndex, int count, Predicate<T> match)
                => _treeList.FindLastIndex(startIndex, count, match);

            public void ForEach(Action<T> action)
                => _treeList.ForEach(action);

            public Enumerator GetEnumerator()
                => new Enumerator(_treeList.GetEnumerator());

            public ImmutableSortedTreeList<T> GetRange(int index, int count)
                => ToImmutable().GetRange(index, count);

            public int IndexOf(T item)
                => IndexOf(item, 0, Count);

            public int IndexOf(T item, int index)
                => IndexOf(item, index, Count - index);

            public int IndexOf(T item, int index, int count)
            {
                var comparer = new CoercingComparer(Comparer, 1);
                int result = _treeList.BinarySearch(index, count, item, comparer);
                if (!comparer.FoundMatch)
                    return -1;

                return ~result;
            }

            public int LastIndexOf(T item)
                => LastIndexOf(item, Count - 1, Count);

            public int LastIndexOf(T item, int startIndex)
                => LastIndexOf(item, startIndex, startIndex + 1);

            public int LastIndexOf(T item, int startIndex, int count)
            {
                var comparer = new CoercingComparer(Comparer, -1);
                int result = _treeList.BinarySearch(startIndex - count + 1, count, item, comparer);
                if (!comparer.FoundMatch)
                    return -1;

                return ~result - 1;
            }

            public bool Remove(T item)
            {
                int index = IndexOf(item);
                if (index >= 0)
                {
                    RemoveAt(index);
                    return true;
                }

                return false;
            }

            public void RemoveRange(int index, int count)
                => _treeList.RemoveRange(index, count);

            public int RemoveAll(Predicate<T> match)
                => _treeList.RemoveAll(match);

            public void RemoveAt(int index)
                => _treeList.RemoveAt(index);

            public ImmutableSortedTreeList<T> ToImmutable()
            {
                ImmutableTreeList<T> treeList = _treeList.ToImmutable();
                if (_immutableList._treeList != treeList)
                {
                    _immutableList = new ImmutableSortedTreeList<T>(treeList, Comparer);
                }

                return _immutableList;
            }

            public bool TrueForAll(Predicate<T> match)
                => _treeList.TrueForAll(match);

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
                => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            int IList.Add(object value)
            {
                if (value == null && default(T) != null)
                    throw new ArgumentNullException(nameof(value));

                try
                {
                    Add((T)value);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException(string.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", value.GetType(), typeof(T)), nameof(value));
                }

                return Count - 1;
            }

            bool IList.Contains(object value)
            {
                if (value == null)
                {
                    if (default(T) == null)
                        return Contains(default);
                }
                else if (value is T)
                {
                    return Contains((T)value);
                }

                return false;
            }

            int IList.IndexOf(object value)
            {
                if (value == null)
                {
                    if (default(T) == null)
                        return IndexOf(default);
                }
                else if (value is T)
                {
                    return IndexOf((T)value);
                }

                return -1;
            }

            void IList.Remove(object value)
            {
                int index = ((IList)this).IndexOf(value);
                if (index >= 0)
                    RemoveAt(index);
            }

            void ICollection.CopyTo(Array array, int index)
            {
                ICollection treeList = _treeList;
                treeList.CopyTo(array, index);
            }

            void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

            void IList.Insert(int index, object value) => throw new NotSupportedException();

            internal void TrimExcess()
                => _treeList.TrimExcess();

            internal void Validate(ValidationRules validationRules)
            {
                Debug.Assert(_immutableList != null, $"Assertion failed: _immutableList != null");
                Debug.Assert(_treeList != null, $"Assertion failed: _treeList != null");

                _treeList.Validate(validationRules);

                for (int i = 0; i < Count - 1; i++)
                {
                    Debug.Assert(Comparer.Compare(this[i], this[i + 1]) <= 0, "Assertion failed: Comparer.Compare(this[i], this[i + 1]) <= 0");
                    Debug.Assert(Comparer.Compare(this[i + 1], this[i]) >= 0, "Assertion failed: Comparer.Compare(this[i + 1], this[i]) >= 0");
                }
            }
        }
    }
}
