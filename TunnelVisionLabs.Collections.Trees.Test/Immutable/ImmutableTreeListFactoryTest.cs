// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System.Collections.Generic;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    /// <summary>
    /// Contains tests for the <see cref="ImmutableTreeList"/> factory class.
    /// </summary>
    public class ImmutableTreeListFactoryTest
    {
        [Fact]
        public void TestCreateEmpty()
        {
            Assert.NotNull(ImmutableTreeList.Create<int>());
            Assert.Empty(ImmutableTreeList.Create<int>());
            Assert.Same(ImmutableTreeList.Create<int>(), ImmutableTreeList.Create<int>());
            Assert.NotSame(ImmutableTreeList.Create<string>(), ImmutableTreeList.Create<object>());
        }

        [Fact]
        public void TestCreateSingle()
        {
            Assert.NotNull(ImmutableTreeList.Create(1));
            Assert.Single(ImmutableTreeList.Create(1));
            Assert.NotSame(ImmutableTreeList.Create(1), ImmutableTreeList.Create(2));
            Assert.Equal(1, ImmutableTreeList.Create(1)[0]);
        }

        [Fact]
        public void TestCreateMany()
        {
            Assert.NotNull(ImmutableTreeList.Create(1, 5, 4));
            Assert.Equal(3, ImmutableTreeList.Create(1, 5, 4).Count);
            Assert.Equal(new[] { 1, 5, 4 }, ImmutableTreeList.Create(1, 5, 4));
        }

        [Fact]
        public void TestCreateBuilder()
        {
            Assert.NotNull(ImmutableTreeList.CreateBuilder<int>());
            Assert.Empty(ImmutableTreeList.CreateBuilder<int>());
            Assert.NotSame(ImmutableTreeList.CreateBuilder<int>(), ImmutableTreeList.CreateBuilder<int>());

            var builder1 = ImmutableTreeList.CreateBuilder<int>();
            var builder2 = ImmutableTreeList.CreateBuilder<int>();
            Assert.Empty(builder1);
            builder1.Add(1);
            Assert.Single(builder1);
            Assert.Empty(builder2);
        }

        [Fact]
        public void TestCreateRange()
        {
            Assert.NotNull(ImmutableTreeList.CreateRange(new[] { 1, 5, 4 }));
            Assert.Equal(3, ImmutableTreeList.CreateRange(new[] { 1, 5, 4 }).Count);
            Assert.Equal(new[] { 1, 5, 4 }, ImmutableTreeList.CreateRange(new[] { 1, 5, 4 }));
        }

        [Fact]
        public void TestToImmutableTreeList()
        {
            Assert.NotNull(new[] { 1, 5, 4 }.ToImmutableTreeList());
            Assert.Equal(3, new[] { 1, 5, 4 }.ToImmutableTreeList().Count);
            Assert.Equal(new[] { 1, 5, 4 }, new[] { 1, 5, 4 }.ToImmutableTreeList());

            // If the source is already an immutable tree list, the method simply returns the same instance
            IEnumerable<int> source = ImmutableTreeList.Create(1, 5, 4);
            Assert.Same(source, source.ToImmutableTreeList());
        }
    }
}
