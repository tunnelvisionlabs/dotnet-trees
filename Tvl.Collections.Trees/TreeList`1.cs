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
                throw new ArgumentOutOfRangeException("branchingFactor");

            _branchingFactor = branchingFactor;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
                if (index >= Count)
                    throw new ArgumentException("index must be less than Count", "index");

                return _root[index];
            }

            set
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
                if (index >= Count)
                    throw new ArgumentException("index must be less than Count", "index");

                _root[index] = value;
                _version++;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }

            set
            {
                if (value == null && default(T) != null)
                    throw new ArgumentNullException(nameof(value));

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

        public int Count
        {
            get
            {
                return _root.Count;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return false;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public void Add(T item)
        {
            _root = Node.Insert(_root, _branchingFactor, Count, item);
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
                    return Contains(default(T));
            }
            else if (value is T)
            {
                return Contains((T)value);
            }

            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Not enough space is available in the destination array.", "arrayIndex");

            for (int i = 0; i < Count; i++)
                array[arrayIndex + i] = this[i];
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return _root.IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            if (value == null)
            {
                if (default(T) == null)
                    return IndexOf(default(T));
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

        void IList.Insert(int index, object value)
        {
            if (value == null && default(T) != null)
                throw new ArgumentNullException(nameof(value));

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
    }
}
