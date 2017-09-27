// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class SortedTreeDictionary<TKey, TValue>
    {
        public partial struct KeyCollection : ICollection<TKey>, IReadOnlyCollection<TKey>, ICollection
        {
            public int Count => throw null;

            bool ICollection<TKey>.IsReadOnly => throw null;

            object ICollection.SyncRoot => throw null;

            bool ICollection.IsSynchronized => throw null;

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

            public void Clear() => throw null;

            public bool Contains(TKey item) => throw null;

            public void CopyTo(TKey[] array, int arrayIndex) => throw null;

            public Enumerator GetEnumerator() => throw null;

            public bool Remove(TKey item) => throw null;

            void ICollection.CopyTo(Array array, int index) => throw null;

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;
        }
    }
}
