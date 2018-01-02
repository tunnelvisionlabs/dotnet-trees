// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using TunnelVisionLabs.Collections.Trees.Immutable;
    using Xunit;

    public class ImmutableTreeListTest
    {
        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(ImmutableTreeList.Create(600, 601), supportsNullValues: false);
            TestIListInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListInterfaceImpl(ImmutableTreeList.Create<object>(600, 601), supportsNullValues: true);

            // Run the same set of tests on List<T> to ensure consistent behavior
            TestIListInterfaceImpl(ImmutableList.Create(600, 601), supportsNullValues: false);
            TestIListInterfaceImpl(ImmutableList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListInterfaceImpl(ImmutableList.Create<object>(600, 601), supportsNullValues: true);
        }

        private static void TestIListInterfaceImpl(IList list, bool supportsNullValues)
        {
            Assert.True(list.IsFixedSize);
            Assert.True(list.IsReadOnly);

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
            Assert.Throws<NotSupportedException>(() => list[-1] = 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<NotSupportedException>(() => list[list.Count] = 1);
            Assert.Throws<NotSupportedException>(() => list.Insert(-1, 602));
            Assert.Throws<NotSupportedException>(() => list.Insert(list.Count + 1, 602));

            Assert.NotEqual(list[1], list[0]);
            Assert.Throws<NotSupportedException>(() => list.Insert(0, list[0]));
            Assert.NotEqual(list[1], list[0]);

            int originalCount = list.Count;
            Assert.Throws<NotSupportedException>(() => list.Remove(null));
            Assert.Throws<NotSupportedException>(() => list.Remove("Text"));
            Assert.Equal(originalCount, list.Count);

            object removedItem = list[0];
            Assert.Throws<NotSupportedException>(() => list.Remove(list[0]));
            Assert.Equal(originalCount, list.Count);
            Assert.True(list.Contains(removedItem));

            if (supportsNullValues)
            {
                Assert.Throws<NotSupportedException>(() => list.Add(null));
                Assert.NotNull(list[list.Count - 1]);
                Assert.False(list.Contains(null));
                Assert.Equal(list.Count - 1, list.IndexOf(601));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = 603);
                Assert.Equal(601, list[list.Count - 1]);
                Assert.False(list.Contains(null));
                Assert.Equal(-1, list.IndexOf(null));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = null);
                Assert.NotNull(list[list.Count - 1]);
            }
            else
            {
                // In the face of two errors, verify consistent behavior
                Assert.Throws<NotSupportedException>(() => list[list.Count] = null);
                Assert.Throws<NotSupportedException>(() => list.Insert(-1, null));

                Assert.Throws<NotSupportedException>(() => list.Add(null));
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = null);
                Assert.Throws<NotSupportedException>(() => list.Add(new object()));
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = new object());
                Assert.Throws<NotSupportedException>(() => list.Insert(0, new object()));
                Assert.Throws<NotSupportedException>(() => list.Add(602));
            }

            Assert.Throws<NotSupportedException>(() => list.RemoveAt(0));
            Assert.Throws<NotSupportedException>(() => list.Clear());
        }

        [Fact]
        public void TestIListTInterface()
        {
            TestIListTInterfaceImpl(ImmutableTreeList.Create(600, 601), supportsNullValues: false);
            TestIListTInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestIListTInterfaceImpl(ImmutableTreeList.Create<object>(600, 601), supportsNullValues: true);

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
            TestICollectionInterfaceImpl(ImmutableTreeList.Create(600, 601), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<object>(600, 601), supportsNullValues: true);

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
            TestICollectionTInterfaceImpl(ImmutableTreeList.Create(600, 601), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableTreeList.Create<object>(600, 601), supportsNullValues: true);

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

            Assert.Throws<NotSupportedException>(() => collection.Clear());
            Assert.Throws<NotSupportedException>(() => collection.Add(default));
            Assert.Throws<NotSupportedException>(() => collection.Remove(default));
        }

        [Fact]
        public void TestIndexer()
        {
            ImmutableTreeList<int> list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.SetItem(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list.SetItem(list.Count, 0));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(i, list[i]);
                list = list.SetItem(i, ~list[i]);
                Assert.Equal(~i, list[i]);
            }
        }

        [Fact]
        public void TestCopyToValidation()
        {
            ImmutableTreeList<int> list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 10));
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

            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
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
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
                list.Add(i);

            list.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
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
        public void TestInsert()
        {
            const int Value = 600;

            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            Assert.True(list.IsEmpty);
            Assert.Empty(list);
            list = list.Insert(0, Value);
            Assert.False(list.IsEmpty);
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

            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            foreach (var item in expected.Reverse())
            {
                list = list.Insert(0, item);
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
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();

            Assert.Same(ImmutableTreeList<int>.Empty, list);
            Assert.Same(list, list.InsertRange(0, Enumerable.Empty<int>()));
            Assert.Same(list, list.InsertRange(list.Count, Enumerable.Empty<int>()));
            Assert.Same(list, list.AddRange(Enumerable.Empty<int>()));

            // Add an initial range to each list
            list = list.InsertRange(0, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(0, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            // Adding an empty range doesn't change the list
            Assert.Equal(list, list.InsertRange(0, Enumerable.Empty<int>()));
            Assert.Equal(list, list.InsertRange(list.Count, Enumerable.Empty<int>()));
            Assert.Equal(list, list.AddRange(Enumerable.Empty<int>()));

            // Adding more items to the end keeps things packed
            list = list.InsertRange(list.Count, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(reference.Count, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            // Add items to the beginning (no longer packed)
            list = list.InsertRange(0, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(0, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Add items to the middle
            list = list.InsertRange(list.Count / 2, Enumerable.Range(0, 8 * 9));
            reference.InsertRange(reference.Count / 2, Enumerable.Range(0, 8 * 9));
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Add a small range near the beginning
            list = list.InsertRange(1, Enumerable.Range(0, 1));
            reference.InsertRange(1, Enumerable.Range(0, 1));
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);
        }

        [Fact(Skip = "https://github.com/tunnelvisionlabs/dotnet-trees/issues/59")]
        public void TestInsertEmptyRange()
        {
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();

            Assert.Same(ImmutableTreeList<int>.Empty, list);
            Assert.Same(list, list.InsertRange(0, Enumerable.Empty<int>()));
            Assert.Same(list, list.InsertRange(list.Count, Enumerable.Empty<int>()));
            Assert.Same(list, list.AddRange(Enumerable.Empty<int>()));

            // Add an initial range to each list
            list = list.InsertRange(0, Enumerable.Range(0, 8 * 9));
            Assert.NotSame(ImmutableTreeList<int>.Empty, list);

            // Adding an empty range doesn't change the list at all
            Assert.Same(list, list.InsertRange(0, Enumerable.Empty<int>()));
            Assert.Same(list, list.InsertRange(list.Count, Enumerable.Empty<int>()));
            Assert.Same(list, list.AddRange(Enumerable.Empty<int>()));
        }

        [Fact]
        public void TestBinarySearchFullList()
        {
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                list = list.Add(i * 2);
                reference.Add(i * 2);
            }

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

            ImmutableTreeList<int> empty = ImmutableTreeList.Create<int>();
            Assert.Equal(~0, empty.BinarySearch(0));
        }

        [Fact]
        public void TestIndexOf()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], -1, 0, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], 0, -1, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], 0, list.Count + 1, null));

            Assert.Equal(-1, list.IndexOf(-1));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.IndexOf(i), list.IndexOf(i));

                int firstIndex = list.IndexOf(i);
                Assert.Equal(reference.IndexOf(i, firstIndex + 1), list.IndexOf(i, firstIndex + 1, list.Count - firstIndex - 1, null));
            }

            ImmutableTreeList<int> empty = ImmutableTreeList.Create<int>();
            Assert.Equal(-1, empty.IndexOf(0));
        }

        [Fact]
        public void TestLastIndexOf()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            Assert.Equal(-1, list.LastIndexOf(0, -1, 0, null));
            Assert.Equal(-1, reference.LastIndexOf(0, -1, 0));

            // LastIndexOf does not validate anything for empty collections
            Assert.Equal(-1, list.LastIndexOf(0, 0, 0, null));
            Assert.Equal(-1, reference.LastIndexOf(0, 0, 0));
            Assert.Equal(-1, list.LastIndexOf(0, -40, -50, null));
            Assert.Equal(-1, reference.LastIndexOf(0, -40, -50));
            Assert.Equal(-1, list.LastIndexOf(0, 40, 50, null));
            Assert.Equal(-1, reference.LastIndexOf(0, 40, 50));

            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list.Insert(index, i);
                reference.Insert(index, i);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count, list.Count, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count - 1, -1, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(list[0], list.Count - 1, list.Count + 1, null));

            Assert.Equal(-1, list.LastIndexOf(-1, list.Count - 1, list.Count, null));
            Assert.Equal(-1, list.LastIndexOf(-1, list.Count - 1, list.Count / 2, null));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.LastIndexOf(i), list.LastIndexOf(i, list.Count - 1, list.Count, null));

                int lastIndex = list.LastIndexOf(i, list.Count - 1, list.Count, null);
                if (lastIndex < 1)
                    continue;

                Assert.Equal(reference.LastIndexOf(i, lastIndex - 1), list.LastIndexOf(i, lastIndex - 1, lastIndex, null));
            }
        }

        [Fact]
        public void TestLastIndexOfInvalidOperations()
        {
            ImmutableTreeList<int> single = ImmutableTreeList.CreateRange(Enumerable.Range(1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 1, 0, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, -1, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, 2, null));
        }

        [Fact]
        public void TestFindIndex()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
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

            ImmutableTreeList<int> empty = ImmutableTreeList.Create<int>();
            Assert.Equal(-1, empty.FindIndex(i => true));
        }

        [Fact]
        public void TestFindLastIndex()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
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
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list = list.Insert(index, item);
                reference.Insert(index, item);
            }

            ImmutableTreeList<string> stringList = list.ConvertAll(value => value.ToString());
            List<string> referenceStringList = reference.ConvertAll(value => value.ToString());

            stringList.Validate(ValidationRules.None);
            Assert.Equal(reference, list);
            Assert.Equal(referenceStringList, stringList);

            ImmutableTreeList<int> empty = ImmutableTreeList.Create<int>();
            Assert.Empty(empty);
            ImmutableTreeList<string> stringEmpty = empty.ConvertAll(value => value.ToString());
            Assert.Empty(stringEmpty);
        }

