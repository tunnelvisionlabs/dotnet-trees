// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class KeyOfPairComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
    {
        internal KeyOfPairComparer(IComparer<TKey> comparer)
        {
            Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");
            KeyComparer = comparer;
        }

        internal static KeyOfPairComparer<TKey, TValue> Default { get; }
            = new KeyOfPairComparer<TKey, TValue>(Comparer<TKey>.Default);

        internal IComparer<TKey> KeyComparer { get; }

        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            => KeyComparer.Compare(x.Key, y.Key);
    }
}
