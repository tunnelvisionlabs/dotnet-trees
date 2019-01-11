// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public partial class ImmutableTreeList<T>
    {
        public sealed class Builder : IList<T>, IReadOnlyList<T>, IList
        {
            private Node _root;
            private int _version;

            internal Builder(ImmutableTreeList<T> list)
            {
                _root = list._root;
            }

            public int Count => _root.Count;

            bool ICollection<T>.IsReadOnly => false;

            bool IList.IsFixedSize => false;

            bool IList.IsReadOnly => false;

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot => this;

            internal int Version => _version;

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

                    _root = _root.SetItem(index, value);
                    _version++;
                }
            }

            object IList.this[int index]
            {
                get
                {
                    if ((uint)index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return this[index];
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
                _root = Node.Insert(_root, Count, item);
                _version++;
            }

            public void AddRange(IEnumerable<T> items)
            {
                int previousCount = Count;
                _root = Node.InsertRange(_root, previousCount, items);
                if (Count > previousCount)
                    _version++;
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

            public void Clear()
            {
                if (Count != 0)
                {
                    _root = Node.Empty;
                    _version++;
                }
            }

            public bool Contains(T item) => IndexOf(item) >= 0;

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
                    throw new ArgumentOutOfRangeException();
                if (array.Length - arrayIndex < count)
                    throw new ArgumentOutOfRangeException(string.Empty, "Not enough space is available in the destination array.");

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

            public Enumerator GetEnumerator() => new Enumerator(ToImmutable(), this);

            public ImmutableTreeList<T> GetRange(int index, int count) => ToImmutable().GetRange(index, count);

            public int IndexOf(T item) => IndexOf(item, 0, Count, equalityComparer: null);

            public int IndexOf(T item, int index) => IndexOf(item, index, Count - index, equalityComparer: null);

            public int IndexOf(T item, int index, int count) => IndexOf(item, index, count, equalityComparer: null);

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

            public void Insert(int index, T item)
            {
                _root = Node.Insert(_root, index, item);
                _version++;
            }

            public void InsertRange(int index, IEnumerable<T> items)
            {
                _root = Node.InsertRange(_root, index, items);
                _version++;
            }

            public int LastIndexOf(T item) => LastIndexOf(item, Count - 1, Count, equalityComparer: null);

            public int LastIndexOf(T item, int startIndex) => LastIndexOf(item, startIndex, startIndex + 1, equalityComparer: null);

            public int LastIndexOf(T item, int startIndex, int count) => LastIndexOf(item, startIndex, count, equalityComparer: null);

            public int LastIndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer)
            {
                if (Count == 0)
                {
                    // index and count are not validated for empty lists. This strange behavior matches the behavior for
                    // List<T>.
                    return -1;
                }

                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));

                if ((uint)startIndex >= (uint)Count)
                    throw new ArgumentOutOfRangeException(nameof(startIndex));

                if (count < 0 || startIndex - count + 1 < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));

                return _root.LastIndexOf(item, TreeSpan.FromReverseSpan(startIndex, count), equalityComparer ?? EqualityComparer<T>.Default);
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

            public int RemoveAll(Predicate<T> match)
            {
                if (match == null)
                    throw new ArgumentNullException(nameof(match));

                int previousCount = Count;
                _root = Node.RemoveAll(_root, match);
                _version++;
                return previousCount - Count;
            }

            public void RemoveAt(int index)
            {
                _root = Node.RemoveAt(_root, index);
                _version++;
            }

            public void Reverse() => Reverse(0, Count);

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
                    _root = _root.Reverse(new TreeSpan(index, count));
                    _version++;
                }
            }

            public void Sort() => Sort(0, Count, comparer: null);

            public void Sort(IComparer<T> comparer) => Sort(0, Count, comparer);

            public void Sort(Comparison<T> comparison)
            {
                if (comparison == null)
                    throw new ArgumentNullException(nameof(comparison));

                Sort(0, Count, new ComparisonComparer<T>(comparison));
            }

            public void Sort(int index, int count, IComparer<T> comparer)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));
                if (index > Count - count)
                    throw new ArgumentException();

                _root = _root.Sort(new TreeSpan(index, count), comparer ?? Comparer<T>.Default);
                _version++;
            }

            public ImmutableTreeList<T> ToImmutable()
            {
                Node root = _root;
                root.Freeze();
                return new ImmutableTreeList<T>(root);
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
                    throw new ArgumentException($"The value \"{value.GetType()}\" isn't of type \"{typeof(T)}\" and can't be used in this generic collection.", nameof(value));
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

            void IList.Remove(object value)
            {
                int index = ((IList)this).IndexOf(value);
                if (index >= 0)
                    RemoveAt(index);
            }

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
                    throw new ArgumentOutOfRangeException("Not enough space is available in the destination array.", nameof(index));

                int offset = index;
                LeafNode leaf = _root.FirstLeaf;
                while (leaf != null)
                {
                    leaf.CopyToArray(array, offset);
                    offset += leaf.Count;
                    if (offset - index < _root.Count)
                        (leaf, _) = _root.GetLeafNode(offset - index);
                    else
                        leaf = null;
                }
            }

            internal void TrimExcess()
            {
                _root = Node.TrimExcess(_root);
                _version++;
            }

            internal void Validate(ValidationRules validationRules)
            {
                Debug.Assert(_root != null, $"Assertion failed: {nameof(_root)} != null");
                if (_root.FirstChild != null)
                {
                    Debug.Assert(((IndexNode)_root).NodeCount > 1, $"Assertion failed: ((IndexNode)_root).NodeCount > 1");
                }

                _root.Validate(validationRules, null);
            }
        }
    }
}
