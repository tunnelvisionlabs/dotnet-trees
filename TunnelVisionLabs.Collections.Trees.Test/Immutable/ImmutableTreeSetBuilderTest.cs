// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public partial class ImmutableTreeSetBuilderTest : AbstractSetTest
    {
        [Fact]
        public void TestImmutableTreeSetBuilderConstructor()
        {
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
            Assert.Empty(set);
        }

        [Fact]
        public void TestCollectionConstructorUsesCorrectComparer()
        {
            object instance1 = new object();
            object instance2 = new object();
            var objectSet = ImmutableTreeSet.Create<object>(ZeroHashCodeEqualityComparer<object>.Default, new[] { instance1, instance2, instance1 }).ToBuilder();
            Assert.Same(ZeroHashCodeEqualityComparer<object>.Default, objectSet.KeyComparer);
            Assert.Equal(2, objectSet.Count);
            Assert.Equal(new[] { instance1, instance2 }, objectSet);

            ImmutableTreeSet<string>.Builder stringSet = ImmutableTreeSet.CreateBuilder<string>();
            Assert.Same(EqualityComparer<string>.Default, stringSet.KeyComparer);

            stringSet = ImmutableTreeSet.CreateBuilder(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringSet.KeyComparer);
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.CreateBuilder<object>().KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.CreateBuilder<int>().KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.CreateBuilder<IComparable>().KeyComparer);

            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.CreateRange<object>(Enumerable.Empty<object>()).ToBuilder().KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.CreateRange<int>(Enumerable.Empty<int>()).ToBuilder().KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.CreateRange<IComparable>(Enumerable.Empty<IComparable>()).ToBuilder().KeyComparer);

            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.CreateBuilder<object>(equalityComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.CreateBuilder<int>(equalityComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.CreateBuilder<IComparable>(equalityComparer: null).KeyComparer);

            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.CreateRange<object>(equalityComparer: null, Enumerable.Empty<object>()).ToBuilder().KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.CreateRange<int>(equalityComparer: null, Enumerable.Empty<int>()).ToBuilder().KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.CreateRange<IComparable>(equalityComparer: null, Enumerable.Empty<IComparable>()).ToBuilder().KeyComparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            ZeroHashCodeEqualityComparer<object> objComparer = ZeroHashCodeEqualityComparer<object>.Default;
            ZeroHashCodeEqualityComparer<int> intComparer = ZeroHashCodeEqualityComparer<int>.Default;
            ZeroHashCodeEqualityComparer<IComparable> comparableComparer = ZeroHashCodeEqualityComparer<IComparable>.Default;

            Assert.Same(objComparer, ImmutableTreeSet.CreateBuilder<object>(equalityComparer: objComparer).KeyComparer);
            Assert.Same(intComparer, ImmutableTreeSet.CreateBuilder<int>(equalityComparer: intComparer).KeyComparer);
            Assert.Same(comparableComparer, ImmutableTreeSet.CreateBuilder<IComparable>(equalityComparer: comparableComparer).KeyComparer);

            Assert.Same(objComparer, ImmutableTreeSet.CreateRange<object>(equalityComparer: objComparer, Enumerable.Empty<object>()).ToBuilder().KeyComparer);
            Assert.Same(intComparer, ImmutableTreeSet.CreateRange<int>(equalityComparer: intComparer, Enumerable.Empty<int>()).ToBuilder().KeyComparer);
            Assert.Same(comparableComparer, ImmutableTreeSet.CreateRange<IComparable>(equalityComparer: comparableComparer, Enumerable.Empty<IComparable>()).ToBuilder().KeyComparer);
        }

        [Fact]
        public void TestICollectionTInterface()
        {
            ICollection<int> set = ImmutableTreeSet.CreateRange(Enumerable.Range(0, 10)).ToBuilder();
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
            TestICollectionInterfaceImpl(ImmutableTreeSet.Create<int>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableTreeSet.Create<int?>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableTreeSet.Create<object>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: true);

            // Run the same set of tests on SortedSet<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(new SortedSet<int> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: false);
            TestICollectionInterfaceImpl(new SortedSet<int?> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: true);
            TestICollectionInterfaceImpl(new SortedSet<object> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: true);
        }

        [Fact]
        public void TestCopyToValidation()
        {
            var set = ImmutableTreeSet.CreateRange<int>(Enumerable.Range(0, 10)).ToBuilder();
            Assert.Throws<ArgumentNullException>("array", () => ((ICollection<int>)set).CopyTo(array: null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => ((ICollection<int>)set).CopyTo(new int[set.Count], -1));
            Assert.Throws<ArgumentException>(string.Empty, () => ((ICollection<int>)set).CopyTo(new int[set.Count - 1], 0));
            Assert.Throws<ArgumentException>(() => ((ICollection<int>)set).CopyTo(new int[set.Count], 1));
        }

        [Fact]
        public void TestAdd()
        {
            int value = Generator.GetInt32();

            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
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
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
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

            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
            foreach (var item in expected)
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
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();

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
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
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
            var set = ImmutableTreeSet.CreateRange(equalityComparer: null, Enumerable.Range(0, 100)).ToBuilder();
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
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
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

            ImmutableTreeSet<int>.Builder empty = ImmutableTreeSet.CreateBuilder<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            var single = ImmutableTreeSet.CreateRange<int>(Enumerable.Range(0, 1)).ToBuilder();
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            ImmutableTreeSet<int>.Builder binary = ImmutableTreeSet.CreateBuilder<int>();
            for (int i = 99; i >= 0; i--)
                binary.Add(i);

            binary.TrimExcess();
            binary.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            var ternary = ImmutableTreeSet.CreateRange<int>(equalityComparer: null, Enumerable.Range(0, 100)).ToBuilder();
            for (int i = 99; i >= 0; i--)
                ternary.Add(i);

            ternary.TrimExcess();
            ternary.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestTryGetValue()
        {
            ImmutableTreeSet<string>.Builder set = ImmutableTreeSet.CreateBuilder(StringComparer.OrdinalIgnoreCase);
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
        public void TestEnumerator()
        {
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
            ImmutableTreeSet<int>.Enumerator enumerator = set.GetEnumerator();
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
            ImmutableTreeSet<int>.Builder set = ImmutableTreeSet.CreateBuilder<int>();
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
            var set = ImmutableTreeSet.CreateRange(equalityComparer: null, Enumerable.Range(0, 10)).ToBuilder();
            Assert.False(set.Remove(-1));
            Assert.Equal(10, set.Count);
            Assert.True(set.Remove(3));
            Assert.Equal(9, set.Count);
        }

        protected override ISet<T> CreateSet<T>()
        {
            return ImmutableTreeSet.CreateBuilder<T>();
        }
    }
}
