// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public class FixedArray8Test
    {
        [Fact]
        public void TestBoundsCheck()
        {
            FixedArray8<int> array = default;
            Assert.Throws<IndexOutOfRangeException>(() => array[-1]);
            Assert.Throws<IndexOutOfRangeException>(() => array[-1] = 0);
            Assert.Throws<IndexOutOfRangeException>(() => array[array.Length]);
            Assert.Throws<IndexOutOfRangeException>(() => array[array.Length] = 0);
        }

        [Fact]
        public void TestSortSmallRange()
        {
            FixedArray8<int> array = default;
            array.Sort(0, 0, Comparer<int>.Default);
            array.Sort(1, 1, Comparer<int>.Default);
            Assert.Equal(default, array);
        }
    }
}