#if false
        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
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

            ImmutableTreeList<int> empty = ImmutableTreeList.Create<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            ImmutableTreeList<int> single = ImmutableTreeList.CreateRange(Enumerable.Range(0, 1));
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            ImmutableTreeList<int> many = ImmutableTreeList.CreateRange(Enumerable.Range(0, 100));
            for (int i = 0; i < 100; i++)
                many.Insert(many.Count / 2, i);

            many.TrimExcess();
            many.Validate(ValidationRules.RequirePacked);
        }
#endif

        [Fact]
        public void TestSort()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list = list.Insert(index, item);
                reference.Insert(index, item);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Sort(-1, 0, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Sort(0, -1, null));
            Assert.Throws<ArgumentException>(() => list.Sort(0, list.Count + 1, null));

            // Start by sorting just the first 2 elements
            list = list.Sort(0, 2, null);
            reference.Sort(0, 2, null);
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Then sort just the first 3 elements
            list = list.Sort(0, 3, null);
            reference.Sort(0, 3, null);
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Then sort 8 index pages worth
            ImmutableTreeList<int> tempList = ImmutableTreeList.CreateRange(list.Concat(list));
            List<int> tempReference = new List<int>(reference.Concat(reference));
            tempList.Validate(ValidationRules.RequirePacked);
            tempList = tempList.Sort(0, 8 * 8, null);
            tempList.Validate(ValidationRules.RequirePacked);
            tempReference.Sort(0, 8 * 8, null);
            Assert.Equal(tempReference, tempList);

            // Then sort everything
            list = list.Sort();
            reference.Sort();
            list.Validate(ValidationRules.None);
            Assert.Equal(reference, list);

            // Test sorting an empty list
            ImmutableTreeList<int> empty = ImmutableTreeList.Create<int>();
            empty = empty.Sort();
            Assert.Empty(empty);
            empty.Validate(ValidationRules.RequirePacked);

            // Test sorting a list with all the same value
            ImmutableTreeList<int> sameValue = ImmutableTreeList.Create<int>();
            for (int i = 0; i < 100; i++)
                sameValue = sameValue.Add(1);

            sameValue = sameValue.Sort();
            sameValue.Validate(ValidationRules.RequirePacked);
            Assert.Equal(100, sameValue.Count);
            for (int i = 0; i < sameValue.Count; i++)
                Assert.Equal(1, sameValue[i]);
        }

        [Fact]
        public void TestSortComparison()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list = list.Insert(index, item);
                reference.Insert(index, item);
            }

            Assert.Throws<ArgumentNullException>(() => list.Sort((Comparison<int>)null));

            Comparison<int> comparison = (x, y) => x - y;
            list = list.Sort(comparison);
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
        public void TestRemoveAt(int seed)
        {
            Random random = new Random(seed);
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list = list.Insert(index, item);
                Assert.Equal(reference.Count + 1, list.Count);
                reference.Insert(index, item);
            }

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
        }

        [Fact]
        public void TestQueueLikeBehavior()
        {
            Random random = new Random();
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list = list.Add(item);
                reference.Add(item);
            }

            while (list.Count > 0)
            {
                Assert.Equal(reference[0], list[0]);
                reference.RemoveAt(0);
                list = list.RemoveAt(0);
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
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = random.Next();
                list = list.Add(item);
                reference.Add(item);
            }

            while (list.Count > 0)
            {
                Assert.Equal(reference[reference.Count - 1], list[list.Count - 1]);
                reference.RemoveAt(reference.Count - 1);
                list = list.RemoveAt(list.Count - 1);
                list.Validate(ValidationRules.None);

                Assert.Equal(reference, list);
            }

            Assert.Empty(list);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            ImmutableTreeList<int>.Enumerator enumerator = list.GetEnumerator();
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
        public void TestPartialEnumeration()
        {
            Random random = new Random(1);
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int index = random.Next(list.Count + 1);
                list = list.Insert(index, i);
                reference.Insert(index, i);
            }

            // Test a subsection of the list
            var listEnumerator = new ImmutableTreeList<int>.Enumerator(list, TreeSpan.FromBounds(1, list.Count - 1), null);
            IEnumerator<int> referenceEnumerator = reference.Skip(1).Take(reference.Count - 2).GetEnumerator();
            TestEnumerators(referenceEnumerator, listEnumerator);

            // Test the first half
            listEnumerator = new ImmutableTreeList<int>.Enumerator(list, TreeSpan.FromBounds(0, list.Count / 2), null);
            referenceEnumerator = reference.Take(reference.Count / 2).GetEnumerator();
            TestEnumerators(referenceEnumerator, listEnumerator);

            // Test the last half
            listEnumerator = new ImmutableTreeList<int>.Enumerator(list, TreeSpan.FromBounds(list.Count / 2, list.Count), null);
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
            ImmutableTreeList<int> list = ImmutableTreeList.Create<int>();
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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 8 * 4));
            Assert.Same(list, list.Remove(-1));
            Assert.Equal(8 * 4, list.Count);
            Assert.NotSame(list, list.Remove(3));
            Assert.Equal((8 * 4) - 1, list.Remove(3).Count);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 20));
            Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null));

            Assert.Equal(10, list.Count - list.RemoveAll(i => (i % 2) == 0).Count);
            Assert.Equal(Enumerable.Range(0, 20), list);
            Assert.Equal(new[] { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 }, list.RemoveAll(i => (i % 2) == 0));
            Assert.Same(list, list.RemoveAll(i => i < 0));
            Assert.Equal(20, list.Count);
        }

        [Fact]
        public void TestExists()
        {
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 20));
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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(1, 20));
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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(0, 20));
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
            var list = ImmutableTreeList.CreateRange(Enumerable.Range(1, 20));
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
        public void TestTrueForAll()
        {
            var list = ImmutableTreeList.Create<int>();
            Assert.True(list.TrueForAll(i => false));
            Assert.Throws<ArgumentNullException>(() => list.TrueForAll(null));

            list = list.Add(1);
            Assert.True(list.TrueForAll(i => i > 0));
            Assert.False(list.TrueForAll(i => i <= 0));
        }
    }
}
