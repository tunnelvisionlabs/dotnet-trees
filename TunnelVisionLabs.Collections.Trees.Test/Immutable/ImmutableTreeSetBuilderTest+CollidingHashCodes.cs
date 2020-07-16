// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public partial class ImmutableTreeSetBuilderTest
    {
        public class CollidingHashCodes : AbstractSetTest
        {
            /// <summary>
            /// Tests <see cref="ImmutableTreeSet{T}.Builder.TryGetValue"/> when element hash codes collide.
            /// </summary>
            /// <seealso cref="ImmutableTreeSetBuilderTest.TestTryGetValue"/>
            [Fact]
            public void TestTryGetValueWithCollidingHashCodes()
            {
                ImmutableTreeSet<string>.Builder set = ImmutableTreeSet.CreateBuilder(new ZeroHashCodeEqualityComparer<string>(StringComparer.OrdinalIgnoreCase));
                Assert.True(set.Add("a"));
                Assert.False(set.Add("A"));

                Assert.True(set.TryGetValue("a", out string value));
                Assert.Equal("a", value);

                Assert.True(set.TryGetValue("A", out value));
                Assert.Equal("a", value);

                Assert.False(set.TryGetValue("b", out value));
                Assert.Null(value);

                // The test below forces coverage of an edge case. We don't know if the hash code for 'aa' or 'bb' comes
                // first, so write the test in a way that either will cover the early-exit branch in TryGetValue.
                set = ImmutableTreeSet.CreateBuilder(new SubsetHashCodeEqualityComparer<string>(StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase));
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
                EqualityComparer<int> equalityComparer = EqualityComparer<int>.Default;
                Func<int, int> getHashCode = value => Math.Abs(value) < 5 ? 0 : 1;
                ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder(new SubsetHashCodeEqualityComparer<int>(equalityComparer, getHashCode));
                ImmutableTreeSet<int>.Builder other = ImmutableTreeSet.CreateBuilder(set.KeyComparer);

                set.UnionWith(Enumerable.Range(0, 10));
                other.UnionWith(new[] { 4, 3, 5 });
                set.IntersectWith(other);
                Assert.Equal(new[] { 3, 4, 5 }, set);
            }

            [Fact]
            public void TestRemoveEdgeCase()
            {
                EqualityComparer<int> equalityComparer = EqualityComparer<int>.Default;
                Func<int, int> getHashCode = value => Math.Abs(value) < 5 ? 0 : 1;
                ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder(new SubsetHashCodeEqualityComparer<int>(equalityComparer, getHashCode));

                set.UnionWith(Enumerable.Range(0, 10));
                Assert.True(set.Remove(4));
                Assert.False(set.Remove(4));
            }

            [Fact]
            public void TestSetEqualsEdgeCase()
            {
                EqualityComparer<int> equalityComparer = EqualityComparer<int>.Default;
                Func<int, int> getHashCode = value => Math.Abs(value) < 5 ? 0 : 1;
                ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder(new SubsetHashCodeEqualityComparer<int>(equalityComparer, getHashCode));

                set.UnionWith(new[] { 1, 3, 7, 9 });
                Assert.False(set.SetEquals(new[] { 1, 4, 7, 9 }));
            }

            protected override ISet<T> CreateSet<T>()
            {
                return ImmutableTreeSet.CreateBuilder(ZeroHashCodeEqualityComparer<T>.Default);
            }
        }
    }
}
