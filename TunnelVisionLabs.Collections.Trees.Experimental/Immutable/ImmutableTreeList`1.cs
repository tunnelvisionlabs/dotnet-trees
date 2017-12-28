// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;

    public sealed partial class ImmutableTreeList<T> : IImmutableList<T>, IReadOnlyList<T>, IList<T>, IList
    {
        public static readonly ImmutableTreeList<T> Empty = new ImmutableTreeList<T>(Node.Empty);
        private readonly Node _root;

        private ImmutableTreeList(Node root)
        {
            Debug.Assert(root != null, $"Assertion failed: {nameof(root)} != null");
            Debug.Assert(root.IsFrozen, $"Assertion failed: {nameof(root)}.{nameof(root.IsFrozen)}");
            _root = root;
        }

        public int Count => _root.Count;

        public bool IsEmpty => Count == 0;

        bool ICollection<T>.IsReadOnly => true;

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => this;

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

        public ImmutableTreeList<T> Add(T value)
        {
            Node root = Node.Insert(_root, Count, value);
            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> AddRange(IEnumerable<T> items)
        {
            Node root = Node.InsertRange(_root, _root.Count, items);
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public int BinarySearch(T item) => BinarySearch(0, Count, item, comparer: null);

        public int BinarySearch(T item, IComparer<T> comparer) => BinarySearch(0, Count, item, comparer);

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

        public ImmutableTreeList<T> Clear() => Empty;

        public bool Contains(T value) => IndexOf(value) >= 0;

        public ImmutableTreeList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter)
        {
            ImmutableTreeList<TOutput>.Node newRoot = _root.ConvertAll(converter);
            newRoot.Freeze();
            return new ImmutableTreeList<TOutput>(newRoot);
        }

        public void CopyTo(T[] array) => CopyTo(0, array, 0, Count);

        public void CopyTo(T[] array, int arrayIndex) => CopyTo(0, array, arrayIndex, Count);

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (Count - index < count)
                throw new ArgumentException();
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("Not enough space is available in the destination array.", string.Empty);

            for (int i = 0; i < count; i++)
            {
                array[arrayIndex + i] = this[index + i];
            }
        }

        public bool Exists(Predicate<T> match) => FindIndex(match) >= 0;

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

        public ImmutableTreeList<T> FindAll(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            return ImmutableTreeList.CreateRange(this.Where(i => match(i)));
        }

        public int FindIndex(Predicate<T> match) => FindIndex(0, Count, match);

        public int FindIndex(int startIndex, Predicate<T> match) => FindIndex(startIndex, Count - startIndex, match);

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

        public int FindLastIndex(Predicate<T> match) => FindLastIndex(Count - 1, Count, match);

        public int FindLastIndex(int startIndex, Predicate<T> match) => FindLastIndex(startIndex, startIndex + 1, match);

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

        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in this)
            {
                action(item);
            }
        }

        public Enumerator GetEnumerator() => new Enumerator(this);

        public ImmutableTreeList<T> GetRange(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            ImmutableTreeList<T> result = RemoveRange(index + count, Count - index - count);
            return RemoveRange(0, index);
        }

        public int IndexOf(T value) => IndexOf(value, 0, Count, equalityComparer: null);

        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentOutOfRangeException();

            return _root.IndexOf(item, new TreeSpan(index, count), equalityComparer ?? EqualityComparer<T>.Default);
        }

        public ImmutableTreeList<T> Insert(int index, T item)
        {
            Node root = Node.Insert(_root, index, item);
            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> InsertRange(int index, IEnumerable<T> items)
        {
            Node root = Node.InsertRange(_root, index, items);
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer)
        {
            if (Count == 0)
            {
                // index and count are not validated for empty lists. This strange behavior matches the behavior for
                // List<T>.
                return -1;
            }

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if ((uint)index >= (uint)Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0 || index - count + 1 < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            return _root.LastIndexOf(item, TreeSpan.FromReverseSpan(index, count), equalityComparer ?? EqualityComparer<T>.Default);
        }

        public ImmutableTreeList<T> Remove(T value) => Remove(value, equalityComparer: null);

        public ImmutableTreeList<T> Remove(T value, IEqualityComparer<T> equalityComparer)
        {
            int index = IndexOf(value, 0, Count, equalityComparer);
            if (index >= 0)
                return RemoveAt(index);

            return this;
        }

        public ImmutableTreeList<T> RemoveAll(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var root = Node.RemoveAll(_root, match);
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> RemoveAt(int index)
        {
            var root = Node.RemoveAt(_root, index);
            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> RemoveRange(IEnumerable<T> items) => RemoveRange(items, equalityComparer: null);

        public ImmutableTreeList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> RemoveRange(int index, int count)
        {
            var root = Node.RemoveRange(_root, index, count);
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> Replace(T oldValue, T newValue) => Replace(oldValue, newValue, equalityComparer: null);

        public ImmutableTreeList<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> Reverse() => Reverse(0, Count);

        public ImmutableTreeList<T> Reverse(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            Node root = _root.Reverse(new TreeSpan(index, count));
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> SetItem(int index, T value)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (index >= Count)
                throw new ArgumentOutOfRangeException($"{nameof(index)} must be less than {nameof(Count)}", nameof(index));

            Node root = _root.SetItem(index, value);
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public ImmutableTreeList<T> Sort() => Sort(0, Count, comparer: null);

        public ImmutableTreeList<T> Sort(IComparer<T> comparer) => Sort(0, Count, comparer);

        public ImmutableTreeList<T> Sort(Comparison<T> comparison)
        {
            if (comparison == null)
                throw new ArgumentNullException(nameof(comparison));

            return Sort(0, Count, new ComparisonComparer<T>(comparison));
        }

        public ImmutableTreeList<T> Sort(int index, int count, IComparer<T> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index > Count - count)
                throw new ArgumentException();

            Node root = _root.Sort(new TreeSpan(index, count), comparer ?? Comparer<T>.Default);
            if (root == _root)
                return this;

            root.Freeze();
            return new ImmutableTreeList<T>(root);
        }

        public Builder ToBuilder() => new Builder(this);

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

        IImmutableList<T> IImmutableList<T>.Clear() => Clear();

        IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);

        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) => Remove(value, equalityComparer);

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) => RemoveRange(items, equalityComparer);

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) => Replace(oldValue, newValue, equalityComparer);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection<T>.Add(T item) => throw new NotSupportedException();

        void ICollection<T>.Clear() => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

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

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", nameof(array));
            if (array.Length - index < Count)
                throw new ArgumentException("Not enough space is available in the destination array.", nameof(index));

            try
            {
                for (int i = 0; i < Count; i++)
                {
                    array.SetValue(this[i], i + index);
                }
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type");
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException("Invalid array type");
            }
        }

        internal void Validate(ValidationRules validationRules)
        {
            Debug.Assert(_root != null, $"Assertion failed: {nameof(_root)} != null");
            Debug.Assert(_root.IsFrozen, $"Assertion failed: {nameof(_root)}.{nameof(_root.IsFrozen)}");
            if (_root.FirstChild != null)
            {
                Debug.Assert(_root.NodeCount > 1, $"Assertion failed: _root.NodeCount > 1");
            }

            _root.Validate(validationRules, null);
        }
    }
}
