// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;

    public partial class ImmutableSortedTreeListTest
    {
        [Fact]
        public void TestEmptyImmutableSortedTreeList()
        {
            var list = ImmutableSortedTreeList.Create<int>();
            Assert.Same(ImmutableSortedTreeList<int>.Empty, list);
            Assert.Empty(list);
            Assert.True(list.IsEmpty);
        }

        [Fact]
        public void TestSingleElementList()
        {
            var value = Generator.GetInt32();
            var list = ImmutableSortedTreeList.Create(value);
            Assert.Equal(new[] { value }, list);
            Assert.False(list.IsEmpty);

            list = ImmutableSortedTreeList.Create(comparer: null, value);
            Assert.Same(Comparer<int>.Default, list.Comparer);
            Assert.Equal(new[] { value }, list);

            list = ImmutableSortedTreeList.Create(ReverseComparer<int>.Default, value);
            Assert.Same(ReverseComparer<int>.Default, list.Comparer);
            Assert.Equal(new[] { value }, list);
        }

        [Fact]
        public void TestMultipleElementList()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };

            // Construction using ImmutableSortedTreeList.Create
            var list = ImmutableSortedTreeList.Create(values);
            Assert.Equal(values.OrderBy(x => x), list);

            list = ImmutableSortedTreeList.Create<int>(comparer: null, values);
            Assert.Same(Comparer<int>.Default, list.Comparer);
            Assert.Equal(values.OrderBy(x => x), list);

            list = ImmutableSortedTreeList.Create(ReverseComparer<int>.Default, values);
            Assert.Same(ReverseComparer<int>.Default, list.Comparer);
            Assert.Equal(values.OrderByDescending(x => x), list);

            // Construction using ImmutableSortedTreeList.ToImmutableSortedTreeList
            list = values.ToImmutableSortedTreeList();
            Assert.Same(Comparer<int>.Default, list.Comparer);
            Assert.Equal(values.OrderBy(x => x), list);

            list = values.ToImmutableSortedTreeList(comparer: null);
            Assert.Same(Comparer<int>.Default, list.Comparer);
            Assert.Equal(values.OrderBy(x => x), list);

            list = values.ToImmutableSortedTreeList(ReverseComparer<int>.Default);
            Assert.Same(ReverseComparer<int>.Default, list.Comparer);
            Assert.Equal(values.OrderByDescending(x => x), list);
        }

        [Fact]
        public void TestImmutableSortedTreeListCreateValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeList.Create(default(int[])!));
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeList.Create(Comparer<int>.Default, default(int[])!));
        }

        [Fact]
        public void TestImmutableSortedTreeListCreateRange()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var list = ImmutableSortedTreeList.CreateRange(values);
            Assert.Equal(values.OrderBy(x => x), list);
        }

        [Fact]
        public void TestImmutableSortedTreeListCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeList.CreateRange<int>(null!));
            Assert.Throws<ArgumentNullException>("items", () => ImmutableSortedTreeList.CreateRange(Comparer<int>.Default, null!));
            Assert.Throws<ArgumentNullException>("items", () => default(IEnumerable<int>)!.ToImmutableSortedTreeList());
            Assert.Throws<ArgumentNullException>("items", () => default(IEnumerable<int>)!.ToImmutableSortedTreeList(Comparer<int>.Default));
        }

        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(ImmutableSortedTreeList.Create<int>(600, 601), supportsNullValues: false);
            TestIListInterfaceImpl(ImmutableSortedTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListInterfaceImpl(ImmutableSortedTreeList.Create<ValueType>(600, 601), supportsNullValues: true);
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
            TestIListTInterfaceImpl(ImmutableSortedTreeList.Create(600, 601), supportsNullValues: false);
            TestIListTInterfaceImpl(ImmutableSortedTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListTInterfaceImpl(ImmutableSortedTreeList.Create<object>(600, 601), supportsNullValues: true);

            // Run the same set of tests on List<T> to ensure consistent behavior
            TestIListTInterfaceImpl(ImmutableList.Create(600, 601), supportsNullValues: false);
            TestIListTInterfaceImpl(ImmutableList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListTInterfaceImpl(ImmutableList.Create<object>(600, 601), supportsNullValues: true);
        }

        private static void TestIListTInterfaceImpl<T>(IList<T> list, bool supportsNullValues)
        {
            Assert.True(list.IsReadOnly);

            Assert.Equal<object?>(600, list[0]);
            Assert.Equal<object?>(601, list[1]);

            Assert.True(list.Contains((T)(object)600));
            Assert.False(list.Contains(default!));

            Assert.Equal(0, list.IndexOf((T)(object)600));
            Assert.Equal(1, list.IndexOf((T)(object)601));
            Assert.Equal(-1, list.IndexOf(default!));

            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<NotSupportedException>(() => list[-1] = default!);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<NotSupportedException>(() => list[list.Count] = default!);
            Assert.Throws<NotSupportedException>(() => list.Insert(-1, default!));
            Assert.Throws<NotSupportedException>(() => list.Insert(list.Count + 1, default!));

            Assert.NotEqual(list[1], list[0]);
            Assert.Throws<NotSupportedException>(() => list.Insert(0, list[0]));
            Assert.NotEqual(list[1], list[0]);

            int originalCount = list.Count;
            Assert.Throws<NotSupportedException>(() => list.Remove(default!));
            Assert.Equal(originalCount, list.Count);
            Assert.Throws<NotSupportedException>(() => list.RemoveAt(0));
            Assert.Equal(originalCount, list.Count);

            T removedItem = list[0];
            Assert.Throws<NotSupportedException>(() => list.Remove(list[0]));
            Assert.Equal(originalCount, list.Count);
            Assert.True(list.Contains(removedItem));

            if (supportsNullValues)
            {
                Assert.Throws<NotSupportedException>(() => list.Add(default!));
                Assert.NotEqual(default!, list[list.Count - 1]);
                Assert.False(list.Contains(default!));
                Assert.Equal(list.Count - 1, list.IndexOf((T)(object)601));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = default!);
                Assert.Equal((T)(object)601, list[list.Count - 1]);
                Assert.False(list.Contains(default!));
                Assert.Equal(-1, list.IndexOf(default!));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = default!);
                Assert.NotEqual(default!, list[list.Count - 1]);
            }
            else
            {
                // In the face of two errors, verify consistent behavior
                Assert.Throws<NotSupportedException>(() => list[list.Count] = default!);
                Assert.Throws<NotSupportedException>(() => list.Insert(-1, default!));

                Assert.Throws<NotSupportedException>(() => list.Add(default!));
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = default!);
                Assert.Throws<NotSupportedException>(() => list.Add(Activator.CreateInstance<T>()));
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = Activator.CreateInstance<T>());
                Assert.Throws<NotSupportedException>(() => list.Insert(0, Activator.CreateInstance<T>()));
                Assert.Throws<NotSupportedException>(() => list.Add((T)(object)602));
            }
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(ImmutableSortedTreeList.Create(600, 601), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableSortedTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableSortedTreeList.Create<object>(600, 601), supportsNullValues: true);

            ICollection collection = ImmutableSortedTreeList<int>.Empty;
            collection.CopyTo(new int[0], 0);

            // Type checks are only performed if the collection has items
            collection.CopyTo(new string[0], 0);

            collection = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 100));
            var array = new int[collection.Count];
            collection.CopyTo(array, 0);
            Assert.Equal(array, collection);

            // Run the same set of tests on ImmutableList<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(ImmutableList.Create(600, 601), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableList.Create<object>(600, 601), supportsNullValues: true);
        }

        private static void TestICollectionInterfaceImpl(ICollection collection, bool supportsNullValues)
        {
            Assert.True(collection.IsSynchronized);

            Assert.NotNull(collection.SyncRoot);
            Assert.Same(collection, collection.SyncRoot);

            Assert.Throws<ArgumentNullException>("array", () => collection.CopyTo(null!, 0));
            Assert.Throws<ArgumentException>(() => collection.CopyTo(new int[collection.Count, 1], 0));

            void CopyToArrayWithNonZeroLowerBound() => collection.CopyTo(Array.CreateInstance(typeof(int), lengths: new[] { collection.Count }, lowerBounds: new[] { 1 }), 0);
            if (collection.GetType().GetGenericTypeDefinition() == typeof(ImmutableList<>))
            {
                Assert.Throws<IndexOutOfRangeException>(CopyToArrayWithNonZeroLowerBound);
            }
            else
            {
                Assert.Throws<ArgumentException>(CopyToArrayWithNonZeroLowerBound);
            }

            if (supportsNullValues)
            {
                var copy = new object[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, Assert.Null);
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, 1));
                Assert.All(copy, Assert.Null);

                collection.CopyTo(copy, 0);
                Assert.Equal(600, copy[0]);
                Assert.Equal(601, copy[1]);

                copy = new object[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Null(copy[0]);
                Assert.Equal(600, copy[1]);
                Assert.Equal(601, copy[2]);
                Assert.Null(copy[3]);

                Assert.Throws<InvalidCastException>(() => collection.CopyTo(new string[collection.Count], 0));
            }
            else
            {
                var copy = new int[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(0, item));
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, 1));
                Assert.All(copy, item => Assert.Equal(0, item));

                collection.CopyTo(copy, 0);
                Assert.Equal(600, copy[0]);
                Assert.Equal(601, copy[1]);

                copy = new int[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Equal(0, copy[0]);
                Assert.Equal(600, copy[1]);
                Assert.Equal(601, copy[2]);
                Assert.Equal(0, copy[3]);

                Assert.Throws<InvalidCastException>(() => collection.CopyTo(new string[collection.Count], 0));
            }
        }

        [Fact]
        public void TestICollectionTInterface()
        {
            TestICollectionTInterfaceImpl(ImmutableSortedTreeList.Create(600, 601), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableSortedTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableSortedTreeList.Create<object>(600, 601), supportsNullValues: true);

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
                Assert.All(copy, item => Assert.Equal(default!, item));
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, 1));
                Assert.All(copy, item => Assert.Equal(default!, item));

                collection.CopyTo(copy, 0);
                Assert.Equal<object?>(600, copy[0]);
                Assert.Equal<object?>(601, copy[1]);

                copy = new T[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Equal(default!, copy[0]);
                Assert.Equal<object?>(600, copy[1]);
                Assert.Equal<object?>(601, copy[2]);
                Assert.Equal(default!, copy[3]);
            }
            else
            {
                var copy = new T[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(default!, item));
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, 1));
                Assert.All(copy, item => Assert.Equal(default!, item));

                collection.CopyTo(copy, 0);
                Assert.Equal<object?>(600, copy[0]);
                Assert.Equal<object?>(601, copy[1]);

                copy = new T[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Equal(default!, copy[0]);
                Assert.Equal<object?>(600, copy[1]);
                Assert.Equal<object?>(601, copy[2]);
                Assert.Equal(default!, copy[3]);
            }

            Assert.Throws<NotSupportedException>(() => collection.Clear());
            Assert.Throws<NotSupportedException>(() => collection.Add(default!));
            Assert.Throws<NotSupportedException>(() => collection.Remove(default!));
        }

        [Fact]
        public void TestIndexer()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<NotSupportedException>(() => ((IImmutableList<int>)list).SetItem(-1, 0));
            Assert.Throws<NotSupportedException>(() => ((IImmutableList<int>)list).SetItem(list.Count, 0));
            Assert.Throws<NotSupportedException>(() => ((IImmutableList<int>)list).SetItem(0, 0));
        }

        [Fact]
        public void TestCopyToValidation()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("array", () => list.CopyTo(null!));
            Assert.Throws<ArgumentNullException>("array", () => list.CopyTo(0, null!, 0, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.CopyTo(-1, new int[list.Count], 0, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => list.CopyTo(0, new int[list.Count], -1, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => list.CopyTo(0, new int[list.Count], 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(null, () => list.CopyTo(1, new int[list.Count], 0, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>(string.Empty, () => list.CopyTo(0, new int[list.Count], 1, list.Count));
        }

        [Fact]
        public void TestAdd()
        {
            const int Value = 600;

            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            Assert.Empty(list);
            list = list.Add(Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);

            Assert.NotSame(list, list.Add(Value, addIfPresent: true));
            list = list.Add(Value, addIfPresent: true);
            Assert.Equal(new[] { Value, Value }, list);

            Assert.Same(list, list.Add(Value, addIfPresent: false));
            list = list.Add(Value, addIfPresent: false);
            Assert.Equal(new[] { Value, Value }, list);
        }

        [Fact]
        public void TestAddViaInterface()
        {
            const int Value = 600;

            IImmutableList<int> list = ImmutableSortedTreeList.Create<int>();
            Assert.Empty(list);
            list = list.Add(Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddStaysPacked()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
                list.Add(i);

            list.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            foreach (var item in expected)
            {
                list = list.Add(item);
                list.Validate(ValidationRules.RequirePacked);
                Assert.Equal(item, list[list.Count - 1]);
            }

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddRange()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            CollectionAssert.EnumeratorNotInvalidated(list, () => list = list.AddRange(expected));

            Assert.Throws<ArgumentNullException>("items", () => list.AddRange(null!));
            CollectionAssert.EnumeratorNotInvalidated(list, () => list.AddRange(Enumerable.Empty<int>()));

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddRangeViaInterface()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            IImmutableList<int> list = ImmutableSortedTreeList.Create<int>();
            list = list.AddRange(expected);

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(600, 8 * 9));
            Assert.Equal(8 * 9, list.Count);
            Assert.Empty(list.Clear());
            Assert.Same(ImmutableSortedTreeList<int>.Empty, list.Clear());

            var stringList = ImmutableSortedTreeList.CreateRange(StringComparer.Ordinal, new[] { "a", "b" });
            ImmutableSortedTreeList<string> emptyStringList = stringList.Clear();
            Assert.Same(stringList.Comparer, emptyStringList.Comparer);
            Assert.Same(emptyStringList, emptyStringList.Clear());
        }

        [Fact]
        public void TestClearViaInterface()
        {
            IImmutableList<int> list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(600, 8 * 9));
            Assert.Equal(8 * 9, list.Count);
            Assert.Empty(list.Clear());
            Assert.Same(ImmutableSortedTreeList<int>.Empty, list.Clear());
        }

        [Fact]
        public void TestInsertViaInterface()
        {
            IImmutableList<int> list = ImmutableSortedTreeList.Create<int>();
            Assert.Throws<NotSupportedException>(() => list.Insert(0, 0));
        }

        [Fact]
        public void TestInsertRange()
        {
            IImmutableList<int> list = ImmutableSortedTreeList.Create<int>();
            Assert.Throws<NotSupportedException>(() => list.InsertRange(0, Enumerable.Range(8, 3)));
        }

        [Fact]
        public void TestBinarySearchFullList()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list = list.Add(i * 2);
                reference.Add(i * 2);
            }

            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.BinarySearch(index: -1, list.Count, 0));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => list.BinarySearch(0, count: -1, 0));
            Assert.Throws<ArgumentException>(() => list.BinarySearch(1, list.Count, 0));

            // Test below start value
            Assert.Equal(reference.BinarySearch(reference[0] - 1), list.BinarySearch(reference[0] - 1));

            for (int i = 0; i < reference.Count; i++)
            {
                // Test current value
                Assert.Equal(reference.BinarySearch(reference[i]), list.BinarySearch(reference[i]));

                // Test above current value
                Assert.Equal(reference.BinarySearch(reference[i] + 1), list.BinarySearch(reference[i] + 1));
            }

            var empty = ImmutableSortedTreeList.Create<int>();
            Assert.Equal(~0, empty.BinarySearch(0));
        }

        [Fact]
        public void TestIndexOf()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list = list.Add(i);
                reference.Add(i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], 0, -1));
            Assert.Throws<ArgumentException>(() => list.IndexOf(list[0], 0, list.Count + 1));

            Assert.Equal(-1, list.IndexOf(-1));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.IndexOf(i), list.IndexOf(i));

                int firstIndex = list.IndexOf(i);
                Assert.Equal(reference.IndexOf(i, firstIndex + 1), list.IndexOf(i, firstIndex + 1));
                Assert.Equal(reference.IndexOf(i, firstIndex + 1), list.IndexOf(i, firstIndex + 1, list.Count - firstIndex - 1));
            }

            ImmutableSortedTreeList<int> empty = ImmutableSortedTreeList.Create<int>();
            Assert.Equal(-1, empty.IndexOf(0));

            // Test with a custom equality comparer
            var stringList = ImmutableSortedTreeList.Create<string>("aa", "aA", "Aa", "AA");
            Assert.Equal(3, stringList.IndexOf("AA", StringComparer.Ordinal));
            Assert.Equal(0, stringList.IndexOf("AA", StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void TestLastIndexOf()
        {
            Random random = new Random();
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            List<int> reference = new List<int>();
            Assert.Equal(-1, list.LastIndexOf(0));
            Assert.Equal(-1, list.LastIndexOf(0, -1, 0));
            Assert.Equal(-1, reference.LastIndexOf(0, -1, 0));

            Assert.Throws<ArgumentException>(() => list.LastIndexOf(0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(0, -40, -50));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(0, 40, 50));

            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list = list.Add(i);
                reference.Add(i);
            }

            Assert.Throws<ArgumentException>(() => list.LastIndexOf(list[0], list.Count, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count - 1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count - 1, list.Count + 1));

            Assert.Equal(-1, list.LastIndexOf(-1, list.Count - 1, list.Count));
            Assert.Equal(-1, list.LastIndexOf(-1, list.Count - 1, list.Count / 2));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.LastIndexOf(i), list.LastIndexOf(i));
                Assert.Equal(reference.LastIndexOf(i), list.LastIndexOf(i, list.Count - 1));
                Assert.Equal(reference.LastIndexOf(i), list.LastIndexOf(i, list.Count - 1, list.Count));

                int lastIndex = list.LastIndexOf(i, list.Count - 1, list.Count);
                if (lastIndex < 1)
                    continue;

                Assert.Equal(reference.LastIndexOf(i, lastIndex - 1), list.LastIndexOf(i, lastIndex - 1, lastIndex));
                Assert.Equal(reference.LastIndexOf(i, lastIndex - 1), list.LastIndexOf(i, lastIndex - 1));
            }

            // Test with a custom equality comparer
            var stringList = ImmutableSortedTreeList.Create<string>("aa", "aA", "Aa", "AA");
            Assert.Equal(0, stringList.LastIndexOf("aa", StringComparer.Ordinal));
            Assert.Equal(3, stringList.LastIndexOf("aa", StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void TestLastIndexOfInvalidOperations()
        {
            ImmutableSortedTreeList<int> single = ImmutableSortedTreeList.CreateRange(Enumerable.Range(1, 1));
            Assert.Throws<ArgumentException>(() => single.LastIndexOf(0, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => single.LastIndexOf(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, 2));
        }

        [Fact]
        public void TestFindIndex()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list = list.Add(i);
                reference.Add(i);
            }

            Assert.Throws<ArgumentNullException>(() => list.FindIndex(null!));
            Assert.Throws<ArgumentNullException>(() => list.FindIndex(0, null!));
            Assert.Throws<ArgumentNullException>(() => list.FindIndex(0, 0, null!));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex(-1, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex(0, -1, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindIndex(0, list.Count + 1, i => true));

            Assert.Equal(-1, list.FindIndex(_ => false));

            for (int i = 0; i < list.Count; i++)
            {
                Predicate<int> predicate = value => value == i;
                Assert.Equal(reference.FindIndex(predicate), list.FindIndex(predicate));

                int firstIndex = list.FindIndex(predicate);
                Assert.Equal(reference.FindIndex(firstIndex + 1, predicate), list.FindIndex(firstIndex + 1, predicate));
            }

            ImmutableSortedTreeList<int> empty = ImmutableSortedTreeList.Create<int>();
            Assert.Equal(-1, empty.FindIndex(i => true));
        }

        [Fact]
        public void TestFindLastIndex()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            List<int> reference = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(0, 0, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => reference.FindLastIndex(0, 0, i => true));
            Assert.Equal(-1, list.FindLastIndex(-1, 0, i => true));
            Assert.Equal(-1, reference.FindLastIndex(-1, 0, i => true));

            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list = list.Add(i);
                reference.Add(i);
            }

            Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(null!));
            Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(-1, null!));
            Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(-1, 0, null!));

            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(list.Count, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(list.Count - 1, -1, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(list.Count - 1, list.Count + 1, i => true));

            Assert.Equal(-1, list.FindLastIndex(_ => false));
            Assert.Equal(-1, list.FindLastIndex(list.Count - 1, list.Count / 2, _ => false));

            for (int i = 0; i < list.Count; i++)
            {
                Predicate<int> predicate = value => value == i;
                Assert.Equal(reference.FindLastIndex(predicate), list.FindLastIndex(predicate));

                int lastIndex = list.FindLastIndex(predicate);
                if (lastIndex < 1)
                    continue;

                Assert.Equal(reference.FindLastIndex(lastIndex - 1, predicate), list.FindLastIndex(lastIndex - 1, predicate));
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void TestRemoveAt(int seed)
        {
            Random random = new Random(seed);
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list = list.Add(item);
                Assert.Equal(reference.Count + 1, list.Count);
                reference.Add(item);
            }

            reference.Sort(list.Comparer);

            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.RemoveAt(-1));
            Assert.Throws<ArgumentOutOfRangeException>(null, () => list.RemoveAt(list.Count));

            while (list.Count > 0)
            {
                int index = random.Next(list.Count);
                Assert.Equal(reference[index], list[index]);
                reference.RemoveAt(index);
                list = list.RemoveAt(index);
                list.Validate(ValidationRules.None);

                Assert.Equal(reference, list);
            }

            Assert.Empty(list);
            Assert.Empty(reference);

            // Test through the IImmutableList<T> interface
            IImmutableList<int> immutableList = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 100));
            Assert.Equal(Enumerable.Range(1, 99), immutableList.RemoveAt(0));
            Assert.IsType<ImmutableSortedTreeList<int>>(immutableList.RemoveAt(0));
        }

        [Fact]
        public void TestRemoveRangeItems()
        {
            Random random = new Random();
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            ImmutableList<int> reference = ImmutableList.Create<int>();
            var itemsToRemove = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list = list.Add(item);
                reference = reference.Add(item);
                if ((random.Next() & 1) != 0)
                    itemsToRemove.Add(item);
            }

            reference = reference.Sort(list.Comparer);

            Assert.True(itemsToRemove.Count > 1);

            ImmutableSortedTreeList<int> pruned = list.RemoveRange(itemsToRemove);
            ImmutableList<int> referencePruned = reference.RemoveRange(itemsToRemove);
            Assert.Equal(referencePruned, pruned);

            IImmutableList<int> immutableList = list;
            IImmutableList<int> prunedViaInterface = immutableList.RemoveRange(itemsToRemove);
            Assert.IsType<ImmutableSortedTreeList<int>>(prunedViaInterface);
            Assert.Equal(referencePruned, prunedViaInterface);

            Assert.Throws<ArgumentNullException>("items", () => list.RemoveRange(items: null!));
            Assert.Throws<ArgumentNullException>("items", () => ((IImmutableList<int>)list).RemoveRange(items: null, equalityComparer: null));
        }

        [Fact]
        public void TestRemoveRange()
        {
            Random random = new Random();
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            ImmutableList<int> reference = ImmutableList.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list = list.Add(item);
                reference = reference.Add(item);
            }

            reference = reference.Sort(list.Comparer);

            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.RemoveRange(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => list.RemoveRange(0, -1));
            Assert.Throws<ArgumentException>(null, () => list.RemoveRange(list.Count, 1));

            Assert.Same(list, list.RemoveRange(5, 0));
            Assert.Same(reference, reference.RemoveRange(5, 0));

            // This is one of the cases where it's currently possible to obtain an empty list which is not the same
            // instance as ImmutableSortedTreeList<T>.Empty.
            Assert.NotSame(ImmutableSortedTreeList<int>.Empty, list.RemoveRange(0, list.Count));
            Assert.Empty(list.RemoveRange(0, list.Count));

            ImmutableSortedTreeList<int> pruned = list.RemoveRange(3, list.Count - 8);
            ImmutableList<int> referencePruned = reference.RemoveRange(3, reference.Count - 8);
            Assert.Equal(referencePruned, pruned);

            IImmutableList<int> immutableList = list;
            IImmutableList<int> prunedViaInterface = immutableList.RemoveRange(3, immutableList.Count - 8);
            Assert.IsType<ImmutableSortedTreeList<int>>(prunedViaInterface);
            Assert.Equal(referencePruned, prunedViaInterface);
        }

        [Fact]
        public void TestReverse()
        {
            Random random = new Random();
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list = list.Add(item);
            }

            IEnumerable<int> reversed = list.Reverse();
            Assert.Equal(list.AsEnumerable().Reverse(), reversed);
            Assert.Equal(list, reversed.Reverse());
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            ImmutableSortedTreeList<int>.Enumerator enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            list = list.Add(1);
            Assert.False(enumerator.MoveNext());
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

        [Fact]
        public void TestIEnumeratorT()
        {
            ImmutableSortedTreeList<int> list = ImmutableSortedTreeList.Create<int>();
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list doesn't affect the enumerator
            list = list.Add(1);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            enumerator.Reset();
            Assert.Equal(0, enumerator.Current);

            enumerator = list.GetEnumerator();
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
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 8 * 4));
            Assert.Same(list, list.Remove(-1));
            Assert.Equal(8 * 4, list.Count);
            Assert.NotSame(list, list.Remove(3));
            Assert.Equal((8 * 4) - 1, list.Remove(3).Count);

            // Test through the IImmutableList<T> interface
            IImmutableList<int> immutableList = list;
            Assert.Same(immutableList, immutableList.Remove(-1));
            Assert.NotSame(immutableList, immutableList.Remove(3));
            Assert.Equal((8 * 4) - 1, immutableList.Remove(3).Count);
        }

        [Fact]
        public void TestReplaceValue()
        {
            var list = ImmutableSortedTreeList.Create("aa", "aA", "Aa", "AA");
            Assert.Equal(new[] { "aa", "aA", "Aa", "AA" }, list);
            Assert.Throws<ArgumentException>("oldValue", () => list.Replace("a", "b"));
            Assert.Throws<ArgumentException>("oldValue", () => ((IImmutableList<string>)list).Replace("a", "b", StringComparer.Ordinal));

            IImmutableList<string> replaceWithOneMatch = ((IImmutableList<string>)list).Replace("AA", "bb", StringComparer.Ordinal);
            Assert.Equal(new[] { "aa", "aA", "Aa", "bb" }, replaceWithOneMatch);

            IImmutableList<string> replaceWithMultipleMatches = ((IImmutableList<string>)list).Replace("AA", "bb", StringComparer.OrdinalIgnoreCase);
            Assert.Equal(new[] { "aA", "Aa", "AA", "bb" }, replaceWithMultipleMatches);

            Assert.Equal(new[] { "aa", "Aa", "AA", "bb" }, list.Replace("aA", "bb"));
        }

        [Fact]
        public void TestRemoveAll()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 20));
            Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null!));

            Assert.Equal(10, list.Count - list.RemoveAll(i => (i % 2) == 0).Count);
            Assert.Equal(Enumerable.Range(0, 20), list);
            Assert.Equal(new[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 }, list.RemoveAll(i => (i % 2) == 0));
            Assert.Same(list, list.RemoveAll(i => i < 0));
            Assert.Equal(20, list.Count);

            // Test through the IImmutableList<T> interface
            IImmutableList<int> immutableList = list;
            Assert.Equal(new[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 }, immutableList.RemoveAll(i => (i % 2) == 0));
            Assert.Same(immutableList, immutableList.RemoveAll(i => i < 0));
        }

        [Fact]
        public void TestExists()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 20));
            Assert.Throws<ArgumentNullException>(() => list.Exists(null!));

            Assert.False(list.Exists(value => value < 0));
            foreach (var i in list)
            {
                Assert.True(list.Exists(value => value == i));
            }

            Assert.False(list.Exists(value => value > 20));
        }

        [Fact]
        public void TestFind()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(1, 20));
            Assert.Throws<ArgumentNullException>(() => list.Find(null!));

            Assert.Equal(0, list.Find(value => value < 0));
            foreach (var i in list)
            {
                Assert.Equal(i, list.Find(value => value == i));
            }

            Assert.Equal(2, list.Find(value => value > 1));
        }

        [Fact]
        public void TestFindAll()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 20));
            Assert.Throws<ArgumentNullException>(() => list.FindAll(null!));

            ImmutableSortedTreeList<int> found = list.FindAll(i => (i % 2) == 0);

            Assert.Equal(20, list.Count);
            Assert.Equal(10, found.Count);
            Assert.Equal(new[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 }, found);

            Assert.Empty(list.FindAll(i => i < 0));

            Assert.Same(ImmutableSortedTreeList<int>.Empty, ImmutableSortedTreeList<int>.Empty.FindAll(i => true));
        }

        [Fact]
        public void TestFindLast()
        {
            var list = ImmutableSortedTreeList.CreateRange(Enumerable.Range(1, 20));
            var reference = new List<int>(Enumerable.Range(1, 20));
            Assert.Throws<ArgumentNullException>(() => list.FindLast(null!));

            Assert.Equal(0, list.FindLast(i => i < 0));
            Assert.Equal(0, reference.FindLast(i => i < 0));

            Assert.Equal(20, list.FindLast(value => (value % 2) == 0));
            Assert.Equal(20, reference.FindLast(value => (value % 2) == 0));

            Assert.Equal(4, list.FindLast(value => value < 5));
            Assert.Equal(4, reference.FindLast(value => value < 5));
        }

        [Fact]
        public void TestTrueForAll()
        {
            var list = ImmutableSortedTreeList.Create<int>();
            Assert.True(list.TrueForAll(i => false));
            Assert.Throws<ArgumentNullException>(() => list.TrueForAll(null!));

            list = list.Add(1);
            Assert.True(list.TrueForAll(i => i > 0));
            Assert.False(list.TrueForAll(i => i <= 0));
        }
    }
}
