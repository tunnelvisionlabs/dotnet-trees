// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal sealed class ComparisonComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            Debug.Assert(comparison != null, $"Assertion failed: {nameof(comparison)} != null");
            _comparison = comparison;
        }

        public int Compare(T x, T y) => _comparison(x, y);
    }
}
