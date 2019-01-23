// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;

    public class ImmutableSortedTreeSetTest : AbstractImmutableSetTest
    {
        [Fact]
        public void TestEmptyImmutableSortedTreeSet()
        {
            var set = ImmutableSortedTreeSet.Create<int>();
            Assert.Same(ImmutableSortedTreeSet<int>.Empty, set);
            Assert.Empty(set);
        }

        [Fact]
        public void TestSingleElementSet()
        {
            var value = Generator.GetInt32().ToString();
            var set = ImmutableSortedTreeSet.Create(value);
            Assert.Equal(new[] { value }, set);

            set = ImmutableSortedTreeSet.Create(comparer: null, value);
            Assert.Same(Comparer<string>.Default, set.KeyComparer);
            Assert.Equal(new[] { value }, set);

            set = ImmutableSortedTreeSet.Create(StringComparer.OrdinalIgnoreCase, value);
            Assert.Same(StringComparer.OrdinalIgnoreCase, set.KeyComparer);
            Assert.Equal(new[] { value }, set);
        }

        [Fact]
        public void TestMinMax()
        {
            Assert.Equal(0, ImmutableSortedTreeSet<int>.Empty.Min);
            Assert.Equal(0, ImmutableSortedTreeSet<int>.Empty.Max);
            Assert.Null(ImmutableSortedTreeSet<object>.Empty.Min);
            Assert.Null(ImmutableSortedTreeSet<object>.Empty.Max);

            var set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(0, 100).Select(x => Generator.GetInt32()));
            Assert.Equal(set.Min(), set.Min);
            Assert.Equal(set.Max(), set.Max);
        }

        [Fact]
        public void TestMultipleElementSet()
        {
            var values = new[] { Generator.GetInt32().ToString(), Generator.GetInt32().ToString(), Generator.GetInt32().ToString() };

            // Construction using ImmutableSortedTreeSet.Create
            var set = ImmutableSortedTreeSet.Create(values);
            Assert.Equal(values.OrderBy(x => x, Comparer<string>.Default), set);

            set = ImmutableSortedTreeSet.Create<string>(comparer: null, values);
            Assert.Same(Comparer<string>.Default, set.KeyComparer);
            Assert.Equal(values.OrderBy(x => x, Comparer<string>.Default), set);

            set = ImmutableSortedTreeSet.Create(StringComparer.OrdinalIgnoreCase, values);
            Assert.Same(StringComparer.OrdinalIgnoreCase, set.KeyComparer);
            Assert.Equal(values.OrderBy(x => x, StringComparer.OrdinalIgnoreCase), set);

            // Construction using ImmutableSortedTreeSet.ToImmutableSortedTreeSet
            set = values.ToImmutableSortedTreeSet();
            Assert.Same(Comparer<string>.Default, set.KeyComparer);
            Assert.Equal(values.OrderBy(x => x, Comparer<string>.Default), set);

            set = values.ToImmutableSortedTreeSet(comparer: null);
            Assert.Same(Comparer<string>.Default, set.KeyComparer);
            Assert.Equal(values.OrderBy(x => x, Comparer<string>.Default), set);

            set = values.ToImmutableSortedTreeSet(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, set.KeyComparer);
            Assert.Equal(values.OrderBy(x => x, StringComparer.OrdinalIgnoreCase), set);
        }

        [Fact]
        public void TestImmutableSortedTreeSetCreateValidation()
        {
            Assert.Throws<ArgumentNullException>("other", () => ImmutableSortedTreeSet.Create(default(int[])));
            Assert.Throws<ArgumentNullException>("other", () => ImmutableSortedTreeSet.Create(Comparer<int>.Default, default(int[])));
        }

        [Fact]
        public void TestImmutableSortedTreeSetCreateRange()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var set = ImmutableSortedTreeSet.CreateRange(values);
            Assert.Equal(values.OrderBy(x => x), set);
        }

        [Fact]
        public void TestImmutableSortedTreeSetCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("other", () => ImmutableSortedTreeSet.CreateRange<int>(null));
            Assert.Throws<ArgumentNullException>("other", () => ImmutableSortedTreeSet.CreateRange(Comparer<int>.Default, null));
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<int>).ToImmutableSortedTreeSet());
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<int>).ToImmutableSortedTreeSet(Comparer<int>.Default));
        }

        [Fact]
        public void TestCollectionConstructorUsesCorrectComparer()
        {
            var instance1 = new StrongBox<int>(1);
            var instance2 = new StrongBox<int>(2);
            var comparer = new ComparisonComparer<StrongBox<int>>((x, y) => Comparer<int>.Default.Compare(x.Value, y.Value));
            var objectSet = ImmutableSortedTreeSet.Create(comparer, new[] { instance1, instance2, instance1 });
            Assert.Same(comparer, objectSet.KeyComparer);
            Assert.Equal(2, objectSet.Count);
            Assert.Equal(new[] { instance1, instance2 }, objectSet);

            var stringSet = ImmutableSortedTreeSet.Create<string>();
            Assert.Same(Comparer<string>.Default, stringSet.KeyComparer);

            stringSet = ImmutableSortedTreeSet.Create<string>(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringSet.KeyComparer);
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.Create<object>().KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.Create<int>().KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.Create<IComparable>().KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.CreateRange<object>(Enumerable.Empty<object>()).KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.CreateRange<int>(Enumerable.Empty<int>()).KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.CreateRange<IComparable>(Enumerable.Empty<IComparable>()).KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.Create<object>(comparer: null).KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.Create<int>(comparer: null).KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.Create<IComparable>(comparer: null).KeyComparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeSet.CreateRange<object>(comparer: null, Enumerable.Empty<object>()).KeyComparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeSet.CreateRange<int>(comparer: null, Enumerable.Empty<int>()).KeyComparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeSet.CreateRange<IComparable>(comparer: null, Enumerable.Empty<IComparable>()).KeyComparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = new ComparisonComparer<object>((x, y) => 0);
            var intComparer = new ComparisonComparer<int>((x, y) => 0);
            var comparableComparer = new ComparisonComparer<IComparable>((x, y) => 0);

            Assert.Same(objComparer, ImmutableSortedTreeSet.Create<object>(comparer: objComparer).KeyComparer);
            Assert.Same(intComparer, ImmutableSortedTreeSet.Create<int>(comparer: intComparer).KeyComparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeSet.Create<IComparable>(comparer: comparableComparer).KeyComparer);

            Assert.Same(objComparer, ImmutableSortedTreeSet.CreateRange<object>(comparer: objComparer, Enumerable.Empty<object>()).KeyComparer);
            Assert.Same(intComparer, ImmutableSortedTreeSet.CreateRange<int>(comparer: intComparer, Enumerable.Empty<int>()).KeyComparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeSet.CreateRange<IComparable>(comparer: comparableComparer, Enumerable.Empty<IComparable>()).KeyComparer);
        }

        [Fact]
        public void TestUnsupportedISetOperations()
        {
            ISet<int> set = ImmutableSortedTreeSet.Create<int>();
            Assert.Throws<NotSupportedException>(() => set.Add(1));
            Assert.Throws<NotSupportedException>(() => set.UnionWith(Enumerable.Empty<int>()));
            Assert.Throws<NotSupportedException>(() => set.IntersectWith(Enumerable.Empty<int>()));
            Assert.Throws<NotSupportedException>(() => set.ExceptWith(Enumerable.Empty<int>()));
            Assert.Throws<NotSupportedException>(() => set.SymmetricExceptWith(Enumerable.Empty<int>()));
        }

        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(ImmutableSortedTreeSet.Create<int>(600, 601), supportsNullValues: false);
            TestIListInterfaceImpl(ImmutableSortedTreeSet.Create<int?>(600, 601), supportsNullValues: true);
            TestIListInterfaceImpl(ImmutableSortedTreeSet.Create<ValueType>(600, 601), supportsNullValues: true);
        }

        private static void TestIListInterfaceImpl(IList list, bool supportsNullValues)
        {
            Assert.True(list.IsFixedSize);
            Assert.True(list.IsReadOnly);

            Assert.Equal(600, list[0]);
            Assert.Equal(601, list[1]);
            Assert.Throws<NotSupportedException>(() => list[0] = 600);

            Assert.True(list.Contains(600));
            Assert.False(list.Contains("Text"));
            Assert.False(list.Contains(null));

            Assert.Equal(0, list.IndexOf(600));
            Assert.Equal(1, list.IndexOf(601));
            Assert.Equal(-1, list.IndexOf("Text"));
            Assert.Equal(-1, list.IndexOf(null));

            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);

            Assert.Throws<NotSupportedException>(() => list[-1] = 1);
            Assert.Throws<NotSupportedException>(() => list[list.Count] = 1);
            Assert.Throws<NotSupportedException>(() => list.Insert(-1, 602));
            Assert.Throws<NotSupportedException>(() => list.Insert(list.Count + 1, 602));

            Assert.NotEqual(list[1], list[0]);

            Assert.Throws<NotSupportedException>(() => list.Add(1));
            Assert.Throws<NotSupportedException>(() => list.Remove(0));

            if (supportsNullValues)
            {
                Assert.NotNull(list[0]);
                Assert.Throws<NotSupportedException>(() => list.Add(null));
                Assert.NotNull(list[0]);
                Assert.False(list.Contains(null));
                Assert.Equal(-1, list.IndexOf(null));

                Assert.Throws<NotSupportedException>(() => list.RemoveAt(0));
                Assert.False(list.Contains(null));
                Assert.Equal(-1, list.IndexOf(null));
            }
            else
            {
                // In the face of two errors, verify consistent
                Assert.Throws<NotSupportedException>(() => list[list.Count] = null);
                Assert.Throws<NotSupportedException>(() => list.Insert(-1, null));

                Assert.Throws<NotSupportedException>(() => list.Add(null));
                Assert.Throws<NotSupportedException>(() => list.Add(new object()));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = null);
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = new object());
                Assert.Throws<NotSupportedException>(() => list.Insert(0, new object()));
            }

            Assert.NotEmpty(list);
            Assert.Throws<NotSupportedException>(() => list.Clear());
            Assert.NotEmpty(list);
        }

        [Fact]
        public void TestIListTInterface()
        {
            TestIListTInterfaceImpl(ImmutableSortedTreeSet.Create(600, 601), supportsNullValues: false);
            TestIListTInterfaceImpl(ImmutableSortedTreeSet.Create<int?>(600, 601), supportsNullValues: true);
            TestIListTInterfaceImpl(ImmutableSortedTreeSet.Create<object>(600, 601), supportsNullValues: true);

            // Run the same set of tests on List<T> to ensure consistent behavior
            TestIListTInterfaceImpl(ImmutableList.Create(600, 601), supportsNullValues: false);
            TestIListTInterfaceImpl(ImmutableList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListTInterfaceImpl(ImmutableList.Create<object>(600, 601), supportsNullValues: true);
        }

        private static void TestIListTInterfaceImpl<T>(IList<T> list, bool supportsNullValues)
        {
            Assert.True(list.IsReadOnly);

            Assert.Equal<object>(600, list[0]);
            Assert.Equal<object>(601, list[1]);

            Assert.True(list.Contains((T)(object)600));
            Assert.False(list.Contains(default));

            Assert.Equal(0, list.IndexOf((T)(object)600));
            Assert.Equal(1, list.IndexOf((T)(object)601));
            Assert.Equal(-1, list.IndexOf(default));

            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<NotSupportedException>(() => list[-1] = default);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<NotSupportedException>(() => list[list.Count] = default);
            Assert.Throws<NotSupportedException>(() => list.Insert(-1, default));
            Assert.Throws<NotSupportedException>(() => list.Insert(list.Count + 1, default));

            Assert.NotEqual(list[1], list[0]);
            Assert.Throws<NotSupportedException>(() => list.Insert(0, list[0]));
            Assert.NotEqual(list[1], list[0]);

            int originalCount = list.Count;
            Assert.Throws<NotSupportedException>(() => list.Remove(default));
            Assert.Equal(originalCount, list.Count);
            Assert.Throws<NotSupportedException>(() => list.RemoveAt(0));
            Assert.Equal(originalCount, list.Count);

            T removedItem = list[0];
            Assert.Throws<NotSupportedException>(() => list.Remove(list[0]));
            Assert.Equal(originalCount, list.Count);
            Assert.True(list.Contains(removedItem));

            if (supportsNullValues)
            {
                Assert.Throws<NotSupportedException>(() => list.Add(default));
                Assert.NotEqual(default, list[list.Count - 1]);
                Assert.False(list.Contains(default));
                Assert.Equal(list.Count - 1, list.IndexOf((T)(object)601));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = default);
                Assert.Equal((T)(object)601, list[list.Count - 1]);
                Assert.False(list.Contains(default));
                Assert.Equal(-1, list.IndexOf(default));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = default);
                Assert.NotEqual(default, list[list.Count - 1]);
            }
            else
            {
                // In the face of two errors, verify consistent behavior
                Assert.Throws<NotSupportedException>(() => list[list.Count] = default);
                Assert.Throws<NotSupportedException>(() => list.Insert(-1, default));

                Assert.Throws<NotSupportedException>(() => list.Add(default));
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = default);
                Assert.Throws<NotSupportedException>(() => list.Add(Activator.CreateInstance<T>()));
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = Activator.CreateInstance<T>());
                Assert.Throws<NotSupportedException>(() => list.Insert(0, Activator.CreateInstance<T>()));
                Assert.Throws<NotSupportedException>(() => list.Add((T)(object)602));
            }
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(ImmutableSortedTreeSet.Create(600, 601), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableSortedTreeSet.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableSortedTreeSet.Create<object>(600, 601), supportsNullValues: true);

            ICollection collection = ImmutableSortedTreeSet<int>.Empty;
            collection.CopyTo(new int[0], 0);

            // Type checks are only performed if the collection has items
            collection.CopyTo(new string[0], 0);

            collection = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(0, 100));
            var array = new int[collection.Count];
            collection.CopyTo(array, 0);
            Assert.Equal(array, collection);

            // Run the same set of tests on ImmutableList<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(ImmutableList.Create(600, 601), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableList.Create<object>(600, 601), supportsNullValues: true);
        }

        [Fact]
        public void TestICollectionTInterface()
        {
            TestICollectionTInterfaceImpl(ImmutableSortedTreeSet.Create(600, 601), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableSortedTreeSet.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableSortedTreeSet.Create<object>(600, 601), supportsNullValues: true);

            // Run the same set of tests on ImmutableList<T> to ensure consistent behavior
            TestICollectionTInterfaceImpl(ImmutableList.Create(600, 601), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableList.Create<object>(600, 601), supportsNullValues: true);
        }

        private static void TestICollectionTInterfaceImpl<T>(ICollection<T> collection, bool supportsNullValues)
        {
            Assert.True(collection.IsReadOnly);

            Assert.Equal(2, collection.Count);

            if (supportsNullValues)
            {
                var copy = new T[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(default, item));
                Assert.All(copy, item => Assert.Equal(default, item));

                collection.CopyTo(copy, 0);
                Assert.Equal<object>(600, copy[0]);
                Assert.Equal<object>(601, copy[1]);

                copy = new T[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Equal(default, copy[0]);
                Assert.Equal<object>(600, copy[1]);
                Assert.Equal<object>(601, copy[2]);
                Assert.Equal(default, copy[3]);
            }
            else
            {
                var copy = new T[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(default, item));
                Assert.All(copy, item => Assert.Equal(default, item));

                collection.CopyTo(copy, 0);
                Assert.Equal<object>(600, copy[0]);
                Assert.Equal<object>(601, copy[1]);

                copy = new T[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Equal(default, copy[0]);
                Assert.Equal<object>(600, copy[1]);
                Assert.Equal<object>(601, copy[2]);
                Assert.Equal(default, copy[3]);
            }

            Assert.Throws<NotSupportedException>(() => collection.Clear());
            Assert.Throws<NotSupportedException>(() => collection.Add(default));
            Assert.Throws<NotSupportedException>(() => collection.Remove(default));
        }

        [Fact]
        public void TestCopyToValidation()
        {
            var set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("array", () => ((ICollection<int>)set).CopyTo(array: null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => ((ICollection<int>)set).CopyTo(new int[set.Count], -1));
            Assert.Throws<ArgumentException>(string.Empty, () => ((ICollection<int>)set).CopyTo(new int[set.Count - 1], 0));
            Assert.Throws<ArgumentException>(() => ((ICollection<int>)set).CopyTo(new int[set.Count], 1));
        }

        [Fact]
        public void TestAdd()
        {
            const int Value = 600;

            var set = ImmutableSortedTreeSet.Create<int>();
            Assert.Empty(set);
            set = set.Add(Value);
            Assert.Single(set);
            int[] expected = { Value };
            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);

            Assert.Same(set, set.Add(Value));
            Assert.Equal(new[] { Value }, set);
        }

        [Fact]
        public void TestAddViaInterface()
        {
            const int Value = 600;

            IImmutableSet<int> set = ImmutableSortedTreeSet.Create<int>();
            Assert.Empty(set);
            set = set.Add(Value);
            Assert.Single(set);
            int[] expected = { Value };
            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddStaysPacked()
        {
            var set = ImmutableSortedTreeSet.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
                set.Add(i);

            set.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            var set = ImmutableSortedTreeSet.Create<int>();
            foreach (var item in expected)
            {
                set = set.Add(item);
                set.Validate(ValidationRules.RequirePacked);
                Assert.Equal(item, set.Last());
            }

            Assert.Equal(expected.Length, set.Count);

            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestUnion2()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            var set = ImmutableSortedTreeSet.Create<int>();
            CollectionAssert.EnumeratorNotInvalidated(set, () => set = set.Union(expected));

            Assert.Throws<ArgumentNullException>("other", () => set.Union(null));
            CollectionAssert.EnumeratorNotInvalidated(set, () => set.Union(Enumerable.Empty<int>()));

            Assert.Equal(expected.Length, set.Count);

            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestUnionViaInterface()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            IImmutableSet<int> set = ImmutableSortedTreeSet.Create<int>();
            set = set.Union(expected);

            Assert.Equal(expected.Length, set.Count);

            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            var set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(600, 8 * 9));
            Assert.Equal(8 * 9, set.Count);
            Assert.Empty(set.Clear());
            Assert.Same(ImmutableSortedTreeSet<int>.Empty, set.Clear());

            var stringSet = ImmutableSortedTreeSet.CreateRange(StringComparer.Ordinal, new[] { "a", "b" });
            ImmutableSortedTreeSet<string> emptyStringSet = stringSet.Clear();
            Assert.Same(stringSet.KeyComparer, emptyStringSet.KeyComparer);
            Assert.Same(emptyStringSet, emptyStringSet.Clear());
        }

        [Fact]
        public void TestClearViaInterface()
        {
            IImmutableSet<int> set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(600, 8 * 9));
            Assert.Equal(8 * 9, set.Count);
            Assert.Empty(set.Clear());
            Assert.Same(ImmutableSortedTreeSet<int>.Empty, set.Clear());
        }

        [Fact]
        public void TestExcept2()
        {
            var random = new Random();
            var set = ImmutableSortedTreeSet.Create<int>();
            var reference = ImmutableList.Create<int>();
            var itemsToRemove = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                set = set.Add(item);
                reference = reference.Add(item);
                if ((random.Next() & 1) != 0)
                    itemsToRemove.Add(item);
            }

            reference = reference.Sort((x, y) => x.GetHashCode().CompareTo(y.GetHashCode()));
            Assert.Equal(reference, set);

            Assert.True(itemsToRemove.Count > 1);

            ImmutableSortedTreeSet<int> pruned = set.Except(itemsToRemove);
            ImmutableList<int> referencePruned = reference.RemoveRange(itemsToRemove);
            Assert.Equal(referencePruned, pruned);

            IImmutableSet<int> immutableSet = set;
            IImmutableSet<int> prunedViaInterface = immutableSet.Except(itemsToRemove);
            Assert.IsType<ImmutableSortedTreeSet<int>>(prunedViaInterface);
            Assert.Equal(referencePruned, prunedViaInterface);

            Assert.Throws<ArgumentNullException>("other", () => set.Except(other: null));
        }

        [Fact]
        public void TestReverse()
        {
            var random = new Random();
            var set = ImmutableSortedTreeSet.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                set = set.Add(item);
            }

            IEnumerable<int> reversed = set.Reverse();
            Assert.Equal(set.AsEnumerable().Reverse(), reversed);
            Assert.Equal(set, reversed.Reverse());
        }

        [Fact]
        public void TestEnumerator()
        {
            var set = ImmutableSortedTreeSet.Create<int>();
            ImmutableSortedTreeSet<int>.Enumerator enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            set = set.Add(1);
            Assert.False(enumerator.MoveNext());
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
            var set = ImmutableSortedTreeSet.Create<int>();
            IEnumerator<int> enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the set doesn't affect the enumerator
            set = set.Add(1);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            enumerator.Reset();
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
            var set = ImmutableSortedTreeSet.CreateRange(Enumerable.Range(0, 8 * 4));
            Assert.Same(set, set.Remove(-1));
            Assert.Equal(8 * 4, set.Count);
            Assert.NotSame(set, set.Remove(3));
            Assert.Equal((8 * 4) - 1, set.Remove(3).Count);

            // Test through the IImmutableSet<T> interface
            IImmutableSet<int> immutableSet = set;
            Assert.Same(immutableSet, immutableSet.Remove(-1));
            Assert.NotSame(immutableSet, immutableSet.Remove(3));
            Assert.Equal((8 * 4) - 1, immutableSet.Remove(3).Count);
        }

        [Fact]
        public void TestTryGetValue()
        {
            var set = ImmutableSortedTreeSet.Create(StringComparer.OrdinalIgnoreCase, "AA");
            Assert.True(set.TryGetValue("AA", out var value));
            Assert.Equal("AA", value);

            Assert.True(set.TryGetValue("aa", out value));
            Assert.Equal("AA", value);
        }

        [Fact]
        public void TestWithComparer()
        {
            // Empty collection with the default comparer
            Assert.Same(ImmutableSortedTreeSet<int>.Empty, ImmutableSortedTreeSet<int>.Empty.WithComparer(null));
            Assert.Same(ImmutableSortedTreeSet<int>.Empty, ImmutableSortedTreeSet<int>.Empty.WithComparer(Comparer<int>.Default));

            // Empty collection with non-default comparer
            ImmutableSortedTreeSet<string> emptyWithCustomComparer = ImmutableSortedTreeSet<string>.Empty.WithComparer(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, emptyWithCustomComparer.KeyComparer);
            Assert.Same(emptyWithCustomComparer, emptyWithCustomComparer.WithComparer(StringComparer.OrdinalIgnoreCase));

            Assert.NotSame(ImmutableSortedTreeSet<string>.Empty, emptyWithCustomComparer);
            Assert.Same(ImmutableSortedTreeSet<string>.Empty, emptyWithCustomComparer.WithComparer(null));

            // Non-empty collections
            var set = ImmutableSortedTreeSet.Create("aa", "AA");
            Assert.Equal(new[] { "aa", "AA" }.OrderBy(x => x.GetHashCode()), set);

            var first = set.First();
            Assert.Equal(new[] { first }, set.WithComparer(StringComparer.OrdinalIgnoreCase));
        }

        protected override IImmutableSet<T> CreateSet<T>()
        {
            return ImmutableSortedTreeSet.Create<T>();
        }
    }
}
