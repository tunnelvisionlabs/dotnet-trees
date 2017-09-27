// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class SortedTreeDictionary<TKey, TValue>
    {
        public partial struct ValueCollection
        {
            public struct Enumerator : IEnumerator<TValue>
            {
                public TValue Current => throw null;

                object IEnumerator.Current => throw null;

                public void Dispose() => throw null;

                public bool MoveNext() => throw null;

                void IEnumerator.Reset() => throw null;
            }
        }
    }
}
