// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;

    public partial class TreeList<T> : IList<T>, IReadOnlyList<T>, IList, ICollection
    {
        private readonly int _branchingFactor;
        private Node _root = Node.Empty;
        private int _version;

        public TreeList()
            : this(16)
        {
        }

        public TreeList(int branchingFactor)
        {
            if (branchingFactor < 2)
                throw new ArgumentOutOfRangeException(nameof(branchingFactor));

            _branchingFactor = branchingFactor;
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

        object ICollection.SyncRoot => throw new NotSupportedException();

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= Count)
                    throw new ArgumentException("index must be less than Count", nameof(index));

                return _root[index];
            }

            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= Count)
                    throw new ArgumentException("index must be less than Count", nameof(index));

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
                    throw new ArgumentException(string.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", value.GetType(), typeof(T)), nameof(value));
                }
            }
        }

        public void Add(T item)
        {
            _root = Node.Insert(_root, _branchingFactor, Count, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            throw new NotImplementedException();
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
                throw new ArgumentException(string.Format("The value \"{0}\" isn't of type \"{1}\" and can't be used in this generic collection.", value.GetType(), typeof(T)), nameof(value));
            }

            return Count - 1;
        }

        public void Clear()
        {
            _root = Node.Empty;
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
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Not enough space is available in the destination array.", nameof(arrayIndex));

            for (int i = 0; i < Count; i++)
                array[arrayIndex + i] = this[i];
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
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
                throw new ArgumentException();

            return _root.IndexOf(item, index, count);
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
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            throw new NotImplementedException();
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
        }

        public void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public int RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int BinarySearch(T item)
        {
            throw new NotImplementedException();
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public TreeList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public T Find(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public TreeList<T> FindAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public T FindLast(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item, int index)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item, int index, int count)
        {
            throw new NotImplementedException();
        }

        public void ForEach(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public TreeList<T> GetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            throw new NotImplementedException();
        }

        public void Reverse(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Sort()
        {
            throw new NotImplementedException();
        }

        public void Sort(IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void Sort(Comparison<T> comparison)
        {
            throw new NotImplementedException();
        }

        public T[] ToArray()
        {
            T[] result = new T[Count];
            CopyTo(result, 0);
            return result;
        }

        public void TrimExcess()
        {
            throw new NotImplementedException();
        }

        public bool TrueForAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }
    }
}
