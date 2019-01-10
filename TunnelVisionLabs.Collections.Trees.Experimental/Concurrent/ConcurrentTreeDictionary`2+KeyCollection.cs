// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Concurrent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ConcurrentTreeDictionary<TKey, TValue>
    {
        public partial struct KeyCollection : IReadOnlyCollection<TKey>, ICollection<TKey>
        {
            public int Count => throw null!;

            bool ICollection<TKey>.IsReadOnly => throw null!;

            public Enumerator GetEnumerator() => throw null!;

            public bool Contains(TKey item) => throw null!;

            public void Clear() => throw new NotSupportedException();

            public bool Remove(TKey item) => throw new NotSupportedException();

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => throw null!;

            IEnumerator IEnumerable.GetEnumerator() => throw null!;

            void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex) => throw null!;

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();
        }
    }
}
