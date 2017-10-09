// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeDictionary<TKey, TValue>
    {
        public sealed class Builder : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
        {
            public IEqualityComparer<TKey> KeyComparer => throw null;

            public IEqualityComparer<TValue> ValueComparer => throw null;

            public int Count => throw null;

            public IEnumerable<TKey> Keys => throw null;

            public IEnumerable<TValue> Values => throw null;

            ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw null;

            ICollection<TValue> IDictionary<TKey, TValue>.Values => throw null;

            int ICollection<KeyValuePair<TKey, TValue>>.Count => throw null;

            bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => throw null;

            ICollection IDictionary.Keys => throw null;

            ICollection IDictionary.Values => throw null;

            bool IDictionary.IsReadOnly => throw null;

            bool IDictionary.IsFixedSize => throw null;

            object ICollection.SyncRoot => throw null;

            bool ICollection.IsSynchronized => throw null;

            public TValue this[TKey key]
            {
                get => throw null;
                set => throw null;
            }

            object IDictionary.this[object key]
            {
                get => throw null;
                set => throw null;
            }

            public void Add(TKey key, TValue value) => throw null;

            public void Add(KeyValuePair<TKey, TValue> item) => throw null;

            public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

            public void Clear() => throw null;

            public bool Contains(KeyValuePair<TKey, TValue> item) => throw null;

            public bool ContainsKey(TKey key) => throw null;

            public bool ContainsValue(TValue value) => throw null;

            public Enumerator GetEnumerator() => throw null;

            public TValue GetValueOrDefault(TKey key) => throw null;

            public TValue GetValueOrDefault(TKey key, TValue defaultValue) => throw null;

            public bool Remove(TKey key) => throw null;

            public bool Remove(KeyValuePair<TKey, TValue> item) => throw null;

            public void RemoveRange(IEnumerable<TKey> keys) => throw null;

            public bool TryGetKey(TKey equalKey, out TKey actualKey) => throw null;

            public bool TryGetValue(TKey key, out TValue value) => throw null;

            public ImmutableTreeDictionary<TKey, TValue> ToImmutable() => throw null;

            void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw null;

            IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            bool IDictionary.Contains(object key) => throw null;

            void IDictionary.Add(object key, object value) => throw null;

            IDictionaryEnumerator IDictionary.GetEnumerator() => throw null;

            void IDictionary.Remove(object key) => throw null;

            void ICollection.CopyTo(Array array, int index) => throw null;
        }
    }
}
