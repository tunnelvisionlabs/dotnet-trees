// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    internal sealed class ComparisonComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        public ComparisonComparer(Comparison<T> comparison)
        {
            Debug.Assert(comparison != null, $"Assertion failed: {nameof(comparison)} != null");
            _comparison = comparison;
        }

#pragma warning disable CS8604 // Possible null reference argument. (.NET 5 corrected the signature of Comparison<T>)
        public int Compare([AllowNull] T x, [AllowNull] T y) => _comparison(x, y);
#pragma warning restore CS8604 // Possible null reference argument.
    }
}
