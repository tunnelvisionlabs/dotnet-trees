// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using IDictionary = System.Collections.IDictionary;

    public partial class SortedTreeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        private readonly IComparer<TKey> _comparer;
        private readonly SortedTreeSet<KeyValuePair<TKey, TValue>> _treeSet;

        public SortedTreeDictionary()
            : this(default(IComparer<TKey>))
        {
        }

        public SortedTreeDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : this(collection, comparer: null)
        {
        }

        public SortedTreeDictionary(IComparer<TKey> comparer)
        {
            _comparer = comparer ?? Comparer<TKey>.Default;
            _treeSet = new SortedTreeSet<KeyValuePair<TKey, TValue>>(new KeyComparer(_comparer));
        }

        public SortedTreeDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> comparer)
            : this(comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public SortedTreeDictionary(int branchingFactor)
            : this(branchingFactor, comparer: default)
        {
        }

        public SortedTreeDictionary(int branchingFactor, IComparer<TKey> comparer)
        {
            _comparer = comparer ?? Comparer<TKey>.Default;
            _treeSet = new SortedTreeSet<KeyValuePair<TKey, TValue>>(branchingFactor, new KeyComparer(_comparer));
        }

        public SortedTreeDictionary(int branchingFactor, IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> comparer)
            : this(branchingFactor, comparer)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public IComparer<TKey> Comparer => _comparer;

        public int Count => _treeSet.Count;

        public KeyCollection Keys => new KeyCollection(this);

        public ValueCollection Values => new ValueCollection(this);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        ICollection IDictionary.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        ICollection IDictionary.Values => Values;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        bool IDictionary.IsFixedSize => false;

        bool IDictionary.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => this;

        public TValue this[TKey key]
        {
            get
            {
                if (!_treeSet.TryGetValue(new KeyValuePair<TKey, TValue>(key, default), out KeyValuePair<TKey, TValue> value))
                    throw new KeyNotFoundException();

                return value.Value;
            }

            set
            {
                _treeSet.Remove(new KeyValuePair<TKey, TValue>(key, default));
                _treeSet.Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (key is TKey typedKey && TryGetValue(typedKey, out TValue value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (value == null && default(TValue) != null)
                    throw new ArgumentException(nameof(value), nameof(value));

                try
                {
                    var typedKey = (TKey)key;
                    try
                    {
                        this[typedKey] = (TValue)value;
                    }
                    catch (InvalidCastException)
                    {
                        throw new ArgumentException(nameof(value), nameof(value));
                    }
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException(nameof(key), nameof(key));
                }
            }
        }

        public bool ContainsKey(TKey key) => _treeSet.Contains(new KeyValuePair<TKey, TValue>(key, default));

        public int IndexOfKey(TKey key) => _treeSet.IndexOf(new KeyValuePair<TKey, TValue>(key, default));

        public bool ContainsValue(TValue value) => _treeSet.Any(pair => EqualityComparer<TValue>.Default.Equals(pair.Value, value));

        public int IndexOfValue(TValue value) => _treeSet.FindIndex(pair => EqualityComparer<TValue>.Default.Equals(pair.Value, value));

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_treeSet.TryGetValue(new KeyValuePair<TKey, TValue>(key, default), out KeyValuePair<TKey, TValue> pair))
            {
                value = default;
                return false;
            }

            value = pair.Value;
            return true;
        }

        public Enumerator GetEnumerator() => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

        public void Add(TKey key, TValue value)
        {
            if (!TryAdd(key, value))
                throw new ArgumentException();
        }

        public bool TryAdd(TKey key, TValue value) => _treeSet.Add(new KeyValuePair<TKey, TValue>(key, value));

        public void Clear() => _treeSet.Clear();

        public bool Remove(TKey key) => _treeSet.Remove(new KeyValuePair<TKey, TValue>(key, default));

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return TryGetValue(item.Key, out TValue value)
                && EqualityComparer<TValue>.Default.Equals(value, item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _treeSet.CopyTo(array, arrayIndex);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => GetEnumerator();

        IDictionaryEnumerator IDictionary.GetEnumerator() => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.DictionaryEntry);

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!TryGetValue(item.Key, out TValue value) || !EqualityComparer<TValue>.Default.Equals(value, item.Value))
                return false;

            Remove(item.Key);
            return true;
        }

        void IDictionary.Add(object key, object value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (value == null && default(TValue) != null)
                throw new ArgumentException(nameof(value), nameof(value));

            try
            {
                var typedKey = (TKey)key;
                try
                {
                    Add(typedKey, (TValue)value);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException(nameof(value), nameof(value));
                }
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(nameof(key), nameof(key));
            }
        }

        bool IDictionary.Contains(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return key is TKey typedKey && ContainsKey(typedKey);
        }

        void IDictionary.Remove(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (key is TKey typedKey)
                Remove(typedKey);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Rank != 1)
                throw new ArgumentException(nameof(array));
            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException();
            if (index < 0 || index > array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (array.Length - index < Count)
                throw new ArgumentException();

            if (array is KeyValuePair<TKey, TValue>[] pairs)
            {
                ICollection<KeyValuePair<TKey, TValue>> collection = this;
                collection.CopyTo(pairs, index);
            }
            else if (array is DictionaryEntry[] dictionaryEntryArray)
            {
                int i = index;
                foreach (KeyValuePair<TKey, TValue> pair in this)
                {
                    dictionaryEntryArray[i] = new DictionaryEntry(pair.Key, pair.Value);
                    i++;
                }
            }
            else if (array is object[] objects)
            {
                try
                {
                    int i = index;
                    foreach (KeyValuePair<TKey, TValue> pair in this)
                    {
                        objects[i] = pair;
                        i++;
                    }
                }
                catch (ArrayTypeMismatchException)
                {
                    throw new ArgumentException(nameof(array), nameof(array));
                }
            }
            else
            {
                throw new ArgumentException(nameof(array));
            }
        }

        private sealed class KeyComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            private readonly IComparer<TKey> _comparer;

            internal KeyComparer(IComparer<TKey> comparer)
            {
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");
                _comparer = comparer;
            }

            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return _comparer.Compare(x.Key, y.Key);
            }
        }
    }
}
