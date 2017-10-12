// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tvl.Collections.Trees;
    using Xunit;
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;

    public class TestTreeList
    {
        [Fact]
        public void TestTreeListConstructor()
        {
            TreeList<int> list = new TreeList<int>();
            Assert.Empty(list);
        }

        [Fact]
        public void TestTreeListBranchingFactorConstructor()
        {
            TreeList<int> list = new TreeList<int>(8);
            Assert.Empty(list);

            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeList<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeList<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeList<int>(1));
        }

        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(new TreeList<int> { 600, 601 }, supportsNullValues: false);
            TestIListInterfaceImpl(new TreeList<int?> { 600, 601 }, supportsNullValues: true);
            TestIListInterfaceImpl(new TreeList<object> { 600, 601 }, supportsNullValues: true);

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
            TestICollectionInterfaceImpl(new TreeList<int> { 600, 601 }, supportsNullValues: false);
            TestICollectionInterfaceImpl(new TreeList<int?> { 600, 601 }, supportsNullValues: true);
            TestICollectionInterfaceImpl(new TreeList<object> { 600, 601 }, supportsNullValues: true);

            // Run the same set of tests on List<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(new List<int> { 600, 601 }, supportsNullValues: false);
            TestICollectionInterfaceImpl(new List<int?> { 600, 601 }, supportsNullValues: true);
            TestICollectionInterfaceImpl(new List<object> { 600, 601 }, supportsNullValues: true);
        }

        private static void TestICollectionInterfaceImpl(ICollection collection, bool supportsNullValues)
        {
            Assert.False(collection.IsSynchronized);

            Assert.IsType<object>(collection.SyncRoot);
            Assert.Same(collection.SyncRoot, collection.SyncRoot);

            if (supportsNullValues)
            {
                var copy = new object[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => copy.CopyTo(copy, -1));
                Assert.All(copy, Assert.Null);
                Assert.Throws<ArgumentException>(() => copy.CopyTo(copy, 1));
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

                Assert.Throws<ArgumentOutOfRangeException>(() => copy.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(0, item));
                Assert.Throws<ArgumentException>(() => copy.CopyTo(copy, 1));
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
            TreeList<int> list = new TreeList<int>(4, Enumerable.Range(0, 10));
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
            TreeList<int> list = new TreeList<int>(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("dest", () => list.CopyTo(0, null, 0, list.Count));
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

            TreeList<int> list = new TreeList<int>();
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
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            for (int i = 0; i < 2 * 4 * 4; i++)
                list.Add(i);

            list.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            TreeList<int> list = new TreeList<int>(branchingFactor: 3);
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
        public void TestInsert()
        {
            const int Value = 600;

            TreeList<int> list = new TreeList<int>();
            Assert.Empty(list);
            list.Insert(0, Value);
            Assert.Single(list);
            Assert.Equal(Value, list[0]);
            int[] expected = { Value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestInsertMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            TreeList<int> list = new TreeList<int>(branchingFactor: 3);
            foreach (var item in expected.Reverse())
            {
                list.Insert(0, item);
                Assert.Equal(item, list[0]);
            }

            Assert.Equal(expected.Length, list.Count);

            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestBinarySearchFullList()
        {
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
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

            TreeList<int> empty = new TreeList<int>();
            Assert.Equal(~0, empty.BinarySearch(0));
        }

        [Fact]
        public void TestListIndexOfInvalidOperations()
        {
            TreeList<int> empty = new TreeList<int>();
            Assert.Equal(-1, empty.IndexOf(0));
            Assert.Equal(-1, empty.IndexOf(0, 0));
            Assert.Equal(-1, empty.IndexOf(0, 0, 0));

            Assert.Equal(-1, empty.LastIndexOf(0));
            Assert.Equal(-1, empty.LastIndexOf(0, -1));
            Assert.Equal(-1, empty.LastIndexOf(0, -1, 0));

            Assert.Throws<ArgumentOutOfRangeException>(() => empty.LastIndexOf(0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => empty.LastIndexOf(0, -1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => empty.LastIndexOf(0, -1, 1));

            TreeList<int> single = new TreeList<int>(Enumerable.Range(1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, 2));
        }

        [Fact]
        public void TestFindIndex()
        {
            Random random = new Random();
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
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

            for (int i = 0; i < list.Count; i++)
            {
                Predicate<int> predicate = value => value == i;
                Assert.Equal(reference.FindIndex(predicate), list.FindIndex(predicate));

                int firstIndex = list.FindIndex(predicate);
                Assert.Equal(reference.FindIndex(firstIndex + 1, predicate), list.FindIndex(firstIndex + 1, predicate));
            }

            TreeList<int> empty = new TreeList<int>();
            Assert.Equal(-1, empty.FindIndex(i => true));
        }

        [Fact]
        public void TestFindLastIndex()
        {
            Random random = new Random();
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(0, 0, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => reference.FindLastIndex(0, 0, i => true));
            Assert.Equal(-1, list.FindLastIndex(-1, 0, i => true));
            Assert.Equal(-1, reference.FindLastIndex(-1, 0, i => true));

            for (int i = 0; i < 2 * 4 * 4; i++)
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
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

            TreeList<string> stringList = list.ConvertAll(value => value.ToString());
            List<string> referenceStringList = reference.ConvertAll(value => value.ToString());

            stringList.Validate(ValidationRules.None);
            Assert.Equal(reference, list);
            Assert.Equal(referenceStringList, stringList);

            TreeList<int> empty = new TreeList<int>();
            Assert.Empty(empty);
            TreeList<string> stringEmpty = empty.ConvertAll(value => value.ToString());
            Assert.Empty(stringEmpty);
        }

        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
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

            TreeList<int> empty = new TreeList<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestSort()
        {
            Random random = new Random();
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

            Assert.Throws<ArgumentOutOfRangeException>(() => list.Sort(-1, 0, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.Sort(0, -1, null));
            Assert.Throws<ArgumentException>(() => list.Sort(0, list.Count + 1, null));

            list.Sort();
            reference.Sort();
            Assert.Equal(reference, list);

            TreeList<int> empty = new TreeList<int>();
            empty.Sort();
            Assert.Empty(empty);
            empty.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestSortComparison()
        {
            Random random = new Random();
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
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
        public void TestRemoveAt(int seed)
        {
            Random random = new Random(seed);
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int index = random.Next(list.Count + 1);
                int item = random.Next();
                list.Insert(index, item);
                reference.Insert(index, item);
            }

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
        public void TestQueueLikeBehavior()
        {
            Random random = new Random();
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
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
            TreeList<int> list = new TreeList<int>(branchingFactor: 4);
            List<int> reference = new List<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
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
            var list = new TreeList<int>();
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
            Assert.Equal(0, enumerator.Current);
        }

        [Fact]
        public void TestIEnumeratorT()
        {
            var list = new TreeList<int>();
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
            Assert.Equal(0, enumerator.Current);

            enumerator.Reset();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
            enumerator.Reset();
            Assert.Equal(0, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Equal(1, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
        }

        [Fact]
        public void TestRemoveValue()
        {
            var list = new TreeList<int>(4, Enumerable.Range(0, 10));
            Assert.False(list.Remove(-1));
            Assert.Equal(10, list.Count);
            Assert.True(list.Remove(3));
            Assert.Equal(9, list.Count);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var list = new TreeList<int>(4, Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null));

            Assert.Equal(5, list.RemoveAll(i => (i % 2) == 0));
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, list);
            Assert.Equal(0, list.RemoveAll(i => i < 0));
            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void TestExists()
        {
            var list = new TreeList<int>(4, Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>(() => list.Exists(null));

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
            var list = new TreeList<int>(4, Enumerable.Range(1, 10));
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
            var list = new TreeList<int>(4, Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>(() => list.FindAll(null));

            TreeList<int> found = list.FindAll(i => (i % 2) == 0);

            Assert.Equal(10, list.Count);
            Assert.Equal(5, found.Count);
            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, found);

            Assert.Empty(list.FindAll(i => i < 0));
        }

        [Fact]
        public void TestFindLast()
        {
            var list = new TreeList<int>(4, Enumerable.Range(1, 10));
            var reference = new List<int>(Enumerable.Range(1, 10));
            Assert.Throws<ArgumentNullException>(() => list.FindLast(null));

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
            var list = new TreeList<int>();
            Assert.True(list.TrueForAll(i => false));
            Assert.Throws<ArgumentNullException>(() => list.TrueForAll(null));

            list.Add(1);
            Assert.True(list.TrueForAll(i => i > 0));
            Assert.False(list.TrueForAll(i => i <= 0));
        }
    }
}
