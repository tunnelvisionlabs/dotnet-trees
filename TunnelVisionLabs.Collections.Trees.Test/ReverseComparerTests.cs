// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System.Collections.Generic;
    using Xunit;

    public class ReverseComparerTests
    {
        private static readonly IComparer<int> DifferencingComparer =
            new ComparisonComparer<int>((x, y) => x - y);

        [Fact]
        public void TestMinValueHandling()
        {
            Assert.Equal(int.MinValue, DifferencingComparer.Compare(0, int.MinValue));
            Assert.Equal(int.MaxValue, new ReverseComparer<int>(DifferencingComparer).Compare(0, int.MinValue));
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.True(Comparer<int>.Default.Compare(0, 1) < 0);
            Assert.True(ReverseComparer<int>.Default.Compare(0, 1) > 0);

            Assert.True(Comparer<int>.Default.Compare(1, 0) > 0);
            Assert.True(ReverseComparer<int>.Default.Compare(1, 0) < 0);

            Assert.True(Comparer<int>.Default.Compare(0, 0) == 0);
            Assert.True(ReverseComparer<int>.Default.Compare(0, 0) == 0);
        }
    }
}
