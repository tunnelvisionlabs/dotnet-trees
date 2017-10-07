// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;
#if NETSTANDARD1_0
    using System.Threading;
#endif

    public partial class TreeList<T> : IList<T>, IReadOnlyList<T>, IList, ICollection
    {
        private readonly int _branchingFactor;
        private Node _root = Node.Empty;
        private int _version;
#if NETSTANDARD1_0
        private object _syncRoot;
#endif

        public TreeList()
            : this(16)
        {
        }

        public TreeList(IEnumerable<T> collection)
            : this(16, collection)
        {
        }

        public TreeList(int branchingFactor)
        {
            if (branchingFactor < 2)
                throw new ArgumentOutOfRangeException(nameof(branchingFactor));

            _branchingFactor = branchingFactor;
        }

        public TreeList(int branchingFactor, IEnumerable<T> collection)
            : this(branchingFactor)
        {
            AddRange(collection);
        }

        private TreeList(int branchingFactor, Node root)
        {
            Debug.Assert(branchingFactor >= 2, $"Assertion failed: {nameof(branchingFactor)} >= 2");
            Debug.Assert(root != null, $"Assertion failed: {nameof(root)} != null");

            _branchingFactor = branchingFactor;
            _root = root;
        }

        public int Count
        {
            get
            {
                return _root.Count;
            }
        }

        bool IList.IsFixedSize => false;

        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot
        {
            get
            {
#if NETSTANDARD1_0
                if (_syncRoot == null)
                    Interlocked.CompareExchange(ref _syncRoot, new object(), null);

                return _syncRoot;
#else
                return SyncRootFallback.GetOrCreateSyncRoot(this);
#endif
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= Count)
                    throw new ArgumentOutOfRangeException($"{nameof(index)} must be less than {nameof(Count)}", nameof(index));

                return _root[index];
            }

            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= Count)
                    throw new ArgumentOutOfRangeException($"{nameof(index)} must be less than {nameof(Count)}", nameof(index));

                _root[index] = value;
                _version++;
            }
        }

        object IList.this[int index]
        {
            get
            {
                if ((uint)index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _root[index];
            }

            set
            {
                if (value == null && default(T) != null)
                    throw new ArgumentNullException(nameof(value));
                if ((uint)index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                try
                {
                    this[index] = (T)value;
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException($"The value \"{value.GetType()}\" isn't of type \"{typeof(T)}\" and can't be used in this generic collection.", nameof(value));
                }
            }
        }

        public void Add(T item)
        {
            _root = Node.Insert(_root, _branchingFactor, Count, item);
            _version++;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            int previousCount = Count;
            _root = Node.InsertRange(_root, _branchingFactor, previousCount, collection);
            if (Count > previousCount)
                _version++;
        }

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
                throw new ArgumentException($"The value \"{value.GetType()}\" isn't of type \"{typeof(T)}\" and can't be used in this generic collection.", nameof(value));
            }

            return Count - 1;
        }

        public void Clear()
        {
            if (Count != 0)
            {
                _root = Node.Empty;
                _version++;
            }
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
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

        public void CopyTo(T[] array)
        {
            CopyTo(0, array, 0, Count);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, Count);
        }

        public void CopyTo(int srcIndex, T[] dest, int dstIndex, int length)
        {
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));
            if (srcIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(srcIndex));
            if (dstIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(dstIndex));
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));
            if (Count - srcIndex < length)
                throw new ArgumentException();
            if (dest.Length - dstIndex < length)
                throw new ArgumentException("Not enough space is available in the destination array.", string.Empty);

            for (int i = 0; i < length; i++)
            {
                dest[dstIndex + i] = this[srcIndex + i];
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Length - index < Count)
                throw new ArgumentException("Not enough space is available in the destination array.", nameof(index));

            try
            {
                int offset = index;
                LeafNode leaf = _root.FirstLeaf;
                while (leaf != null)
                {
                    leaf.CopyToArray(array, offset);
                    offset += leaf.Count;
                    leaf = leaf.Next;
                }
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type");
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item)
        {
            return IndexOf(item, 0, Count);
        }

        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, Count - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentOutOfRangeException();

            return _root.IndexOf(item, new TreeSpan(index, count));
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

        public void Insert(int index, T item)
        {
            _root = Node.Insert(_root, _branchingFactor, index, item);
            _version++;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            _root = Node.InsertRange(_root, _branchingFactor, index, collection);
            _version++;
        }

        void IList.Insert(int index, object value)
        {
            if (value == null && default(T) != null)
                throw new ArgumentNullException(nameof(value));
            if ((uint)index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            try
            {
                Insert(index, (T)value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(string.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", value.GetType(), typeof(T)), nameof(value));
            }
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

        void IList.Remove(object value)
        {
            int index = ((IList)this).IndexOf(value);
            if (index >= 0)
                RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            _root = Node.RemoveAt(_root, index);
            _version++;
        }

        public void RemoveRange(int index, int count)
        {
            _root = Node.RemoveRange(_root, index, count);
            if (count > 0)
                _version++;
        }

        public int RemoveAll(Predicate<T> match)
        {
            int previousCount = Count;
            _root = Node.RemoveAll(_root, match);
            _version++;
            return previousCount - Count;
        }

        public int BinarySearch(T item)
        {
            return BinarySearch(0, Count, item, null);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            return _root.BinarySearch(new TreeSpan(index, count), item, comparer ?? Comparer<T>.Default);
        }

        public TreeList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter)
        {
            var newRoot = _root.ConvertAll(converter);
            return new TreeList<TOutput>(_branchingFactor, newRoot);
        }

        public bool Exists(Predicate<T> match)
        {
            return FindIndex(match) >= 0;
        }

        public T Find(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            foreach (T item in this)
            {
                if (match(item))
                    return item;
            }

            return default;
        }

        public TreeList<T> FindAll(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            return new TreeList<T>(_branchingFactor, this.Where(i => match(i)));
        }

        public int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, Count, match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return FindIndex(startIndex, Count - startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (startIndex > Count - count)
                throw new ArgumentOutOfRangeException();

            return _root.FindIndex(new TreeSpan(startIndex, count), match);
        }

        public T FindLast(Predicate<T> match)
        {
            int index = FindLastIndex(match);
            if (index < 0)
                return default;

            return this[index];
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return FindLastIndex(Count - 1, Count, match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return FindLastIndex(startIndex, startIndex + 1, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (Count == 0)
            {
                if (startIndex != -1)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            else
            {
                if ((uint)startIndex >= (uint)Count)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (count < 0 || startIndex - count + 1 < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return _root.FindLastIndex(TreeSpan.FromReverseSpan(startIndex, count), match);
        }

        public int LastIndexOf(T item)
        {
            if (Count == 0)
            {
                return -1;
            }

            return LastIndexOf(item, Count - 1, Count);
        }

        public int LastIndexOf(T item, int index)
        {
            return LastIndexOf(item, index, index + 1);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count - 1 > index)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count)
                throw new ArgumentOutOfRangeException();

            return _root.LastIndexOf(item, TreeSpan.FromReverseSpan(index, count));
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in this)
            {
                action(item);
            }
        }

        public TreeList<T> GetRange(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            return new TreeList<T>(_branchingFactor, this.Skip(index).Take(count));
        }

        public void Reverse()
        {
            Reverse(0, Count);
        }

        public void Reverse(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            if (count != 0)
            {
                _root.Reverse(new TreeSpan(index, count));
                _version++;
            }
        }

        public void Sort()
        {
            Sort(0, Count, null);
        }

        public void Sort(IComparer<T> comparer)
        {
            Sort(0, Count, comparer);
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            _root.Sort(new TreeSpan(index, count), comparer ?? Comparer<T>.Default);
        }

        public void Sort(Comparison<T> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException(nameof(comparison));

            Sort(0, Count, new ComparisonComparer(comparison));
        }

        public T[] ToArray()
        {
            T[] result = new T[Count];
            CopyTo(result, 0);
            return result;
        }

        public void TrimExcess()
        {
            _root = Node.TrimExcess(_root);
            _version++;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            foreach (T item in this)
            {
                if (!match(item))
                    return false;
            }

            return true;
        }

        internal void Validate(ValidationRules validationRules)
        {
            Debug.Assert(_branchingFactor >= 2, $"Assertion failed: {nameof(_branchingFactor)} >= 2");
            Debug.Assert(_root != null, $"Assertion failed: {nameof(_root)} != null");
            if (_root.FirstChild != null)
            {
                Debug.Assert(_root.FirstChild.NextNode != null, $"Assertion failed: _root.FirstChild.NextNode != null");
            }

            _root.Validate(validationRules);
        }

        private sealed class ComparisonComparer : IComparer<T>
        {
            private readonly Comparison<T> _comparison;

            public ComparisonComparer(Comparison<T> comparison)
            {
                _comparison = comparison;
            }

            public int Compare(T x, T y) => _comparison(x, y);
        }
    }
}
