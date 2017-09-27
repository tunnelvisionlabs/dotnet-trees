// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System.Collections;
    using System.Collections.Generic;
    using IDictionaryEnumerator = System.Collections.IDictionaryEnumerator;

    public partial class TreeDictionary<TKey, TValue>
    {
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
        {
            public KeyValuePair<TKey, TValue> Current => throw null;

            object IEnumerator.Current => throw null;

            DictionaryEntry IDictionaryEnumerator.Entry => throw null;

            object IDictionaryEnumerator.Key => throw null;

            object IDictionaryEnumerator.Value => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            void IEnumerator.Reset() => throw null;
        }
    }
}
