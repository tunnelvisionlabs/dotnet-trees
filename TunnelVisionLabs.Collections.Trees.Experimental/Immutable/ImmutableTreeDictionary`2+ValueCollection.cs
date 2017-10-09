// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeDictionary<TKey, TValue>
    {
        public partial struct ValueCollection : IReadOnlyCollection<TValue>, ICollection<TValue>
        {
            public int Count => throw null;

            bool ICollection<TValue>.IsReadOnly => throw null;

            public Enumerator GetEnumerator() => throw null;

            public bool Contains(TValue item) => throw null;

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex) => throw null;

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();

            void ICollection<TValue>.Clear() => throw new NotSupportedException();

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();
        }
    }
}
