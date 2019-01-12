// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public class ImmutableSortedTreeListBuilderTest
    {
        [Fact]
        public void TestSortedTreeListBuilderConstructor()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            Assert.Equal(Comparer<int>.Default, list.Comparer);

            ImmutableSortedTreeList<string>.Builder stringList = ImmutableSortedTreeList.CreateBuilder(StringComparer.Ordinal);
            Assert.Same(StringComparer.Ordinal, stringList.Comparer);

            stringList = ImmutableSortedTreeList.CreateBuilder(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringList.Comparer);
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            ImmutableSortedTreeList<int>.Enumerator enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            CollectionAssert.EnumeratorInvalidated(list, () => list.Add(1));
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);

            // Reset has no effect due to boxing the value type
            ((IEnumerator<int>)enumerator).Reset();
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
        }
    }
}
