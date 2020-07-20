// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public partial class TreeSetWithCollidingHashCodesTest : AbstractSetTest
    {
        /// <summary>
        /// Tests <see cref="Trees.TreeSet{T}.TryGetValue"/> when element hash codes collide.
        /// </summary>
        /// <seealso cref="TreeSetTest.TestTryGetValue"/>
        [Fact]
        public void TestTryGetValueWithCollidingHashCodes()
        {
            var set = new TreeSet<string?>(new ZeroHashCodeEqualityComparer<string?>(StringComparer.OrdinalIgnoreCase));
            Assert.True(set.Add("a"));
            Assert.False(set.Add("A"));

            Assert.True(set.TryGetValue("a", out string? value));
            Assert.Equal("a", value);

            Assert.True(set.TryGetValue("A", out value));
            Assert.Equal("a", value);

            Assert.False(set.TryGetValue("b", out value));
            Assert.Null(value);

            // The test below forces coverage of an edge case. We don't know if the hash code for 'aa' or 'bb' comes
            // first, so write the test in a way that either will cover the early-exit branch in TryGetValue.
            set = new TreeSet<string?>(new SubsetHashCodeEqualityComparer<string?>(StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase));
            Assert.True(set.Add("aa"));
            Assert.True(set.Add("Aa"));
            Assert.True(set.Add("bb"));
            Assert.True(set.Add("Bb"));

            Assert.True(set.TryGetValue("aa", out value));
            Assert.Equal("aa", value);

            Assert.True(set.TryGetValue("Aa", out value));
            Assert.Equal("Aa", value);

            Assert.False(set.TryGetValue("AA", out value));
            Assert.Null(value);

            Assert.True(set.TryGetValue("bb", out value));
            Assert.Equal("bb", value);

            Assert.True(set.TryGetValue("Bb", out value));
            Assert.Equal("Bb", value);

            Assert.False(set.TryGetValue("BB", out value));
            Assert.Null(value);
        }

        [Fact]
        public void TestIntersectWithEdgeCase()
        {
            var equalityComparer = EqualityComparer<int>.Default;
            Func<int, int> getHashCode = value => Math.Abs(value) < 5 ? 0 : 1;
            var set = new TreeSet<int>(branchingFactor: 4, comparer: new SubsetHashCodeEqualityComparer<int>(equalityComparer, getHashCode));
            var other = new TreeSet<int>(branchingFactor: 4, comparer: set.Comparer);

            set.UnionWith(Enumerable.Range(0, 10));
            other.UnionWith(new[] { 4, 3, 5 });
            set.IntersectWith(other);
            Assert.Equal(new[] { 3, 4, 5 }, set);
        }

        [Fact]
        public void TestRemoveEdgeCase()
        {
            var equalityComparer = EqualityComparer<int>.Default;
            Func<int, int> getHashCode = value => Math.Abs(value) < 5 ? 0 : 1;
            var set = new TreeSet<int>(branchingFactor: 4, comparer: new SubsetHashCodeEqualityComparer<int>(equalityComparer, getHashCode));

            set.UnionWith(Enumerable.Range(0, 10));
            Assert.True(set.Remove(4));
            Assert.False(set.Remove(4));
        }

        [Fact]
        public void TestSetEqualsEdgeCase()
        {
            var equalityComparer = EqualityComparer<int>.Default;
            Func<int, int> getHashCode = value => Math.Abs(value) < 5 ? 0 : 1;
            var set = new TreeSet<int>(branchingFactor: 4, comparer: new SubsetHashCodeEqualityComparer<int>(equalityComparer, getHashCode));

            set.UnionWith(new[] { 1, 3, 7, 9 });
            Assert.False(set.SetEquals(new[] { 1, 4, 7, 9 }));
        }

        protected override ISet<T> CreateSet<T>()
        {
            return new TreeSet<T>(branchingFactor: 4, comparer: ZeroHashCodeEqualityComparer<T>.Default);
        }
    }
}
