// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public partial class ImmutableSortedTreeSetBuilderTest : AbstractSetTest
    {
        [Fact]
        public void TestImmutableSortedTreeSetBuilderConstructor()
        {
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            Assert.Empty(set);
        }

        [Fact]
        public void TestMinMax()
        {
            Assert.Equal(0, ImmutableSortedTreeSet.CreateBuilder<int>().Min);
            Assert.Equal(0, ImmutableSortedTreeSet.CreateBuilder<int>().Max);
            Assert.Null(ImmutableSortedTreeSet.CreateBuilder<object>().Min);
            Assert.Null(ImmutableSortedTreeSet.CreateBuilder<object>().Max);

            var set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(0, 100).Select(x => Generator.GetInt32())).ToBuilder();
            Assert.Equal(set.Min(), set.Min);
            Assert.Equal(set.Max(), set.Max);
        }

        [Fact]
        public void TestCollectionConstructorUsesCorrectComparer()
        {
            var instance1 = new StrongBox<int>(1);
            var instance2 = new StrongBox<int>(2);
            var comparer = new ComparisonComparer<StrongBox<int>>((x, y) => Comparer<int>.Default.Compare(x.Value, y.Value));
            var objectSet = ImmutableSortedTreeSet.Create(comparer, new[] { instance1, instance2, instance1 }).ToBuilder();
            Assert.Same(comparer, objectSet.KeyComparer);
            Assert.Equal(2, objectSet.Count);
            Assert.Equal(new[] { instance1, instance2 }, objectSet);

            ImmutableSortedTreeSet<string>.Builder stringSet = ImmutableSortedTreeSet.CreateBuilder<string>();
            Assert.Same(Comparer<string>.Default, stringSet.KeyComparer);

            stringSet = ImmutableSortedTreeSet.CreateBuilder(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringSet.KeyComparer);
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.CreateBuilder<object>().KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.CreateBuilder<int>().KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.CreateBuilder<IComparable>().KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.CreateRange<object>(Enumerable.Empty<object>()).ToBuilder().KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.CreateRange<int>(Enumerable.Empty<int>()).ToBuilder().KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.CreateRange<IComparable>(Enumerable.Empty<IComparable>()).ToBuilder().KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.CreateBuilder<object>(comparer: null).KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.CreateBuilder<int>(comparer: null).KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.CreateBuilder<IComparable>(comparer: null).KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.CreateRange<object>(comparer: null, Enumerable.Empty<object>()).ToBuilder().KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.CreateRange<int>(comparer: null, Enumerable.Empty<int>()).ToBuilder().KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.CreateRange<IComparable>(comparer: null, Enumerable.Empty<IComparable>()).ToBuilder().KeyComparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = new ComparisonComparer<object>((x, y) => 0);
            var intComparer = new ComparisonComparer<int>((x, y) => 0);
            var comparableComparer = new ComparisonComparer<IComparable>((x, y) => 0);

            Assert.Same(objComparer, ImmutableSortedTreeSet.CreateBuilder<object>(comparer: objComparer).KeyComparer);
            Assert.Same(intComparer, ImmutableSortedTreeSet.CreateBuilder<int>(comparer: intComparer).KeyComparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeSet.CreateBuilder<IComparable>(comparer: comparableComparer).KeyComparer);

            Assert.Same(objComparer, ImmutableSortedTreeSet.CreateRange<object>(comparer: objComparer, Enumerable.Empty<object>()).ToBuilder().KeyComparer);
            Assert.Same(intComparer, ImmutableSortedTreeSet.CreateRange<int>(comparer: intComparer, Enumerable.Empty<int>()).ToBuilder().KeyComparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeSet.CreateRange<IComparable>(comparer: comparableComparer, Enumerable.Empty<IComparable>()).ToBuilder().KeyComparer);
        }

        [Fact]
        public void TestICollectionTInterface()
        {
            ICollection<int> set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(0, 10)).ToBuilder();
            Assert.False(set.IsReadOnly);

            Assert.Equal(10, set.Count);
            Assert.True(set.Contains(3));
            set.Add(3);
            Assert.True(set.Contains(3));
            Assert.Equal(10, set.Count);

            Assert.False(set.Contains(12));
            set.Add(12);
            Assert.True(set.Contains(12));
            Assert.Equal(11, set.Count);
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(ImmutableSortedTreeSet.Create<int>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableSortedTreeSet.Create<int?>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableSortedTreeSet.Create<object>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: true);

            // Run the same set of tests on SortedSet<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(new SortedSet<int> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: false);
            TestICollectionInterfaceImpl(new SortedSet<int?> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: true);
            TestICollectionInterfaceImpl(new SortedSet<object> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: true);
        }

        [Fact]
        public void TestCopyToValidation()
        {
            var set = ImmutableSortedTreeSet.CreateRange<int>(Enumerable.Range(0, 10)).ToBuilder();
            Assert.Throws<ArgumentNullException>("array", () => ((ICollection<int>)set).CopyTo(array: null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => ((ICollection<int>)set).CopyTo(new int[set.Count], -1));
            Assert.Throws<ArgumentException>(string.Empty, () => ((ICollection<int>)set).CopyTo(new int[set.Count - 1], 0));
            Assert.Throws<ArgumentException>(string.Empty, () => ((ICollection<int>)set).CopyTo(new int[set.Count], 1));
        }

        [Fact]
        public void TestAdd()
        {
            int value = Generator.GetInt32();

            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            Assert.Empty(set);
            set.Add(value);
            Assert.Single(set);
            Assert.Equal(value, set.First());
            int[] expected = { value };
            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddStaysPacked()
        {
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                // This test assumes items are being added are already ordered by hash code
                Assert.Equal(i, i.GetHashCode());

                set.Add(i);
                set.Validate(ValidationRules.RequirePacked);
            }
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            foreach (int item in expected)
            {
                set.Add(item);
            }

            Assert.Equal(expected.Length, set.Count);

            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();

            set.Clear();
            Assert.Empty(set);

            set.UnionWith(Enumerable.Range(0, 10));
            Assert.NotEmpty(set);
            set.Clear();
            Assert.Empty(set);
        }

        [Fact]
        public void TestContains()
        {
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int value = Generator.GetInt32(set.Count + 1);
                set.Add(i);

                // Use set.Contains(i) since this is a targeted collection API test
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
                Assert.True(set.Contains(i));
#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection
            }

            set.Validate(ValidationRules.None);
        }

        [Fact]
        public void TestCopyTo()
        {
            var set = ImmutableSortedTreeSet.CreateRange(comparer: null, Enumerable.Range(0, 100)).ToBuilder();
            var reference = new SortedSet<int>(Enumerable.Range(0, 100));

            int[] listArray = new int[set.Count * 2];
            int[] referenceArray = new int[reference.Count * 2];

            ((ICollection<int>)set).CopyTo(listArray, 0);
            reference.CopyTo(referenceArray);
            Assert.Equal(referenceArray, listArray);

            ((ICollection<int>)set).CopyTo(listArray, set.Count / 2);
            reference.CopyTo(referenceArray, reference.Count / 2);
            Assert.Equal(referenceArray, listArray);
        }

        [Fact]
        public void TestTrimExcess()
        {
            var random = new Random();
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            var reference = new SortedSet<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int value = random.Next(set.Count + 1);
                set.Add(i);
                reference.Add(i);
            }

            set.Validate(ValidationRules.None);

            // In the first call to TrimExcess, items will move
            set.TrimExcess();
            set.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, set);

            // In the second call, the list is already packed so nothing will move
            set.TrimExcess();
            set.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, set);

            ImmutableSortedTreeSet<int>.Builder empty = ImmutableSortedTreeSet.CreateBuilder<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            var single = ImmutableSortedTreeSet.CreateRange<int>(Enumerable.Range(0, 1)).ToBuilder();
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            ImmutableSortedTreeSet<int>.Builder binary = ImmutableSortedTreeSet.CreateBuilder<int>();
            for (int i = 99; i >= 0; i--)
                binary.Add(i);

            binary.TrimExcess();
            binary.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            var ternary = ImmutableSortedTreeSet.CreateRange<int>(comparer: null, Enumerable.Range(0, 100)).ToBuilder();
            for (int i = 99; i >= 0; i--)
                ternary.Add(i);

            ternary.TrimExcess();
            ternary.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestTryGetValue()
        {
            ImmutableSortedTreeSet<string>.Builder set = ImmutableSortedTreeSet.CreateBuilder(StringComparer.OrdinalIgnoreCase);
            Assert.True(set.Add("a"));
            Assert.False(set.Add("A"));

            Assert.True(set.TryGetValue("a", out string value));
            Assert.Equal("a", value);

            Assert.True(set.TryGetValue("A", out value));
            Assert.Equal("a", value);

            Assert.False(set.TryGetValue("b", out value));
            Assert.Null(value);
        }

        [Fact]
        public void TestReverse()
        {
            var random = new Random();
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                set.Add(item);
            }

            IEnumerable<int> reversed = set.Reverse();
            Assert.Equal(set.AsEnumerable().Reverse(), reversed);
            Assert.Equal(set, reversed.Reverse());
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            ImmutableSortedTreeSet<int>.Enumerator enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            CollectionAssert.EnumeratorInvalidated(set, () => set.Add(1));
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);

            // Reset has no effect due to boxing the value type
            ((IEnumerator<int>)enumerator).Reset();
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
        }

        [Fact]
        public void TestIEnumeratorT()
        {
            ImmutableSortedTreeSet<int>.Builder set = ImmutableSortedTreeSet.CreateBuilder<int>();
            IEnumerator<int> enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            CollectionAssert.EnumeratorInvalidated(set, () => set.Add(1));
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            Assert.Equal(0, enumerator.Current);

            enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);

            enumerator.Reset();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
            enumerator.Reset();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
        }

        [Fact]
        public void TestRemoveValue()
        {
            var set = ImmutableSortedTreeSet.CreateRange(comparer: null, Enumerable.Range(0, 10)).ToBuilder();
            Assert.False(set.Remove(-1));
            Assert.Equal(10, set.Count);
            Assert.True(set.Remove(3));
            Assert.Equal(9, set.Count);
        }

        protected override ISet<T> CreateSet<T>()
        {
            return ImmutableSortedTreeSet.CreateBuilder<T>();
        }
    }
}
