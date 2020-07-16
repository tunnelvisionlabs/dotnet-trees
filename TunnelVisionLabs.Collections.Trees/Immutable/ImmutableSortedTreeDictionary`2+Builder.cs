// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ImmutableSortedTreeDictionary<TKey, TValue>
    {
        public sealed partial class Builder : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
        {
            private ImmutableSortedTreeDictionary<TKey, TValue> _dictionary;
            private readonly ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>>.Builder _treeSetBuilder;

            internal Builder(ImmutableSortedTreeDictionary<TKey, TValue> dictionary)
            {
                _dictionary = dictionary;
                _treeSetBuilder = _dictionary._treeSet.ToBuilder();
            }

            public IComparer<TKey> KeyComparer => _dictionary.KeyComparer;

            public IEqualityComparer<TValue> ValueComparer => _dictionary.ValueComparer;

            public int Count => _treeSetBuilder.Count;

            public KeyCollection Keys => new KeyCollection(this);

            public ValueCollection Values => new ValueCollection(this);

            IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

            IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

            ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

            ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

            bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

            ICollection IDictionary.Keys => Keys;

            ICollection IDictionary.Values => Values;

            bool IDictionary.IsReadOnly => false;

            bool IDictionary.IsFixedSize => false;

            object ICollection.SyncRoot => this;

            bool ICollection.IsSynchronized => false;

            public TValue this[TKey key]
            {
                get
                {
                    if (!TryGetValue(key, out TValue value))
                        throw new KeyNotFoundException();

                    return value;
                }

                set
                {
                    _treeSetBuilder.Remove(new KeyValuePair<TKey, TValue>(key, default));
                    _treeSetBuilder.Add(new KeyValuePair<TKey, TValue>(key, value));
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

            public void Add(TKey key, TValue value)
            {
                if (!_treeSetBuilder.Add(new KeyValuePair<TKey, TValue>(key, value)))
                {
                    if (!ValueComparer.Equals(this[key], value))
                    {
                        throw new ArgumentException();
                    }
                }
            }

            public void Add(KeyValuePair<TKey, TValue> item)
                => Add(item.Key, item.Value);

            public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
            {
                if (items == null)
                    throw new ArgumentNullException(nameof(items));

                foreach (KeyValuePair<TKey, TValue> pair in items)
                    Add(pair.Key, pair.Value);
            }

            public void Clear()
                => _treeSetBuilder.Clear();

            public bool Contains(KeyValuePair<TKey, TValue> item)
            {
                return TryGetValue(item.Key, out TValue value)
                    && ValueComparer.Equals(value, item.Value);
            }

            public bool ContainsKey(TKey key)
                => _treeSetBuilder.Contains(new KeyValuePair<TKey, TValue>(key, default));

            public bool ContainsValue(TValue value)
                => _treeSetBuilder.Any(pair => ValueComparer.Equals(pair.Value, value));

            public Enumerator GetEnumerator()
                => new Enumerator(_treeSetBuilder.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

            public TValue GetValueOrDefault(TKey key)
                => GetValueOrDefault(key, default);

            public TValue GetValueOrDefault(TKey key, TValue defaultValue)
            {
                if (TryGetValue(key, out TValue value))
                    return value;

                return defaultValue;
            }

            public bool Remove(TKey key)
                => _treeSetBuilder.Remove(new KeyValuePair<TKey, TValue>(key, default));

            public bool Remove(KeyValuePair<TKey, TValue> item)
            {
                if (!Contains(item))
                {
                    return false;
                }

                _treeSetBuilder.Remove(item);
                return true;
            }

            public void RemoveRange(IEnumerable<TKey> keys)
            {
                if (keys is null)
                    throw new ArgumentNullException(nameof(keys));

                _treeSetBuilder.ExceptWith(keys.Select(key => new KeyValuePair<TKey, TValue>(key, default)));
            }

            public bool TryGetKey(TKey equalKey, out TKey actualKey)
                => ToImmutable().TryGetKey(equalKey, out actualKey);

            public bool TryGetValue(TKey key, out TValue value)
                => ToImmutable().TryGetValue(key, out value);

            public ImmutableSortedTreeDictionary<TKey, TValue> ToImmutable()
            {
                ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>> treeSet = _treeSetBuilder.ToImmutable();
                if (_dictionary._treeSet != treeSet)
                {
                    _dictionary = new ImmutableSortedTreeDictionary<TKey, TValue>(treeSet, KeyComparer, ValueComparer);
                }

                return _dictionary;
            }

            void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            {
                ICollection<KeyValuePair<TKey, TValue>> collection = _treeSetBuilder;
                collection.CopyTo(array, arrayIndex);
            }

            IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
                => new Enumerator(_treeSetBuilder.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

            IEnumerator IEnumerable.GetEnumerator()
                => new Enumerator(_treeSetBuilder.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

            bool IDictionary.Contains(object key)
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                return key is TKey typedKey && ContainsKey(typedKey);
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

            IDictionaryEnumerator IDictionary.GetEnumerator()
                => new Enumerator(_treeSetBuilder.GetEnumerator(), Enumerator.ReturnType.DictionaryEntry);

            void IDictionary.Remove(object key)
            {
                if (key == null)
                    throw new ArgumentNullException(nameof(key));

                if (!(key is TKey typedKey))
                    return;

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
        }
    }
}
