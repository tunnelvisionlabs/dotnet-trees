// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using IDictionary = System.Collections.IDictionary;

    public partial class SortedTreeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IDictionary
    {
        public SortedTreeDictionary() => throw null;

        public SortedTreeDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) => throw null;

        public SortedTreeDictionary(IComparer<TKey> comparer) => throw null;

        public SortedTreeDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> comparer) => throw null;

        public SortedTreeDictionary(int branchingFactor) => throw null;

        public SortedTreeDictionary(int branchingFactor, IComparer<TKey> comparer) => throw null;

        public SortedTreeDictionary(int branchingFactor, IEnumerable<KeyValuePair<TKey, TValue>> collection, IComparer<TKey> comparer) => throw null;

        public IComparer<TKey> Comparer => throw null;

        public int Count => throw null;

        public KeyCollection Keys => throw null;

        public ValueCollection Values => throw null;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => throw null;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw null;

        ICollection IDictionary.Keys => throw null;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => throw null;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw null;

        ICollection IDictionary.Values => throw null;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => throw null;

        bool IDictionary.IsFixedSize => throw null;

        bool IDictionary.IsReadOnly => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

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

        public bool ContainsKey(TKey key) => throw null;

        public int IndexOfKey(TKey key) => throw null;

        public bool ContainsValue(TValue value) => throw null;

        public int IndexOfValue(TValue value) => throw null;

        public bool TryGetValue(TKey key, out TValue value) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public void Add(TKey key, TValue value) => throw null;

        public bool TryAdd(TKey key, TValue value) => throw null;

        public void Clear() => throw null;

        public bool Remove(TKey key) => throw null;

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => throw null;

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => throw null;

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => throw null;

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => throw null;

        IDictionaryEnumerator IDictionary.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) => throw null;

        void IDictionary.Add(object key, object value) => throw null;

        bool IDictionary.Contains(object key) => throw null;

        void IDictionary.Remove(object key) => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;
    }
}
