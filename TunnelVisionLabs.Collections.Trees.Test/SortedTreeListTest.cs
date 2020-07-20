// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TunnelVisionLabs.Collections.Trees;
    using Xunit;
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;

    public class SortedTreeListTest
    {
        [Fact]
        public void TestTreeListConstructor()
        {
            SortedTreeList<int> list = new SortedTreeList<int>();
            Assert.Empty(list);

            Assert.Throws<ArgumentNullException>(() => new SortedTreeList<int>(collection: null!));
        }

        [Fact]
        public void TestTreeListBranchingFactorConstructor()
        {
            SortedTreeList<int> list = new SortedTreeList<int>(8);
            Assert.Empty(list);

            Assert.Throws<ArgumentOutOfRangeException>(() => new SortedTreeList<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new SortedTreeList<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new SortedTreeList<int>(1));
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>().Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>().Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>().Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>(Enumerable.Empty<object>()).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>(Enumerable.Empty<int>()).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>(Enumerable.Empty<IComparable>()).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>(comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>(comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>(comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>(Enumerable.Empty<object>(), comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>(Enumerable.Empty<int>(), comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>(Enumerable.Empty<IComparable>(), comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>(4).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>(4).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>(4).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>(4, comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>(4, comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>(4, comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, new SortedTreeList<object>(4, Enumerable.Empty<object>(), comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, new SortedTreeList<int>(4, Enumerable.Empty<int>(), comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, new SortedTreeList<IComparable>(4, Enumerable.Empty<IComparable>(), comparer: null).Comparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = new ComparisonComparer<object>((x, y) => 0);
            var intComparer = new ComparisonComparer<int>((x, y) => 0);
            var comparableComparer = new ComparisonComparer<IComparable>((x, y) => 0);

            Assert.Same(objComparer, new SortedTreeList<object>(comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeList<int>(comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeList<IComparable>(comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new SortedTreeList<object>(Enumerable.Empty<object>(), comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeList<int>(Enumerable.Empty<int>(), comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeList<IComparable>(Enumerable.Empty<IComparable>(), comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new SortedTreeList<object>(4, comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeList<int>(4, comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeList<IComparable>(4, comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, new SortedTreeList<object>(4, Enumerable.Empty<object>(), comparer: objComparer).Comparer);
            Assert.Same(intComparer, new SortedTreeList<int>(4, Enumerable.Empty<int>(), comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, new SortedTreeList<IComparable>(4, Enumerable.Empty<IComparable>(), comparer: comparableComparer).Comparer);
        }

        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(new SortedTreeList<int> { 600, 601 }, supportsNullValues: false);
            TestIListInterfaceImpl(new SortedTreeList<int?> { 600, 601 }, supportsNullValues: true);
            TestIListInterfaceImpl(new SortedTreeList<ValueType> { 600, 601 }, supportsNullValues: true);
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
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);

            Assert.Throws<NotSupportedException>(() => list[-1] = 1);
            Assert.Throws<NotSupportedException>(() => list[list.Count] = 1);
            Assert.Throws<NotSupportedException>(() => list.Insert(-1, 602));
            Assert.Throws<NotSupportedException>(() => list.Insert(list.Count + 1, 602));

            Assert.NotEqual(list[1], list[0]);

            list.Add(list[0]);

            Assert.Equal(list[1], list[0]);

            int originalCount = list.Count;
            list.Remove(null);
            list.Remove("Text");
            Assert.Equal(originalCount, list.Count);

            object? removedItem = list[0];
            list.Remove(list[0]);
            Assert.Equal(originalCount - 1, list.Count);
            Assert.True(list.Contains(removedItem));

            if (supportsNullValues)
            {
                Assert.NotNull(list[0]);
                Assert.Equal(list.Count, list.Add(null));
                Assert.Null(list[0]);
                Assert.True(list.Contains(null));
                Assert.Equal(0, list.IndexOf(null));

                list.RemoveAt(0);
                Assert.False(list.Contains(null));
                Assert.Equal(-1, list.IndexOf(null));
            }
            else
            {
                // In the face of two errors, verify consistent
                Assert.Throws<NotSupportedException>(() => list[list.Count] = null);
                Assert.Throws<NotSupportedException>(() => list.Insert(-1, null));

                Assert.Throws<ArgumentNullException>(() => list.Add(null));
                Assert.Throws<ArgumentException>(() => list.Add(new object()));

                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = null);
                Assert.Throws<NotSupportedException>(() => list[list.Count - 1] = new object());
                Assert.Throws<NotSupportedException>(() => list.Insert(0, new object()));

                Assert.Equal(list.Count, list.Add(602));
            }

            Assert.NotEmpty(list);
            list.Clear();
            Assert.Empty(list);
        }

        [Fact]
        public void TestRemoveRange()
        {
            var list = new SortedTreeList<int>(branchingFactor: 4, collection: Enumerable.Range(0, 100), comparer: null);
            var reference = new List<int>(Enumerable.Range(0, 100));

            list.RemoveRange(10, 80);
            reference.RemoveRange(10, 80);
            Assert.Equal(reference, list);

            list.RemoveRange(0, list.Count);
            reference.RemoveRange(0, reference.Count);
            Assert.Equal(reference, list);
            Assert.Empty(list);
        }

        [Fact]
        public void TestIListTInterface()
        {
            IList<int> list = new SortedTreeList<int>(Enumerable.Range(0, 10));
            Assert.False(list.IsReadOnly);
            Assert.Equal(0, list[0]);

            Assert.Throws<NotSupportedException>(() => list.Insert(0, 0));
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(new SortedTreeList<int> { 600, 601 }, isOwnSyncRoot: true, supportsNullValues: false);
            TestICollectionInterfaceImpl(new SortedTreeList<int?> { 600, 601 }, isOwnSyncRoot: true, supportsNullValues: true);
            TestICollectionInterfaceImpl(new SortedTreeList<object> { 600, 601 }, isOwnSyncRoot: true, supportsNullValues: true);

            // Run the same set of tests on List<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(new List<int> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: false);
            TestICollectionInterfaceImpl(new List<int?> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: true);
            TestICollectionInterfaceImpl(new List<object> { 600, 601 }, isOwnSyncRoot: false, supportsNullValues: true);
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

            if (supportsNullValues)
            {
                var copy = new object[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, Assert.Null);
                Assert.Throws<ArgumentException>(() => collection.CopyTo(copy, 1));
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

                // TODO: One of these applies to int?, while the other applies to object. Need to resolve.
                ////Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[collection.Count], 0));
                ////Assert.Throws<InvalidCastException>(() => collection.CopyTo(new string[collection.Count], 0));
            }
            else
            {
                var copy = new int[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(0, item));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(copy, 1));
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

                Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[collection.Count], 0));
            }
        }

        [Fact]
        public void TestIndexer()
        {
            SortedTreeList<int> list = new SortedTreeList<int>(4, Enumerable.Range(0, 10), null);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
            Assert.Throws<ArgumentOutOfRangeException>(() => list[list.Count]);
            Assert.Throws<NotSupportedException>(() => ((IList<int>)list)[-1] = 0);
            Assert.Throws<NotSupportedException>(() => ((IList<int>)list)[list.Count] = 0);

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(i, list[i]);
            }
        }

        [Fact]
        public void TestCopyToValidation()
        {
            SortedTreeList<int> list = new SortedTreeList<int>(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("dest", () => list.CopyTo(0, null!, 0, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>("srcIndex", () => list.CopyTo(-1, new int[list.Count], 0, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => list.CopyTo(0, new int[list.Count], -1, list.Count));
            Assert.Throws<ArgumentOutOfRangeException>("length", () => list.CopyTo(0, new int[list.Count], 0, -1));
            Assert.Throws<ArgumentException>(null, () => list.CopyTo(1, new int[list.Count], 0, list.Count));
            Assert.Throws<ArgumentException>(string.Empty, () => list.CopyTo(0, new int[list.Count], 1, list.Count));
       }

        [Fact]
        public void TestAdd()
        {
            const int Value = 600;

            SortedTreeList<int> list = new SortedTreeList<int>();
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
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            for (int i = 0; i < 2 * 4 * 4; i++)
                list.Add(i);

            list.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 3);
            foreach (var item in expected)
            {
                list.Add(item);
                Assert.Equal(item, list[list.Count - 1]);
            }

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestAddRange()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 3);
            list.AddRange(expected);

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);

            Assert.Throws<ArgumentNullException>(() => list.AddRange(null!));
        }

        [Fact]
        public void TestInsertionLocation()
        {
            var comparer = new ComparisonComparer<StrongBox<int>>((x, y) => x.Value - y.Value);
            var list = new SortedTreeList<StrongBox<int>>(branchingFactor: 4, comparer: comparer);
            for (int i = 0; i < 1000; i++)
            {
                var value = new StrongBox<int>(Generator.GetInt32(0, 200));
                int index = list.LastIndexOf(value);
                if (index < 0)
                {
                    // No item with this value already exists
                    index = list.FindLastIndex(box => box.Value < value.Value);
                }
                else
                {
                    Assert.Equal(list[index].Value, value.Value);
                }

                if (index < list.Count - 1)
                {
                    Assert.True(list[index + 1].Value > value.Value);
                }

                // The item is inserted after the previous last item with this value (or the last item less than it)
                list.Add(value);
                Assert.Same(list[index + 1], value);
                Assert.Equal(index + 1, list.LastIndexOf(new StrongBox<int>(value.Value)));

                // Check IndexOf as well
                int firstInstance = list.IndexOf(new StrongBox<int>(value.Value));
                Assert.True(firstInstance >= 0 && firstInstance < list.Count);
                Assert.Equal(value.Value, list[firstInstance].Value);
                Assert.True(firstInstance == 0 || list[firstInstance - 1].Value < value.Value);

                // Check BinarySearch consistency
                int binarySearch = list.BinarySearch(new StrongBox<int>(value.Value));
                Assert.True(binarySearch >= firstInstance && binarySearch <= index + 1);
                Assert.Equal(firstInstance, list.BinarySearch(0, firstInstance + 1, new StrongBox<int>(value.Value)));
                Assert.Equal(~firstInstance, list.BinarySearch(0, firstInstance, new StrongBox<int>(value.Value)));
                Assert.Equal(index + 1, list.BinarySearch(index + 1, list.Count - index - 1, new StrongBox<int>(value.Value)));
                Assert.Equal(~(index + 2), list.BinarySearch(index + 2, list.Count - index - 2, new StrongBox<int>(value.Value)));
            }
        }

        [Fact]
        public void TestCopyTo()
        {
            var list = new SortedTreeList<int>(branchingFactor: 4, collection: Enumerable.Range(0, 100), comparer: null);
            var reference = new List<int>(Enumerable.Range(0, 100));

            int[] listArray = new int[list.Count * 2];
            int[] referenceArray = new int[reference.Count * 2];

            list.CopyTo(listArray);
            reference.CopyTo(referenceArray);
            Assert.Equal(referenceArray, listArray);

            list.CopyTo(listArray, 0);
            Assert.Equal(referenceArray, listArray);

            list.CopyTo(listArray, list.Count / 2);
            reference.CopyTo(referenceArray, reference.Count / 2);
            Assert.Equal(referenceArray, listArray);
        }

        [Fact]
        public void TestForEach()
        {
            var list = new SortedTreeList<int>(branchingFactor: 4, collection: Enumerable.Range(0, 100), comparer: null);
            var reference = new List<int>(Enumerable.Range(0, 100));

            Assert.Throws<ArgumentNullException>("action", () => list.ForEach(null!));
            Assert.Throws<ArgumentNullException>(() => reference.ForEach(null!));

            var listOutput = new List<int>();
            var referenceOutput = new List<int>();

            list.ForEach(listOutput.Add);
            reference.ForEach(referenceOutput.Add);
            Assert.Equal(referenceOutput, listOutput);
        }

        [Fact]
        public void TestGetRange()
        {
            var list = new SortedTreeList<int>(branchingFactor: 4, collection: Enumerable.Range(0, 100), comparer: null);
            var reference = new List<int>(Enumerable.Range(0, 100));

            SortedTreeList<int> subList = list.GetRange(10, 80);
            List<int> subReference = reference.GetRange(10, 80);
            Assert.Equal(subReference, subList);

            // Verify that changes to the original list do not affect previous calls to GetRange
            int[] values = subList.ToArray();
            list.Add(list[list.Count / 2]);
            reference.Insert((reference.Count / 2) + 1, reference[reference.Count / 2]);
            Assert.Equal(reference, list);
            Assert.Equal(values, subList);
            Assert.Equal(values, subReference);
        }

        [Fact]
        public void TestBinarySearchFullList()
        {
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                list.Add(i * 2);
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

            SortedTreeList<int> empty = new SortedTreeList<int>();
            Assert.Equal(~0, empty.BinarySearch(0));
        }

        [Fact]
        public void TestIndexOf()
        {
            Random random = new Random();
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next(list.Count + 1);
                list.Add(item);
                reference.Add(item);
            }

            reference.Sort();

            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.IndexOf(list[0], 0, -1));
            Assert.Throws<ArgumentException>(() => list.IndexOf(list[0], 0, list.Count + 1));

            Assert.Equal(-1, list.IndexOf(-1));

            for (int i = 0; i < list.Count; i++)
            {
                Assert.Equal(reference.IndexOf(i), list.IndexOf(i));

                int firstIndex = list.IndexOf(i);
                Assert.Equal(reference.IndexOf(i, firstIndex + 1), list.IndexOf(i, firstIndex + 1));
            }

            SortedTreeList<int> empty = new SortedTreeList<int>();
            Assert.Equal(-1, empty.IndexOf(0));
        }

        [Fact]
        public void TestLastIndexOf()
        {
            Random random = new Random();
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            Assert.Equal(-1, list.LastIndexOf(0, -1, 0));
            Assert.Equal(-1, reference.LastIndexOf(0, -1, 0));

            Assert.Throws<ArgumentException>(() => list.LastIndexOf(0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(0, -40, -50));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(0, 40, 50));

            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next(list.Count + 1);
                list.Add(item);
                reference.Add(item);
            }

            reference.Sort();

            Assert.Throws<ArgumentException>(() => list.LastIndexOf(list[0], list.Count));
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
        }

        [Fact]
        public void TestLastIndexOfInvalidOperations()
        {
            SortedTreeList<int> single = new SortedTreeList<int>(Enumerable.Range(1, 1));
            Assert.Throws<ArgumentException>(() => single.LastIndexOf(0, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, 2));
        }

        [Fact]
        public void TestFindIndex()
        {
            Random random = new Random();
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next(list.Count + 1);
                list.Add(item);
                reference.Add(item);
            }

            reference.Sort();

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

            SortedTreeList<int> empty = new SortedTreeList<int>();
            Assert.Equal(-1, empty.FindIndex(i => true));
        }

        [Fact]
        public void TestFindLastIndex()
        {
            Random random = new Random();
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(0, 0, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => reference.FindLastIndex(0, 0, i => true));
            Assert.Equal(-1, list.FindLastIndex(-1, 0, i => true));
            Assert.Equal(-1, reference.FindLastIndex(-1, 0, i => true));

            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next(list.Count + 1);
                list.Add(item);
                reference.Add(item);
            }

            reference.Sort();

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

        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int value = random.Next(list.Count + 1);
                list.Add(i);
                reference.Add(i);
            }

            reference.Sort();

            list.Validate(ValidationRules.None);

            // In the first call to TrimExcess, items will move
            list.TrimExcess();
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            // In the second call, the list is already packed so nothing will move
            list.TrimExcess();
            list.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, list);

            SortedTreeList<int> empty = new SortedTreeList<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            SortedTreeList<int> single = new SortedTreeList<int>(Enumerable.Range(0, 1));
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            SortedTreeList<int> binary = new SortedTreeList<int>(branchingFactor: 2);
            for (int i = 99; i >= 0; i--)
                binary.Add(i);

            binary.TrimExcess();
            binary.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            SortedTreeList<int> ternary = new SortedTreeList<int>(branchingFactor: 3, collection: Enumerable.Range(0, 100), comparer: null);
            for (int i = 99; i >= 0; i--)
                ternary.Add(i);

            ternary.TrimExcess();
            ternary.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestRemoveAt()
        {
            Random random = new Random();
            SortedTreeList<int> list = new SortedTreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next();
                list.Add(item);
                reference.Add(item);
            }

            reference.Sort();

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
        public void TestEnumerator()
        {
            var list = new SortedTreeList<int>();
            var enumerator = list.GetEnumerator();
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
        public void TestIEnumeratorT()
        {
            var list = new SortedTreeList<int>();
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
            var list = new SortedTreeList<int>(4, Enumerable.Range(0, 10), comparer: null);
            Assert.False(list.Remove(-1));
            Assert.Equal(10, list.Count);
            Assert.True(list.Remove(3));
            Assert.Equal(9, list.Count);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var list = new SortedTreeList<int>(4, Enumerable.Range(0, 10), comparer: null);
            Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null!));

            Assert.Equal(5, list.RemoveAll(i => (i % 2) == 0));
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, list);
            Assert.Equal(0, list.RemoveAll(i => i < 0));
            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void TestExists()
        {
            var list = new SortedTreeList<int>(4, Enumerable.Range(0, 10), comparer: null);
            Assert.Throws<ArgumentNullException>(() => list.Exists(null!));

            Assert.False(list.Exists(value => value < 0));
            foreach (var i in list)
            {
                Assert.True(list.Exists(value => value == i));
            }

            Assert.False(list.Exists(value => value > 10));
        }

        [Fact]
        public void TestFind()
        {
            var list = new SortedTreeList<int>(4, Enumerable.Range(1, 10), comparer: null);
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
            var list = new SortedTreeList<int>(4, Enumerable.Range(0, 10), comparer: null);
            Assert.Throws<ArgumentNullException>(() => list.FindAll(null!));

            SortedTreeList<int> found = list.FindAll(i => (i % 2) == 0);

            Assert.Equal(10, list.Count);
            Assert.Equal(5, found.Count);
            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, found);

            Assert.Empty(list.FindAll(i => i < 0));
        }

        [Fact]
        public void TestFindLast()
        {
            var list = new SortedTreeList<int>(4, Enumerable.Range(1, 10), comparer: null);
            var reference = new List<int>(Enumerable.Range(1, 10));
            Assert.Throws<ArgumentNullException>(() => list.FindLast(null!));

            Assert.Equal(0, list.FindLast(i => i < 0));
            Assert.Equal(0, reference.FindLast(i => i < 0));

            Assert.Equal(10, list.FindLast(value => (value % 2) == 0));
            Assert.Equal(10, reference.FindLast(value => (value % 2) == 0));

            Assert.Equal(4, list.FindLast(value => value < 5));
            Assert.Equal(4, reference.FindLast(value => value < 5));
        }

        [Fact]
        public void TestTrueForAll()
        {
            var list = new SortedTreeList<int>();
            Assert.True(list.TrueForAll(i => false));
            Assert.Throws<ArgumentNullException>(() => list.TrueForAll(null!));

            list.Add(1);
            Assert.True(list.TrueForAll(i => i > 0));
            Assert.False(list.TrueForAll(i => i <= 0));
        }
    }
}
