// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public partial class ImmutableTreeListBuilderTest
    {
        [Fact]
        public void TestTreeListConstructor()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            Assert.Empty(list);
        }

        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(ImmutableTreeList.Create(600, 601).ToBuilder(), supportsNullValues: false);
            TestIListInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601).ToBuilder(), supportsNullValues: true);
            TestIListInterfaceImpl(ImmutableTreeList.Create<object>(600, 601).ToBuilder(), supportsNullValues: true);

            // Run the same set of tests on List<T> to ensure consistent behavior
            TestIListInterfaceImpl(new List<int> { 600, 601 }, supportsNullValues: false);
            TestIListInterfaceImpl(new List<int?> { 600, 601 }, supportsNullValues: true);
            TestIListInterfaceImpl(new List<object> { 600, 601 }, supportsNullValues: true);
        }

        private static void TestIListInterfaceImpl(IList list, bool supportsNullValues)
        {
            Assert.False(list.IsFixedSize);
            Assert.False(list.IsReadOnly);

            Assert.Equal(600, list[0]);
            Assert.Equal(601, list[1]);

            Assert.True(list.Contains(600));
            Assert.False(list.Contains("Text"));
            Assert.False(list.Contains(null));

            Assert.Equal(0, list.IndexOf(600));
            Assert.Equal(1, list.IndexOf(601));
            Assert.Equal(-1, list.IndexOf("Text"));
            Assert.Equal(-1, list.IndexOf(null));

            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] = 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count] = 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-1, 602));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(list.Count + 1, 602));

            Assert.NotEqual(list[1], list[0]);
            list.Insert(0, list[0]);
            Assert.Equal(list[1], list[0]);

            int originalCount = list.Count;
            list.Remove(null);
            list.Remove("Text");
            Assert.Equal(originalCount, list.Count);

            object removedItem = list[0];
            list.Remove(list[0]);
            Assert.Equal(originalCount - 1, list.Count);
            Assert.True(list.Contains(removedItem));

            if (supportsNullValues)
            {
                Assert.Equal(list.Count, list.Add(null));
                Assert.Null(list[list.Count - 1]);
                Assert.True(list.Contains(null));
                Assert.Equal(list.Count - 1, list.IndexOf(null));

                list[list.Count - 1] = 603;
                Assert.Equal(603, list[list.Count - 1]);
                Assert.False(list.Contains(null));
                Assert.Equal(-1, list.IndexOf(null));

                list[list.Count - 1] = null;
                Assert.Null(list[list.Count - 1]);
            }
            else
            {
                // In the face of two errors, verify consistent behavior
                Assert.Throws<ArgumentNullException>(() => list[list.Count] = null);
                Assert.Throws<ArgumentNullException>(() => list.Insert(-1, null));

                Assert.Throws<ArgumentNullException>(() => list.Add(null));
                Assert.Throws<ArgumentNullException>(() => list[list.Count - 1] = null);
                Assert.Throws<ArgumentException>(() => list.Add(new object()));
                Assert.Throws<ArgumentException>(() => list[list.Count - 1] = new object());
                Assert.Throws<ArgumentException>(() => list.Insert(0, new object()));
                Assert.Equal(list.Count, list.Add(602));
            }
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(ImmutableTreeList.Create(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<object>(600, 601).ToBuilder(), isOwnSyncRoot: true, supportsNullValues: true);

            ICollection collection = ImmutableTreeList<int>.Empty.ToBuilder();
            collection.CopyTo(new int[0], 0);

            // Type checks are only performed if the collection has items
            collection.CopyTo(new string[0], 0);

            collection = ImmutableTreeList.CreateRange(Enumerable.Range(0, 100)).ToBuilder();
            var array = new int[collection.Count];
            collection.CopyTo(array, 0);
            Assert.Equal(array, collection);

            // Run the same set of tests on ImmutableList<T>.Builder to ensure consistent behavior
            TestICollectionInterfaceImpl(ImmutableList.Create(600, 601).ToBuilder(), isOwnSyncRoot: false, supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableList.Create<int?>(600, 601).ToBuilder(), isOwnSyncRoot: false, supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableList.Create<object>(600, 601).ToBuilder(), isOwnSyncRoot: false, supportsNullValues: true);
        }

        private static void TestICollectionInterfaceImpl(ICollection collection, bool isOwnSyncRoot, bool supportsNullValues)
        {
            Assert.False(collection.IsSynchronized);

            if (isOwnSyncRoot)
            {
                Assert.Same(collection, collection.SyncRoot);
            }
            else
            {
                Assert.IsType<object>(collection.SyncRoot);
                Assert.Same(collection.SyncRoot, collection.SyncRoot);
            }

            Assert.Throws<ArgumentNullException>("array", () => collection.CopyTo(null, 0));
            Assert.Throws<ArgumentException>(() => collection.CopyTo(new int[collection.Count, 1], 0));

            void CopyToArrayWithNonZeroLowerBound() => collection.CopyTo(Array.CreateInstance(typeof(int), lengths: new[] { collection.Count }, lowerBounds: new[] { 1 }), 0);
            if (collection.GetType().GetGenericTypeDefinition() == typeof(ImmutableList<>.Builder))
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
            TestICollectionTInterfaceImpl(ImmutableTreeList.Create(600, 601).ToBuilder(), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601).ToBuilder(), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableTreeList.Create<object>(600, 601).ToBuilder(), supportsNullValues: true);

            // Run the same set of tests on ImmutableList<T>.Builder to ensure consistent behavior
            TestICollectionTInterfaceImpl(ImmutableList.Create(600, 601).ToBuilder(), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableList.Create<int?>(600, 601).ToBuilder(), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableList.Create<object>(600, 601).ToBuilder(), supportsNullValues: true);
        }

        private static void TestICollectionTInterfaceImpl<T>(ICollection<T> collection, bool supportsNullValues)
        {
            Assert.False(collection.IsReadOnly);

            Assert.Equal(2, collection.Count);

            if (supportsNullValues)
            {
                var copy = new T[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(default, item));
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, 1));
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
                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, 1));
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
        }

        [Fact]
        public void TestIndexer()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 10)).ToBuilder();
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] = 0);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count] = 0);

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(i, list[i]);
                list[i] = ~list[i];
                Assert.Equal(~i, list[i]);
            }
        }

        [Fact]
        public void TestCopyToValidation()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 10)).ToBuilder();
            Assert.Throws<ArgumentNullException>("array", () => list.CopyTo(null));
            Assert.Throws<ArgumentNullException>("array", () => list.CopyTo(0, null, 0, list.Count));
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

            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            list.Add(Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddStaysPacked()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
                list.Add(i);

            list.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            foreach (var item in expected)
            {
                list.Add(item);
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

            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            CollectionAssert.EnumeratorInvalidated(list, () => list.AddRange(expected));

            Assert.Throws<ArgumentNullException>("collection", () => list.AddRange(null));
            CollectionAssert.EnumeratorNotInvalidated(list, () => list.AddRange(Enumerable.Empty<int>()));

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(600, 8 * 9)).ToBuilder();
            Assert.Equal(8 * 9, list.Count);
            CollectionAssert.EnumeratorInvalidated(list, () => list.Clear());
            Assert.Empty(list);

            // If the list is already clear, existing iterators are not invalidated
            CollectionAssert.EnumeratorNotInvalidated(list, () => list.Clear());
            Assert.Empty(list);
        }

        [Fact]
        public void TestInsert()
        {
            const int Value = 600;

            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            list.Insert(0, Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);

            List<int> reference = new List<int>(list);
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.Insert(-1, item: 0));
            Assert.Throws<ArgumentOutOfRangeException>("index", () => reference.Insert(-1, item: 0));
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.Insert(list.Count + 1, item: 0));
            Assert.Throws<ArgumentOutOfRangeException>("index", () => reference.Insert(list.Count + 1, item: 0));
        }

        [Fact]
        public void TestInsertMany()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            foreach (var item in expected.Reverse())
            {
                list.Insert(0, item);
                list.Validate(ValidationRules.None);
                Assert.Equal(item, list[0]);
            }

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestInsertRange()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();

            Assert.Throws<ArgumentNullException>("collection", () => list.InsertRange(0, null));
            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.InsertRange(-1, Enumerable.Empty<int>()));
            Assert.Throws<ArgumentOutOfRangeException>(null, () => list.InsertRange(1, Enumerable.Empty<int>()));

            // Add an initial range to each list
            list.InsertRange(0, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(0, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            // Adding more items to the end keeps things packed
            list.InsertRange(list.Count, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(reference.Count, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            // Add items to the beginning (no longer packed)
            list.InsertRange(0, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(0, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Add items to the middle
            list.InsertRange(list.Count / 2, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(reference.Count / 2, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Add a small range near the beginning
            list.InsertRange(1, Enumerable.Range(0, 1));
            reference.InsertRange(1, Enumerable.Range(0, 1));
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);
        }

        [Fact]
        public void TestBinarySearchFullList()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list.Add(i * 2);
                reference.Add(i * 2);
            }

            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.BinarySearch(index: -1, list.Count, 0, comparer: null));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => list.BinarySearch(0, count: -1, 0, comparer: null));
            Assert.Throws<ArgumentException>(() => list.BinarySearch(1, list.Count, 0, comparer: null));

            // Test below start value
            Assert.Equal(reference.BinarySearch(reference[0] - 1), list.BinarySearch(reference[0] - 1));

            for (int i = 0; i < reference.Count; i++)
            {
                // Test current value
                Assert.Equal(reference.BinarySearch(reference[i]), list.BinarySearch(reference[i]));

                // Test above current value
                Assert.Equal(reference.BinarySearch(reference[i] + 1), list.BinarySearch(reference[i] + 1));
            }

            // Test case where an exception is thrown by the comparer
            var expected = new NotSupportedException();
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => list.BinarySearch(0, new ComparisonComparer<int>((x, y) => throw expected)));
            Assert.Same(expected, exception.InnerException);
            exception = Assert.Throws<InvalidOperationException>(() => reference.BinarySearch(0, new ComparisonComparer<int>((x, y) => throw expected)));
            Assert.Same(expected, exception.InnerException);

            ImmutableTreeList<int>.Builder empty = ImmutableTreeList.CreateBuilder<int>();
            Assert.Equal(~0, empty.BinarySearch(0));
        }

        [Fact]
        public void TestIndexOf()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], 0, list.Count + 1));

            Assert.Equal(-1, list.IndexOf(-1));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.IndexOf(i), list.IndexOf(i));

                int firstIndex = list.IndexOf(i);
                Assert.Equal(reference.IndexOf(i, firstIndex + 1), list.IndexOf(i, firstIndex + 1));
            }

            ImmutableTreeList<int>.Builder empty = ImmutableTreeList.CreateBuilder<int>();
            Assert.Equal(-1, empty.IndexOf(0));

            // Test with a custom equality comparer
            var stringList = ImmutableTreeList.Create<string>("aa", "aA", "Aa", "AA").ToBuilder();
            Assert.Equal(3, stringList.IndexOf("AA", 0, stringList.Count, StringComparer.Ordinal));
            Assert.Equal(0, stringList.IndexOf("AA", 0, stringList.Count, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void TestLastIndexOf()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            Assert.Equal(-1, list.LastIndexOf(0, -1, 0));
            Assert.Equal(-1, reference.LastIndexOf(0, -1, 0));

            // LastIndexOf does not validate anything for empty collections
            Assert.Equal(-1, list.LastIndexOf(0, 0, 0));
            Assert.Equal(-1, reference.LastIndexOf(0, 0, 0));
            Assert.Equal(-1, list.LastIndexOf(0, -40, -50));
            Assert.Equal(-1, reference.LastIndexOf(0, -40, -50));
            Assert.Equal(-1, list.LastIndexOf(0, 40, 50));
            Assert.Equal(-1, reference.LastIndexOf(0, 40, 50));

            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count - 1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count - 1, list.Count + 1));

            Assert.Equal(-1, list.LastIndexOf(-1));
            Assert.Equal(-1, list.LastIndexOf(-1, list.Count - 1, list.Count / 2));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.LastIndexOf(i), list.LastIndexOf(i));

                int lastIndex = list.LastIndexOf(i);
                if (lastIndex < 1)
                    continue;

                Assert.Equal(reference.LastIndexOf(i, lastIndex - 1), list.LastIndexOf(i, lastIndex - 1));
            }

            // Test with a custom equality comparer
            var stringList = ImmutableTreeList.Create<string>("aa", "aA", "Aa", "AA").ToBuilder();
            Assert.Equal(0, stringList.LastIndexOf("aa", stringList.Count - 1, stringList.Count, StringComparer.Ordinal));
            Assert.Equal(3, stringList.LastIndexOf("aa", stringList.Count - 1, stringList.Count, StringComparer.OrdinalIgnoreCase));
        }

        [Fact]
        public void TestLastIndexOfInvalidOperations()
        {
            ImmutableTreeList<int>.Builder single = ImmutableTreeList.CreateRange(Enumerable.Range(1, 1)).ToBuilder();
            Assert.Throws<ArgumentOutOfRangeException>("startIndex", () => single.LastIndexOf(0, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => single.LastIndexOf(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, 2));
        }

        [Fact]
        public void TestFindIndex()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            Assert.Throws<ArgumentNullException>(() => list.FindIndex(null));
            Assert.Throws<ArgumentNullException>(() => list.FindIndex(0, null));
            Assert.Throws<ArgumentNullException>(() => list.FindIndex(0, 0, null));

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

            ImmutableTreeList<int>.Builder empty = ImmutableTreeList.CreateBuilder<int>();
            Assert.Equal(-1, empty.FindIndex(i => true));
        }

        [Fact]
        public void TestFindLastIndex()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(0, 0, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => reference.FindLastIndex(0, 0, i => true));
            Assert.Equal(-1, list.FindLastIndex(-1, 0, i => true));
            Assert.Equal(-1, reference.FindLastIndex(-1, 0, i => true));

            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(null));
            Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(-1, null));
            Assert.Throws<ArgumentNullException>(() => list.FindLastIndex(-1, 0, null));

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

        [Fact]
        public void TestConvertAll()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

            ImmutableTreeList<string> stringList = list.ConvertAll(value => value.ToString());
            List<string> referenceStringList = reference.ConvertAll(value => value.ToString());

            stringList.Validate(ValidationRules.None);
            Assert.Equal(reference, list);
            Assert.Equal(referenceStringList, stringList);

            ImmutableTreeList<int>.Builder empty = ImmutableTreeList.CreateBuilder<int>();
            Assert.Empty(empty);
            ImmutableTreeList<string> stringEmpty = empty.ConvertAll(value => value.ToString());
            Assert.Empty(stringEmpty);
        }

        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            list.Validate(ValidationRules.None);

            // In the first call to TrimExcess, items will move
            list.TrimExcess();
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            // In the second call, the list is already packed so nothing will move
            list.TrimExcess();
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            ImmutableTreeList<int>.Builder empty = ImmutableTreeList.CreateBuilder<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            ImmutableTreeList<int>.Builder single = ImmutableTreeList.CreateRange(Enumerable.Range(0, 1)).ToBuilder();
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            ImmutableTreeList<int>.Builder many = ImmutableTreeList.CreateRange(Enumerable.Range(0, 100)).ToBuilder();
            for (int i = 0; i < 100; i++)
                many.Insert(many.Count / 2, i);

            many.TrimExcess();
            many.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with frozen nodes
            many = ImmutableTreeList.CreateRange(Enumerable.Range(0, 8)).ToBuilder();
            many.Insert(0, -1);
            ImmutableTreeList<int> immutable = many.ToImmutable();
            ////Assert.Same(immutable, many.ToImmutable());
            many.TrimExcess();
            many.Validate(ValidationRules.RequirePacked);
            Assert.NotSame(immutable, many.ToImmutable());
            Assert.Equal(immutable, many.ToImmutable());
        }

        [Fact]
        public void TestSort()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Sort(-1, 0, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Sort(0, -1, null));
            Assert.Throws<ArgumentException>(() => list.Sort(0, list.Count + 1, null));

            // Start by sorting just the first 3 elements
            list.Sort(0, 3, null);
            reference.Sort(0, 3, null);
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Then sort 8 index pages worth
            ImmutableTreeList<int>.Builder tempList = ImmutableTreeList.CreateRange(list.Concat(list)).ToBuilder();
            List<int> tempReference = new List<int>(reference.Concat(reference));
            tempList.Validate(ValidationRules.RequirePacked);
            tempList.Sort(0, 8 * 8, null);
            tempList.Validate(ValidationRules.RequirePacked);
            tempReference.Sort(0, 8 * 8, null);
            Assert.Equal(tempReference, tempList);

            // Then sort everything
            list.Sort();
            reference.Sort();
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Then sort everything in reverse order
            list.Sort(ReverseComparer<int>.Default);
            reference.Sort(ReverseComparer<int>.Default);
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Test sorting an empty list
            ImmutableTreeList<int>.Builder empty = ImmutableTreeList.CreateBuilder<int>();
            empty.Sort();
            Assert.Empty(empty);
            empty.Validate(ValidationRules.RequirePacked);

            // Test sorting a list with all the same value
            ImmutableTreeList<int>.Builder sameValue = ImmutableTreeList.CreateBuilder<int>();
            for (int i = 0; i < 100; i++)
                sameValue.Add(1);

            sameValue.Sort();
            sameValue.Validate(ValidationRules.RequirePacked);
            Assert.Equal(100, sameValue.Count);
            for (int i = 0; i < sameValue.Count; i++)
                Assert.Equal(1, sameValue[i]);
        }

        [Fact]
        public void TestSortComparison()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

            Assert.Throws<ArgumentNullException>(() => list.Sort((Comparison<int>)null));

            Comparison<int> comparison = (x, y) => x - y;
            list.Sort(comparison);
            reference.Sort(comparison);
            Assert.Equal(reference, list);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(11)]
        public void TestRemoveAt(int seed)
        {
            Random random = new Random(seed);
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.RemoveAt(-1));
            Assert.Throws<ArgumentOutOfRangeException>(null, () => list.RemoveAt(list.Count));

            while (list.Count > 0)
            {
                int index = random.Next(list.Count);
                Assert.Equal(reference[index], list[index]);
                reference.RemoveAt(index);
                list.RemoveAt(index);
                list.Validate(ValidationRules.None);

                Assert.Equal(reference, list);
            }

            Assert.Empty(list);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestReverse()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> ordered = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                ordered.Insert(index, item);
            }

            list.Reverse();
            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(ordered[i], list[list.Count - 1 - i]);
            }

            list.Reverse();
            Assert.Equal(ordered, list);

            Assert.Throws<ArgumentOutOfRangeException>("index", () => list.Reverse(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>("count", () => list.Reverse(0, -1));
            Assert.Throws<ArgumentException>(null, () => list.Reverse(list.Count, 1));

            CollectionAssert.EnumeratorNotInvalidated(list, () => list.Reverse(3, count: 0));
            Assert.Equal(ordered, list);

            CollectionAssert.EnumeratorInvalidated(list, () => list.Reverse(3, count: 1));
            Assert.Equal(ordered, list);
        }

        [Fact]
        public void TestQueueLikeBehavior()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list.Add(item);
                reference.Add(item);
            }

            while (list.Count > 0)
            {
                Assert.Equal(reference[0], list[0]);
                reference.RemoveAt(0);
                list.RemoveAt(0);
                list.Validate(ValidationRules.None);

                Assert.Equal(reference, list);
            }

            Assert.Empty(list);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestStackLikeBehavior()
        {
            Random random = new Random();
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list.Add(item);
                reference.Add(item);
            }

            while (list.Count > 0)
            {
                Assert.Equal(reference[reference.Count - 1], list[list.Count - 1]);
                reference.RemoveAt(reference.Count - 1);
                list.RemoveAt(list.Count - 1);
                list.Validate(ValidationRules.None);

                Assert.Equal(reference, list);
            }

            Assert.Empty(list);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            ImmutableTreeList<int>.Enumerator enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            list.Add(1);
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

        [Fact]
        public void TestPartialEnumeration()
        {
            Random random = new Random(1);
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            // Test a subsection of the list
            var listEnumerator = new ImmutableTreeList<int>.Enumerator(list.ToImmutable(), TreeSpan.FromBounds(1, list.Count - 1), list);
            IEnumerator<int> referenceEnumerator = reference.Skip(1).Take(reference.Count - 2).GetEnumerator();
            TestEnumerators(referenceEnumerator, listEnumerator);

            // Test the first half
            listEnumerator = new ImmutableTreeList<int>.Enumerator(list.ToImmutable(), TreeSpan.FromBounds(0, list.Count / 2), list);
            referenceEnumerator = reference.Take(reference.Count / 2).GetEnumerator();
            TestEnumerators(referenceEnumerator, listEnumerator);

            // Test the last half
            listEnumerator = new ImmutableTreeList<int>.Enumerator(list.ToImmutable(), TreeSpan.FromBounds(list.Count / 2, list.Count), list);
            referenceEnumerator = reference.Skip(reference.Count / 2).GetEnumerator();
            TestEnumerators(referenceEnumerator, listEnumerator);

            void TestEnumerators(IEnumerator<int> expected, ImmutableTreeList<int>.Enumerator actual)
            {
                Assert.Equal(referenceEnumerator.Current, listEnumerator.Current);

                while (true)
                {
                    if (!referenceEnumerator.MoveNext())
                    {
                        Assert.False(listEnumerator.MoveNext());
                        break;
                    }

                    Assert.True(listEnumerator.MoveNext());
                    Assert.Equal(referenceEnumerator.Current, listEnumerator.Current);
                }

                Assert.Equal(referenceEnumerator.Current, listEnumerator.Current);
            }
        }

        [Fact]
        public void TestIEnumeratorT()
        {
            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            IEnumerator<int> enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            list.Add(1);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 8 * 4)).ToBuilder();
            Assert.False(list.Remove(-1));
            Assert.Equal(8 * 4, list.Count);
            Assert.True(list.Remove(3));
            Assert.Equal((8 * 4) - 1, list.Count);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 20)).ToBuilder();
            Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null));

            Assert.Equal(10, list.RemoveAll(i => (i % 2) == 0));
            Assert.Equal(new[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 }, list);
            Assert.Equal(0, list.RemoveAll(i => i < 0));
            Assert.Equal(10, list.Count);
        }

        [Fact]
        public void TestExists()
        {
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 20)).ToBuilder();
            Assert.Throws<ArgumentNullException>(() => list.Exists(null));

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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(1, 20)).ToBuilder();
            Assert.Throws<ArgumentNullException>(() => list.Find(null));

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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 20)).ToBuilder();
            Assert.Throws<ArgumentNullException>(() => list.FindAll(null));

            ImmutableTreeList<int> found = list.FindAll(i => (i % 2) == 0);

            Assert.Equal(20, list.Count);
            Assert.Equal(10, found.Count);
            Assert.Equal(new[] { 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 }, found);

            Assert.Empty(list.FindAll(i => i < 0));
        }

        [Fact]
        public void TestFindLast()
        {
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(1, 20)).ToBuilder();
            var reference = new List<int>(Enumerable.Range(1, 20));
            Assert.Throws<ArgumentNullException>(() => list.FindLast(null));

            Assert.Equal(0, list.FindLast(i => i < 0));
            Assert.Equal(0, reference.FindLast(i => i < 0));

            Assert.Equal(20, list.FindLast(value => (value % 2) == 0));
            Assert.Equal(20, reference.FindLast(value => (value % 2) == 0));

            Assert.Equal(4, list.FindLast(value => value < 5));
            Assert.Equal(4, reference.FindLast(value => value < 5));
        }

        [Fact]
        public void TestToImmutable()
        {
            int value = Generator.GetInt32();

            ImmutableTreeList<int>.Builder list = ImmutableTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            Assert.Same(ImmutableTreeList<int>.Empty, list.ToImmutable());

            list.Add(value);
            Assert.Equal(new[] { value }, list.ToImmutable());
            Assert.Same(list.ToImmutable(), list.ToImmutable());

            list.Add(value);
            Assert.Equal(new[] { value, value }, list.ToImmutable());
            Assert.Same(list.ToImmutable(), list.ToImmutable());
        }

        [Fact]
        public void TestTrueForAll()
        {
            var list = ImmutableTreeList.CreateBuilder<int>();
            Assert.True(list.TrueForAll(i => false));
            Assert.Throws<ArgumentNullException>(() => list.TrueForAll(null));

            list.Add(1);
            Assert.True(list.TrueForAll(i => i > 0));
            Assert.False(list.TrueForAll(i => i <= 0));
        }
    }
}
