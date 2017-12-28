// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class SortedTreeSetTest : AbstractSetTest
    {
        [Fact]
        public void TestTreeSetConstructor()
        {
            SortedTreeSet<int> set = new SortedTreeSet<int>();
            Assert.Empty(set);
        }

        [Fact]
        public void TestCollectionConstructor()
        {
            Assert.Throws<ArgumentNullException>(() => new SortedTreeSet<int>(collection: null));

            var set = new SortedTreeSet<int>(new[] { 1, 1 });
            Assert.Single(set);
            Assert.Equal(1, set.Min);
            Assert.Equal(1, set.Max);
        }

        [Fact]
        public void TestTreeSetBranchingFactorConstructor()
        {
            SortedTreeSet<int> set = new SortedTreeSet<int>(8);
            Assert.Empty(set);

            Assert.Throws<ArgumentOutOfRangeException>(() => new SortedTreeSet<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new SortedTreeSet<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new SortedTreeSet<int>(1));
        }

        [Fact]
        public void TestEmptySetMinMax()
        {
            TestEmptySetMinMaxImpl<int>();
            TestEmptySetMinMaxImpl<int?>();
            TestEmptySetMinMaxImpl<object>();
            TestEmptySetMinMaxImpl<IEqualityComparer>();
        }

        private void TestEmptySetMinMaxImpl<T>()
        {
            var set = new SortedTreeSet<T>();
            Assert.Equal(default, set.Min);
            Assert.Equal(default, set.Max);
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>().Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>().Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>().Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>(Enumerable.Empty<object>()).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>(Enumerable.Empty<int>()).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>(Enumerable.Empty<IComparable>()).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>(comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>(comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>(comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>(Enumerable.Empty<object>(), comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>(Enumerable.Empty<int>(), comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>(Enumerable.Empty<IComparable>(), comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>(4).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>(4).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>(4).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>(4, comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>(4, comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>(4, comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeSet<object>(4, Enumerable.Empty<object>(), comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeSet<int>(4, Enumerable.Empty<int>(), comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeSet<IComparable>(4, Enumerable.Empty<IComparable>(), comparer: null).Comparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = new ComparisonComparer<object>((x, y) => 0);
            var intComparer = new ComparisonComparer<int>((x, y) => 0);
            var comparableComparer = new ComparisonComparer<IComparable>((x, y) => 0);

            Assert.Same(objComparer, new SortedTreeSet<object>(comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeSet<int>(comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeSet<IComparable>(comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new SortedTreeSet<object>(Enumerable.Empty<object>(), comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeSet<int>(Enumerable.Empty<int>(), comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeSet<IComparable>(Enumerable.Empty<IComparable>(), comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new SortedTreeSet<object>(4, comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeSet<int>(4, comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeSet<IComparable>(4, comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new SortedTreeSet<object>(4, Enumerable.Empty<object>(), comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeSet<int>(4, Enumerable.Empty<int>(), comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeSet<IComparable>(4, Enumerable.Empty<IComparable>(), comparer: comparableComparer).Comparer);
        }

        [Fact]
        public void TestICollectionTInterface()
        {
            ICollection<int> set = new SortedTreeSet<int>(Enumerable.Range(0, 10));
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
            TestICollectionInterfaceImpl(new SortedTreeSet<int> { 600, 601 }, supportsNullValues: false);
            TestICollectionInterfaceImpl(new SortedTreeSet<int?> { 600, 601 }, supportsNullValues: true);
            TestICollectionInterfaceImpl(new SortedTreeSet<object> { 600, 601 }, supportsNullValues: true);

            // Run the same set of tests on SortedSet<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(new SortedSet<int> { 600, 601 }, supportsNullValues: false);
            TestICollectionInterfaceImpl(new SortedSet<int?> { 600, 601 }, supportsNullValues: true);
            TestICollectionInterfaceImpl(new SortedSet<object> { 600, 601 }, supportsNullValues: true);
        }

        [Fact]
        public void TestCopyToValidation()
        {
            SortedTreeSet<int> set = new SortedTreeSet<int>(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("dest", () => set.CopyTo(null, 0, set.Count));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => set.CopyTo(new int[set.Count], -1, set.Count));
            Assert.Throws<ArgumentOutOfRangeException>("length", () => set.CopyTo(new int[set.Count], 0, -1));
            Assert.Throws<ArgumentException>(string.Empty, () => set.CopyTo(new int[set.Count], 1, set.Count));
        }

        [Fact]
        public void TestAdd()
        {
            const int Value = 600;

            SortedTreeSet<int> set = new SortedTreeSet<int>();
            Assert.Empty(set);
            set.Add(Value);
            Assert.Single(set);
            Assert.Equal(Value, set.Min);
            int[] expected = { Value };
            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddStaysPacked()
        {
            SortedTreeSet<int> set = new SortedTreeSet<int>(branchingFactor: 4);
            for (int i = 0; i < 2 * 4 * 4; i++)
                set.Add(i);

            set.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            SortedTreeSet<int> set = new SortedTreeSet<int>(branchingFactor: 3);
            foreach (var item in expected)
            {
                set.Add(item);
                Assert.Equal(item, set.Max);
            }

            Assert.Equal(expected.Length, set.Count);

            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            var set = new SortedTreeSet<int>(branchingFactor: 3);

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
            Random random = new Random();
            SortedTreeSet<int> set = new SortedTreeSet<int>(branchingFactor: 4);
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int value = random.Next(set.Count + 1);
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
            var set = new SortedTreeSet<int>(branchingFactor: 4, collection: Enumerable.Range(0, 100), comparer: null);
            var reference = new SortedSet<int>(Enumerable.Range(0, 100));

            int[] listArray = new int[set.Count * 2];
            int[] referenceArray = new int[reference.Count * 2];

            set.CopyTo(listArray);
            reference.CopyTo(referenceArray);
            Assert.Equal(referenceArray, listArray);

            set.CopyTo(listArray, 0);
            Assert.Equal(referenceArray, listArray);

            set.CopyTo(listArray, set.Count / 2);
            reference.CopyTo(referenceArray, reference.Count / 2);
            Assert.Equal(referenceArray, listArray);
        }

        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            SortedTreeSet<int> set = new SortedTreeSet<int>(branchingFactor: 4);
            SortedSet<int> reference = new SortedSet<int>();
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

            SortedTreeSet<int> empty = new SortedTreeSet<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            SortedTreeSet<int> single = new SortedTreeSet<int>(Enumerable.Range(0, 1));
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            SortedTreeSet<int> binary = new SortedTreeSet<int>(branchingFactor: 2);
            for (int i = 99; i >= 0; i--)
                binary.Add(i);

            binary.TrimExcess();
            binary.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            SortedTreeSet<int> ternary = new SortedTreeSet<int>(branchingFactor: 3, collection: Enumerable.Range(0, 100), comparer: null);
            for (int i = 99; i >= 0; i--)
                ternary.Add(i);

            ternary.TrimExcess();
            ternary.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestTryGetValue()
        {
            var set = new SortedTreeSet<string>(StringComparer.OrdinalIgnoreCase);
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
            var set = new SortedTreeSet<int>();
            var enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            set.Add(1);
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
            var set = new SortedTreeSet<int>();
            IEnumerator<int> enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            set.Add(1);
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
            var set = new SortedTreeSet<int>(4, Enumerable.Range(0, 10), comparer: null);
            Assert.False(set.Remove(-1));
            Assert.Equal(10, set.Count);
            Assert.True(set.Remove(3));
            Assert.Equal(9, set.Count);
        }

        [Fact]
        public void TestRemoveWhere()
        {
            var set = new SortedTreeSet<int>(4, Enumerable.Range(0, 10), comparer: null);
            Assert.Throws<ArgumentNullException>(() => set.RemoveWhere(null));

            Assert.Equal(5, set.RemoveWhere(i => (i % 2) == 0));
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, set);
            Assert.Equal(0, set.RemoveWhere(i => i < 0));
            Assert.Equal(5, set.Count);
        }

        [Fact]
        public void TestSetComparer()
        {
            IEqualityComparer<SortedTreeSet<int>> setComparer = SortedTreeSet<int>.CreateSetComparer();
            Assert.True(setComparer.Equals(SortedTreeSet<int>.CreateSetComparer()));
            Assert.False(setComparer.Equals(null));
            Assert.Equal(setComparer.GetHashCode(), SortedTreeSet<int>.CreateSetComparer().GetHashCode());

            var set = new SortedTreeSet<int>();
            var other = new SortedTreeSet<int>();

            // Test behavior with nulls
            Assert.True(setComparer.Equals(null, null));
            Assert.False(setComparer.Equals(set, null));
            Assert.False(setComparer.Equals(null, set));

            Assert.Equal(0, setComparer.GetHashCode(null));

            // Test behavior with empty sets
            Assert.True(setComparer.Equals(set, other));
            Assert.Equal(setComparer.GetHashCode(set), setComparer.GetHashCode(other));

            // Test behavior with non-empty sets
            set.UnionWith(Enumerable.Range(0, 10));
            Assert.False(setComparer.Equals(set, other));
            other.UnionWith(Enumerable.Range(0, 5));
            Assert.False(setComparer.Equals(set, other));
            other.UnionWith(Enumerable.Range(5, 5));
            Assert.True(setComparer.Equals(set, other));
            Assert.Equal(setComparer.GetHashCode(set), setComparer.GetHashCode(other));

            // Test behavior with non-empty sets with different comparers
            set.Clear();
            other = new SortedTreeSet<int>(ReversingComparer<int>.Default);
            set.UnionWith(Enumerable.Range(0, 10));
            Assert.False(setComparer.Equals(set, other));
            other.UnionWith(Enumerable.Range(0, 5));
            Assert.False(setComparer.Equals(set, other));
            other.UnionWith(Enumerable.Range(5, 5));
            Assert.True(setComparer.Equals(set, other));
            Assert.Equal(setComparer.GetHashCode(set), setComparer.GetHashCode(other));

            // The IEqualityComparer<T> only exists for alignment with Comparer<T>.Default for type T
            Assert.Equal(SortedTreeSet<string>.CreateSetComparer(), SortedTreeSet<string>.CreateSetComparer(StringComparer.OrdinalIgnoreCase));
            Assert.Equal(SortedTreeSet<string>.CreateSetComparer(StringComparer.OrdinalIgnoreCase), SortedTreeSet<string>.CreateSetComparer(StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void TestSetComparerSpecialCases()
        {
            Assert.Same(SortedTreeSet<string>.CreateSetComparer(), SortedTreeSet<string>.CreateSetComparer());
            Assert.Same(SortedTreeSet<string>.CreateSetComparer(), SortedTreeSet<string>.CreateSetComparer(null));
            Assert.Same(SortedTreeSet<string>.CreateSetComparer(), SortedTreeSet<string>.CreateSetComparer(EqualityComparer<string>.Default));
        }

        [Fact]
        public void TestReversingComparer()
        {
            Assert.Equal(0, ReversingComparer<int>.Default.Compare(0, 0));
            Assert.Equal(Comparer<int>.Default.Compare(1, 3), -ReversingComparer<int>.Default.Compare(1, 3));
            Assert.Equal(StringComparer.OrdinalIgnoreCase.Compare("a", "B"), -new ReversingComparer<string>(StringComparer.OrdinalIgnoreCase).Compare("a", "B"));
            Assert.Equal(0, new ReversingComparer<string>(StringComparer.OrdinalIgnoreCase).Compare("a", "A"));
        }

        protected override ISet<T> CreateSet<T>()
        {
            return new SortedTreeSet<T>(branchingFactor: 4);
        }

        private sealed class ReversingComparer<T> : IComparer<T>
        {
            public static readonly ReversingComparer<T> Default = new ReversingComparer<T>(null);

            private readonly IComparer<T> _comparer;

            public ReversingComparer(IComparer<T> comparer)
            {
                _comparer = comparer ?? Comparer<T>.Default;
            }

            public int Compare(T x, T y) => -_comparer.Compare(x, y);
        }
    }
}
