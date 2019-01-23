// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class KeyOfPairEqualityComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        internal KeyOfPairEqualityComparer(IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");
            KeyComparer = comparer;
        }

        internal static KeyOfPairEqualityComparer<TKey, TValue> Default { get; }
            = new KeyOfPairEqualityComparer<TKey, TValue>(EqualityComparer<TKey>.Default);

        internal IEqualityComparer<TKey> KeyComparer { get; }

        public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return KeyComparer.Equals(x.Key, y.Key);
        }

        public int GetHashCode(KeyValuePair<TKey, TValue> obj)
        {
            return KeyComparer.GetHashCode(obj.Key);
        }
    }
}
