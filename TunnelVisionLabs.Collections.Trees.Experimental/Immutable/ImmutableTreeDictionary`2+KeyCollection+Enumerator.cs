// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeDictionary<TKey, TValue>
    {
        public partial struct KeyCollection
        {
            public struct Enumerator : IEnumerator<TKey>
            {
                public TKey Current => throw null;

                object IEnumerator.Current => throw null;

                public void Dispose() => throw null;

                public bool MoveNext() => throw null;

                public void Reset() => throw null;
            }
        }
    }
}
