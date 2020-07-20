// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal sealed class ReverseComparer<T> : IComparer<T>
    {
        public static readonly ReverseComparer<T> Default = new ReverseComparer<T>(null);

        private readonly IComparer<T> _comparer;

        public ReverseComparer(IComparer<T>? comparer)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public int Compare([AllowNull] T x, [AllowNull] T y)
        {
            var direct = _comparer.Compare(x, y);
            if (direct == int.MinValue)
                return int.MaxValue;

            return -direct;
        }
    }
}
