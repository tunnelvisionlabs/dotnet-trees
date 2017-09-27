// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class SortedTreeDictionary<TKey, TValue>
    {
        public partial struct ValueCollection : ICollection<TValue>, IReadOnlyCollection<TValue>, ICollection
        {
            public int Count => throw null;

            bool ICollection<TValue>.IsReadOnly => throw null;

            object ICollection.SyncRoot => throw null;

            bool ICollection.IsSynchronized => throw null;

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();

            public void Clear() => throw null;

            public bool Contains(TValue item) => throw null;

            public void CopyTo(TValue[] array, int arrayIndex) => throw null;

            public Enumerator GetEnumerator() => throw null;

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();

            void ICollection.CopyTo(Array array, int index) => throw null;

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;
        }
    }
}
