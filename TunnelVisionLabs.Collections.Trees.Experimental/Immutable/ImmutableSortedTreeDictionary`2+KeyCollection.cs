// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableSortedTreeDictionary<TKey, TValue>
    {
        public partial struct KeyCollection : IReadOnlyCollection<TKey>, ICollection<TKey>
        {
            public int Count => throw null;

            bool ICollection<TKey>.IsReadOnly => throw null;

            public Enumerator GetEnumerator() => throw null;

            public bool Contains(TKey item) => throw null;

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex) => throw null;

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

            void ICollection<TKey>.Clear() => throw new NotSupportedException();

            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException();
        }
    }
}
