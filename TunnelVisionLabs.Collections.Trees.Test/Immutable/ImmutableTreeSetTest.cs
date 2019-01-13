// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public class ImmutableTreeSetTest
    {
        [Fact]
        public void TestEmptyImmutableTreeSet()
        {
            var list = ImmutableTreeSet.Create<int>();
            Assert.Same(ImmutableTreeSet<int>.Empty, list);
            Assert.Empty(list);
        }

        [Fact]
        public void TestSingleElementList()
        {
            var value = Generator.GetInt32().ToString();
            var list = ImmutableTreeSet.Create(value);
            Assert.Equal(new[] { value }, list);

            list = ImmutableTreeSet.Create(equalityComparer: null, value);
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(new[] { value }, list);

            list = ImmutableTreeSet.Create(StringComparer.OrdinalIgnoreCase, value);
            Assert.Same(StringComparer.OrdinalIgnoreCase, list.KeyComparer);
            Assert.Equal(new[] { value }, list);
        }

        [Fact]
        public void TestMultipleElementList()
        {
            var values = new[] { Generator.GetInt32().ToString(), Generator.GetInt32().ToString(), Generator.GetInt32().ToString() };

            // Construction using ImmutableTreeSet.Create
            var list = ImmutableTreeSet.Create(values);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = ImmutableTreeSet.Create<string>(equalityComparer: null, values);
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = ImmutableTreeSet.Create(StringComparer.OrdinalIgnoreCase, values);
            Assert.Same(StringComparer.OrdinalIgnoreCase, list.KeyComparer);
            Assert.Equal(values.OrderBy(StringComparer.OrdinalIgnoreCase.GetHashCode), list);

            // Construction using ImmutableTreeSet.ToImmutableTreeSet
            list = values.ToImmutableTreeSet();
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = values.ToImmutableTreeSet(equalityComparer: null);
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = values.ToImmutableTreeSet(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, list.KeyComparer);
            Assert.Equal(values.OrderBy(StringComparer.OrdinalIgnoreCase.GetHashCode), list);
        }

        [Fact]
        public void TestImmutableTreeSetCreateValidation()
        {
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.Create(default(int[])));
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.Create(EqualityComparer<int>.Default, default(int[])));
        }

        [Fact]
        public void TestImmutableTreeSetCreateRange()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var list = ImmutableTreeSet.CreateRange(values);
            Assert.Equal(values.OrderBy(x => x), list);
        }

        [Fact]
        public void TestImmutableTreeSetCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.CreateRange<int>(null));
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.CreateRange(EqualityComparer<int>.Default, null));
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<int>).ToImmutableTreeSet());
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<int>).ToImmutableTreeSet(EqualityComparer<int>.Default));
        }
    }
}
