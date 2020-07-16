// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeDictionary<TKey, TValue>
    {
        public partial struct ValueCollection
        {
            public struct Enumerator : IEnumerator<TValue>
            {
                private ImmutableTreeDictionary<TKey, TValue>.Enumerator _enumerator;

                internal Enumerator(ImmutableTreeDictionary<TKey, TValue>.Enumerator enumerator)
                {
                    _enumerator = enumerator;
                }

                public TValue Current => _enumerator.Current.Value;

                object IEnumerator.Current => Current;

                public void Dispose() => _enumerator.Dispose();

                public bool MoveNext() => _enumerator.MoveNext();

                public void Reset() => _enumerator.Reset();
            }
        }
    }
}
