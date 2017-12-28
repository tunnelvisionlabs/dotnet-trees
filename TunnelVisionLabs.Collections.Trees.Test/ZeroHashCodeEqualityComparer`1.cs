// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test
{
    using System.Collections.Generic;

    internal sealed class ZeroHashCodeEqualityComparer<T> : IEqualityComparer<T>
    {
        public static readonly ZeroHashCodeEqualityComparer<T> Default = new ZeroHashCodeEqualityComparer<T>(null);
        private readonly IEqualityComparer<T> _comparer;

        public ZeroHashCodeEqualityComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(T x, T y) => _comparer.Equals(x, y);

        public int GetHashCode(T obj) => 0;
    }
}
