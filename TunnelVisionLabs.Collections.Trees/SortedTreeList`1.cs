// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class SortedTreeList<T> : IList<T>, IReadOnlyList<T>, IList
    {
        private readonly IComparer<T> _comparer;
        private readonly TreeList<T> _treeList;

        public SortedTreeList()
            : this(default(IComparer<T>))
        {
        }

        public SortedTreeList(IEnumerable<T> collection)
            : this(collection, comparer: null)
        {
        }

        public SortedTreeList(IComparer<T> comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _treeList = new TreeList<T>();
        }

        public SortedTreeList(IEnumerable<T> collection, IComparer<T> comparer)
            : this(comparer)
        {
            AddRange(collection);
        }

        public SortedTreeList(int branchingFactor)
            : this(branchingFactor, comparer: null)
        {
        }

        public SortedTreeList(int branchingFactor, IComparer<T> comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
            _treeList = new TreeList<T>(branchingFactor);
        }

        public SortedTreeList(int branchingFactor, IEnumerable<T> collection, IComparer<T> comparer)
            : this(branchingFactor, comparer)
        {
            AddRange(collection);
        }

        public IComparer<T> Comparer => _comparer;

        public int Count => _treeList.Count;

        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

        bool IList.IsFixedSize => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => ((ICollection)_treeList).SyncRoot;

        public T this[int index]
        {
            get => _treeList[index];
        }

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

        public int BinarySearch(T item) => _treeList.BinarySearch(item, _comparer);

        public int BinarySearch(int index, int count, T item) => _treeList.BinarySearch(index, count, item, _comparer);

        public int IndexOf(T item) => IndexOf(item, 0, Count);

        public int IndexOf(T item, int index) => IndexOf(item, index, Count - index);

        public int IndexOf(T item, int index, int count)
        {
            var comparer = new CoercingComparer(_comparer, 1);
            int result = _treeList.BinarySearch(index, count, item, comparer);
            if (!comparer.FoundMatch)
                return -1;

            return ~result;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index < 0)
                return false;

            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index) => _treeList.RemoveAt(index);

        public void RemoveRange(int index, int count) => _treeList.RemoveRange(index, count);

        public int RemoveAll(Predicate<T> match) => _treeList.RemoveAll(match);

        public void Add(T item) => Add(item, addIfPresent: true);

        internal bool Add(T item, bool addIfPresent)
        {
            var comparer = addIfPresent ? new CoercingComparer(_comparer, -1) : _comparer;
            int result = _treeList.BinarySearch(item, comparer);
            if (result >= 0)
                return false;

            _treeList.Insert(~result, item);
            return true;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (T item in collection)
                Add(item);
        }

        public void Clear() => _treeList.Clear();

        public bool Contains(T item) => _treeList.BinarySearch(item, _comparer) >= 0;

        public void CopyTo(T[] array) => _treeList.CopyTo(array);

        public void CopyTo(T[] array, int arrayIndex) => _treeList.CopyTo(array, arrayIndex);

        public void CopyTo(int index, T[] array, int arrayIndex, int count) => _treeList.CopyTo(index, array, arrayIndex, count);

        public Enumerator GetEnumerator() => new Enumerator(_treeList.GetEnumerator());

        public bool Exists(Predicate<T> match) => _treeList.Exists(match);

        public T Find(Predicate<T> match) => _treeList.Find(match);

        public SortedTreeList<T> FindAll(Predicate<T> match) => new SortedTreeList<T>(_treeList.FindAll(match), _comparer);

        public int FindIndex(Predicate<T> match) => _treeList.FindIndex(match);

        public int FindIndex(int startIndex, Predicate<T> match) => _treeList.FindIndex(startIndex, match);

        public int FindIndex(int startIndex, int count, Predicate<T> match) => _treeList.FindIndex(startIndex, count, match);

        public T FindLast(Predicate<T> match) => _treeList.FindLast(match);

        public int FindLastIndex(Predicate<T> match) => _treeList.FindLastIndex(match);

        public int FindLastIndex(int startIndex, Predicate<T> match) => _treeList.FindLastIndex(startIndex, match);

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => _treeList.FindLastIndex(startIndex, count, match);

        public int LastIndexOf(T item)
        {
            if (Count == 0)
                return -1;

            return LastIndexOf(item, Count - 1, Count);
        }

        public int LastIndexOf(T item, int index) => LastIndexOf(item, index, index + 1);

        public int LastIndexOf(T item, int index, int count)
        {
            var comparer = new CoercingComparer(_comparer, -1);
            int result = _treeList.BinarySearch(index - count + 1, count, item, comparer);
            if (!comparer.FoundMatch)
                return -1;

            return ~result - 1;
        }

        public void ForEach(Action<T> action) => _treeList.ForEach(action);

        public SortedTreeList<T> GetRange(int index, int count) => new SortedTreeList<T>(_treeList.GetRange(index, count), _comparer);

        public T[] ToArray() => _treeList.ToArray();

        public void TrimExcess() => _treeList.TrimExcess();

        public bool TrueForAll(Predicate<T> match) => _treeList.TrueForAll(match);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            int index = ((IList)this).IndexOf(value);
            if (index >= 0)
                RemoveAt(index);
        }

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_treeList).CopyTo(array, index);

        internal void Validate(ValidationRules validationRules)
        {
            Debug.Assert(_comparer != null, $"Assertion failed: _comparer != null");
            Debug.Assert(_treeList != null, $"Assertion failed: _treeList != null");

            _treeList.Validate(validationRules);

            for (int i = 0; i < Count - 1; i++)
            {
                Debug.Assert(_comparer.Compare(this[i], this[i + 1]) <= 0, "Assertion failed: _comparer.Compare(this[i], this[i + 1]) <= 0");
                Debug.Assert(_comparer.Compare(this[i + 1], this[i]) >= 0, "Assertion failed: _comparer.Compare(this[i + 1], this[i]) >= 0");
            }
        }

        private sealed class CoercingComparer : IComparer<T>
        {
            private readonly IComparer<T> _underlyingComparer;
            private readonly int _coerceResult;
            private bool _foundMatch;

            public CoercingComparer(IComparer<T> underlyingComparer, int coerceResult)
            {
                _underlyingComparer = underlyingComparer;
                _coerceResult = coerceResult;
            }

            public bool FoundMatch => _foundMatch;

            public int Compare(T x, T y)
            {
                int result = _underlyingComparer.Compare(x, y);
                if (result != 0)
                    return result;

                // if we get here from a binary search, it means we found the item we searched for
                _foundMatch = true;
                return _coerceResult;
            }
        }
    }
}
