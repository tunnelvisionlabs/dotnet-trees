// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public sealed partial class ImmutableSortedTreeDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
        where TKey : notnull
    {
        public static readonly ImmutableSortedTreeDictionary<TKey, TValue> Empty
            = new ImmutableSortedTreeDictionary<TKey, TValue>(ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>>.Empty.WithComparer(KeyOfPairComparer<TKey, TValue>.Default), keyComparer: null, valueComparer: null);

        private readonly ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>> _treeSet;
        private readonly IComparer<TKey> _keyComparer;
        private readonly IEqualityComparer<TValue> _valueComparer;

        private ImmutableSortedTreeDictionary(ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>> treeSet, IComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
        {
            keyComparer = keyComparer ?? Comparer<TKey>.Default;
            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            Debug.Assert(
                treeSet.KeyComparer is KeyOfPairComparer<TKey, TValue> comparer && comparer.KeyComparer == keyComparer,
                "Assertion failed: treeSet.KeyComparer is KeyOfPairComparer<TKey, TValue> comparer && comparer.KeyComparer == keyComparer");

            _treeSet = treeSet;
            _keyComparer = keyComparer;
            _valueComparer = valueComparer;
        }

        public IComparer<TKey> KeyComparer => _keyComparer;

        public IEqualityComparer<TValue> ValueComparer => _valueComparer;

        public int Count => _treeSet.Count;

        public bool IsEmpty => _treeSet.IsEmpty;

        public KeyCollection Keys => new KeyCollection(this);

        public ValueCollection Values => new ValueCollection(this);

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;

        ICollection IDictionary.Keys => Keys;

        ICollection IDictionary.Values => Values;

        bool IDictionary.IsReadOnly => true;

        bool IDictionary.IsFixedSize => true;

        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => true;

        public TValue this[TKey key]
        {
            get
            {
                if (!_treeSet.TryGetValue(new KeyValuePair<TKey, TValue>(key, default!), out KeyValuePair<TKey, TValue> value))
                    throw new KeyNotFoundException();

                return value.Value;
            }
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get => this[key];
            set => throw new NotSupportedException();
        }

        object? IDictionary.this[object key]
        {
            get
            {
                if (key is null)
                    throw new ArgumentNullException(nameof(key));

                if (key is TKey typedKey && TryGetValue(typedKey, out TValue value))
                    return value;

                return null;
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> Add(TKey key, TValue value)
        {
            ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>> result = _treeSet.Add(new KeyValuePair<TKey, TValue>(key, value));
            if (result == _treeSet)
            {
                if (!ValueComparer.Equals(this[key], value))
                {
                    throw new ArgumentException();
                }

                return this;
            }

            return new ImmutableSortedTreeDictionary<TKey, TValue>(result, _keyComparer, _valueComparer);
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            Builder result = ToBuilder();
            result.AddRange(pairs);
            return result.ToImmutable();
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> Clear()
        {
            if (IsEmpty)
            {
                return this;
            }

            return new ImmutableSortedTreeDictionary<TKey, TValue>(_treeSet.Clear(), _keyComparer, _valueComparer);
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            return TryGetValue(pair.Key, out TValue value)
                && ValueComparer.Equals(value, pair.Value);
        }

        public bool ContainsKey(TKey key)
            => _treeSet.Contains(new KeyValuePair<TKey, TValue>(key, default!));

        public bool ContainsValue(TValue value)
            => _treeSet.Any(pair => ValueComparer.Equals(pair.Value, value));

        public Enumerator GetEnumerator()
            => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

        public ImmutableSortedTreeDictionary<TKey, TValue> Remove(TKey key)
        {
            ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>> result = _treeSet.Remove(new KeyValuePair<TKey, TValue>(key, default!));
            if (result == _treeSet)
            {
                return this;
            }

            return new ImmutableSortedTreeDictionary<TKey, TValue>(result, _keyComparer, _valueComparer);
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
        {
            if (keys is null)
                throw new ArgumentNullException(nameof(keys));

            ImmutableSortedTreeSet<KeyValuePair<TKey, TValue>> result = _treeSet.Except(keys.Select(key => new KeyValuePair<TKey, TValue>(key, default!)));
            if (result == _treeSet)
            {
                return this;
            }

            return new ImmutableSortedTreeDictionary<TKey, TValue>(result, _keyComparer, _valueComparer);
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> SetItem(TKey key, TValue value)
            => Remove(key).Add(key, value);

        public ImmutableSortedTreeDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            Builder result = ToBuilder();
            foreach (var item in items)
            {
                result[item.Key] = item.Value;
            }

            return result.ToImmutable();
        }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public bool TryGetKey(TKey equalKey, [MaybeNullWhen(false)] out TKey actualKey)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            if (!_treeSet.TryGetValue(new KeyValuePair<TKey, TValue>(equalKey, default!), out KeyValuePair<TKey, TValue> value))
            {
                actualKey = default;
                return false;
            }

            actualKey = value.Key;
            return true;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            if (!_treeSet.TryGetValue(new KeyValuePair<TKey, TValue>(key, default!), out KeyValuePair<TKey, TValue> actualValue))
            {
                value = default;
                return false;
            }

            value = actualValue.Value;
            return true;
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> WithComparers(IComparer<TKey>? keyComparer)
            => WithComparers(keyComparer, valueComparer: null);

        public ImmutableSortedTreeDictionary<TKey, TValue> WithComparers(IComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
        {
            keyComparer = keyComparer ?? Comparer<TKey>.Default;
            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            if (IsEmpty)
            {
                if (keyComparer == Empty.KeyComparer && valueComparer == Empty.ValueComparer)
                    return Empty;
                else
                    return new ImmutableSortedTreeDictionary<TKey, TValue>(Empty._treeSet.WithComparer(new KeyOfPairComparer<TKey, TValue>(keyComparer)), keyComparer, valueComparer);
            }

            if (KeyComparer == keyComparer)
            {
                if (ValueComparer == valueComparer)
                {
                    return this;
                }
                else
                {
                    // Don't need to reconstruct the tree because the key comparer is the same
                    return new ImmutableSortedTreeDictionary<TKey, TValue>(_treeSet, keyComparer, valueComparer);
                }
            }

            return ImmutableSortedTreeDictionary.CreateRange(keyComparer, valueComparer, this);
        }

        public Builder ToBuilder()
            => new Builder(this);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear()
            => Clear();

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value)
            => Add(key, value);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
            => AddRange(pairs);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value)
            => SetItem(key, value);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
            => SetItems(items);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys)
            => RemoveRange(keys);

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key) => Remove(key);

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

        IDictionaryEnumerator IDictionary.GetEnumerator()
            => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.DictionaryEntry);

        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(_treeSet.GetEnumerator(), Enumerator.ReturnType.KeyValuePair);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ICollection<KeyValuePair<TKey, TValue>> treeSet = _treeSet;
            treeSet.CopyTo(array, arrayIndex);
        }

        bool IDictionary.Contains(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return key is TKey typedKey && ContainsKey(typedKey);
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

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new NotSupportedException();

        bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new NotSupportedException();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        void IDictionary.Add(object key, object? value) => throw new NotSupportedException();

        void IDictionary.Clear() => throw new NotSupportedException();

        void IDictionary.Remove(object key) => throw new NotSupportedException();
    }
}
