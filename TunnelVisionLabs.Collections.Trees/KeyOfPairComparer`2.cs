// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class KeyOfPairComparer<TKey, TValue> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        internal KeyOfPairComparer(IEqualityComparer<TKey> comparer)
        {
            Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");
            KeyComparer = comparer;
        }

        internal static KeyOfPairComparer<TKey, TValue> Default { get; }
            = new KeyOfPairComparer<TKey, TValue>(EqualityComparer<TKey>.Default);

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
