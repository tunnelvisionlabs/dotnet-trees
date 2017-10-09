// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableSortedTreeDictionary<TKey, TValue> : IImmutableDictionary<TKey, TValue>, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        public static readonly ImmutableSortedTreeDictionary<TKey, TValue> Empty;

        public IComparer<TKey> KeyComparer => throw null;

        public IEqualityComparer<TValue> ValueComparer => throw null;

        public int Count => throw null;

        public bool IsEmpty => throw null;

        public KeyCollection Keys => throw null;

        public ValueCollection Values => throw null;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw null;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw null;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw null;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw null;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => throw null;

        ICollection IDictionary.Keys => throw null;

        ICollection IDictionary.Values => throw null;

        bool IDictionary.IsReadOnly => throw null;

        bool IDictionary.IsFixedSize => throw null;

        object ICollection.SyncRoot => throw null;

        bool ICollection.IsSynchronized => throw null;

        public TValue this[TKey key] => throw null;

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get => throw null;
            set => throw null;
        }

        object IDictionary.this[object key]
        {
            get => throw null;
            set => throw null;
        }

        public ImmutableSortedTreeDictionary<TKey, TValue> Add(TKey key, TValue value) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> Clear() => throw null;

        public bool Contains(KeyValuePair<TKey, TValue> pair) => throw null;

        public bool ContainsKey(TKey key) => throw null;

        public bool ContainsValue(TValue value) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> Remove(TKey key) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> SetItem(TKey key, TValue value) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public bool TryGetKey(TKey equalKey, out TKey actualKey) => throw null;

        public bool TryGetValue(TKey key, out TValue value) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> WithComparers(IComparer<TKey> keyComparer) => throw null;

        public ImmutableSortedTreeDictionary<TKey, TValue> WithComparers(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public Builder ToBuilder() => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Clear() => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs) => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItem(TKey key, TValue value) => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.RemoveRange(IEnumerable<TKey> keys) => throw null;

        IImmutableDictionary<TKey, TValue> IImmutableDictionary<TKey, TValue>.Remove(TKey key) => throw null;

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw null;

        IDictionaryEnumerator IDictionary.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw null;

        bool IDictionary.Contains(object key) => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => throw new NotSupportedException();

        bool IDictionary<TKey, TValue>.Remove(TKey key) => throw new NotSupportedException();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw new NotSupportedException();

        void IDictionary.Add(object key, object value) => throw new NotSupportedException();

        void IDictionary.Clear() => throw new NotSupportedException();

        void IDictionary.Remove(object key) => throw new NotSupportedException();
    }
}
