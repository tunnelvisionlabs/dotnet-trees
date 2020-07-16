// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableSortedTreeDictionary<TKey, TValue>
    {
        public partial struct KeyCollection
        {
            public struct Enumerator : IEnumerator<TKey>
            {
                private ImmutableSortedTreeDictionary<TKey, TValue>.Enumerator _enumerator;

                internal Enumerator(ImmutableSortedTreeDictionary<TKey, TValue>.Enumerator enumerator)
                {
                    _enumerator = enumerator;
                }

                public TKey Current => _enumerator.Current.Key;

                object IEnumerator.Current => Current;

                public void Dispose() => _enumerator.Dispose();

                public bool MoveNext() => _enumerator.MoveNext();

                public void Reset() => _enumerator.Reset();
            }
        }
    }
}
