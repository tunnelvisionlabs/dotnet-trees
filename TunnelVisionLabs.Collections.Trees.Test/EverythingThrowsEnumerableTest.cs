// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using Xunit;
    using IEnumerable = System.Collections.IEnumerable;

    public class EverythingThrowsEnumerableTest
    {
        [Fact]
        public void TestEverythingThrows()
        {
            TestEverythingThrowsImpl<int>();
            TestEverythingThrowsImpl<int?>();
            TestEverythingThrowsImpl<object>();
            TestEverythingThrowsImpl<string>();
        }

        private void TestEverythingThrowsImpl<T>()
        {
            Assert.NotNull(EverythingThrowsEnumerable<T>.Instance);
            Assert.Same(EverythingThrowsEnumerable<T>.Instance, EverythingThrowsEnumerable<T>.Instance);
            Assert.Throws<NotSupportedException>(() => EverythingThrowsEnumerable<T>.Instance.GetEnumerator());

            IEnumerable<T> enumerableT = EverythingThrowsEnumerable<T>.Instance;
            Assert.Throws<NotSupportedException>(() => enumerableT.GetEnumerator());

            IEnumerable enumerable = EverythingThrowsEnumerable<T>.Instance;
            Assert.Throws<NotSupportedException>(() => enumerable.GetEnumerator());
        }
    }
}
