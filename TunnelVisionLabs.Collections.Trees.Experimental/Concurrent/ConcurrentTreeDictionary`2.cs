// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Concurrent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class ConcurrentTreeDictionary<TKey, TValue>
        : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
        where TKey : notnull
    {
        public ConcurrentTreeDictionary() => throw null!;

        public ConcurrentTreeDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) => throw null!;

        public ConcurrentTreeDictionary(IEqualityComparer<TKey>? comparer) => throw null!;

        public ConcurrentTreeDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) => throw null!;

        public ConcurrentTreeDictionary(int concurrencyLevel, int capacity) => throw null!;

        public ConcurrentTreeDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) => throw null!;

        public ConcurrentTreeDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey>? comparer) => throw null!;

        public int Count => throw null!;

        public bool IsEmpty => throw null!;

        public KeyCollection Keys => throw null!;

        public ValueCollection Values => throw null!;

        bool ICollection.IsSynchronized => throw null!;

        object ICollection.SyncRoot => throw null!;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => throw null!;

        bool IDictionary.IsFixedSize => throw null!;

        bool IDictionary.IsReadOnly => throw null!;

        ICollection IDictionary.Keys => throw null!;

        ICollection IDictionary.Values => throw null!;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw null!;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw null!;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw null!;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw null!;

        public TValue this[TKey key]
        {
            get => throw null!;
            set => throw null!;
        }

        object? IDictionary.this[object key]
        {
            get => throw null!;
            set => throw null!;
        }

        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory) => throw null!;

        public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory) => throw null!;

        public TValue AddOrUpdate<TArg>(TKey key, Func<TKey, TArg, TValue> addValueFactory, Func<TKey, TValue, TArg, TValue> updateValueFactory, TArg factoryArgument) => throw null!;

        public void Clear() => throw null!;

        public bool ContainsKey(TKey key) => throw null!;

        public Enumerator GetEnumerator() => throw null!;

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory) => throw null!;

        public TValue GetOrAdd(TKey key, TValue value) => throw null!;

        public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> valueFactory, TArg factoryArgument) => throw null!;

        public KeyValuePair<TKey, TValue>[] ToArray() => throw null!;

        public bool TryAdd(TKey key, TValue value) => throw null!;

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => throw null!;

        public bool TryRemove(TKey key, [MaybeNullWhen(false)] out TValue value) => throw null!;

        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue) => throw null!;

        void ICollection.CopyTo(Array array, int index) => throw null!;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw null!;

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => throw null!;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw null!;

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw null!;

        void IDictionary.Add(object key, object? value) => throw null!;

        bool IDictionary.Contains(object key) => throw null!;

        IDictionaryEnumerator IDictionary.GetEnumerator() => throw null!;

        void IDictionary.Remove(object key) => throw null!;

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw null!;

        bool IDictionary<TKey, TValue>.Remove(TKey key) => throw null!;

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw null!;

        IEnumerator IEnumerable.GetEnumerator() => throw null!;
    }
}
