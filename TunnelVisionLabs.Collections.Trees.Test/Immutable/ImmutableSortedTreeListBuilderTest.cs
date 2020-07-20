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
    using ICollection = System.Collections.ICollection;
    using IList = System.Collections.IList;

    public class ImmutableSortedTreeListBuilderTest
    {
        [Fact]
        public void TestSortedTreeListBuilderConstructor()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            Assert.Equal(Comparer<int>.Default, list.Comparer);

            ImmutableSortedTreeList<string?>.Builder stringList = ImmutableSortedTreeList.CreateBuilder(StringComparer.Ordinal);
            Assert.Same(StringComparer.Ordinal, stringList.Comparer);

            stringList = ImmutableSortedTreeList.CreateBuilder(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringList.Comparer);
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeList.CreateBuilder<object>().Comparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeList.CreateBuilder<int>().Comparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeList.CreateBuilder<IComparable>().Comparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeList.CreateRange<object>(Enumerable.Empty<object>()).ToBuilder().Comparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeList.CreateRange<int>(Enumerable.Empty<int>()).ToBuilder().Comparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeList.CreateRange<IComparable>(Enumerable.Empty<IComparable>()).ToBuilder().Comparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeList.CreateBuilder<object>(comparer: null).Comparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeList.CreateBuilder<int>(comparer: null).Comparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeList.CreateBuilder<IComparable>(comparer: null).Comparer);

            Assert.Same(Comparer<object>.Default, ImmutableSortedTreeList.CreateRange<object>(comparer: null, Enumerable.Empty<object>()).ToBuilder().Comparer);
            Assert.Same(Comparer<int>.Default, ImmutableSortedTreeList.CreateRange<int>(comparer: null, Enumerable.Empty<int>()).ToBuilder().Comparer);
            Assert.Same(Comparer<IComparable>.Default, ImmutableSortedTreeList.CreateRange<IComparable>(comparer: null, Enumerable.Empty<IComparable>()).ToBuilder().Comparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            var objComparer = new ComparisonComparer<object>((x, y) => 0);
            var intComparer = new ComparisonComparer<int>((x, y) => 0);
            var comparableComparer = new ComparisonComparer<IComparable>((x, y) => 0);

            Assert.Same(objComparer, ImmutableSortedTreeList.CreateBuilder<object>(comparer: objComparer).Comparer);
            Assert.Same(intComparer, ImmutableSortedTreeList.CreateBuilder<int>(comparer: intComparer).Comparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeList.CreateBuilder<IComparable>(comparer: comparableComparer).Comparer);

            Assert.Same(objComparer, ImmutableSortedTreeList.CreateRange<object>(comparer: objComparer, Enumerable.Empty<object>()).ToBuilder().Comparer);
            Assert.Same(intComparer, ImmutableSortedTreeList.CreateRange<int>(comparer: intComparer, Enumerable.Empty<int>()).ToBuilder().Comparer);
            Assert.Same(comparableComparer, ImmutableSortedTreeList.CreateRange<IComparable>(comparer: comparableComparer, Enumerable.Empty<IComparable>()).ToBuilder().Comparer);
        }

        [Fact]
        public void TestIListInterface()
        {
            TestIListInterfaceImpl(ImmutableSortedTreeList.Create<int>(600, 601).ToBuilder(), supportsNullValues: false);
            TestIListInterfaceImpl(ImmutableSortedTreeList.Create<int?>(600, 601).ToBuilder(), supportsNullValues: true);
            TestIListInterfaceImpl(ImmutableSortedTreeList.Create<ValueType>(600, 601).ToBuilder(), supportsNullValues: true);
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
        public void TestIListTInterface()
        {
            IList<int> list = ImmutableSortedTreeList.CreateRange<int>(Enumerable.Range(0, 10)).ToBuilder();
            Assert.False(list.IsReadOnly);
            Assert.Equal(0, list[0]);

            Assert.Throws<NotSupportedException>(() => list.Insert(0, 0));
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(ImmutableSortedTreeList.Create<int>(600, 601).ToBuilder(), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableSortedTreeList.Create<int?>(600, 601).ToBuilder(), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableSortedTreeList.Create<object>(600, 601).ToBuilder(), supportsNullValues: true);

            // Run the same set of tests on ImmutableTreeList<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<int>(600, 601).ToBuilder(), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<int?>(600, 601).ToBuilder(), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableTreeList.Create<object>(600, 601).ToBuilder(), supportsNullValues: true);
        }

        private static void TestICollectionInterfaceImpl(ICollection collection, bool supportsNullValues)
        {
            Assert.False(collection.IsSynchronized);

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
        public void TestIndexer()
        {
            var list = ImmutableSortedTreeList.CreateRange<int>(null, Enumerable.Range(0, 10)).ToBuilder();
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
            var list = ImmutableSortedTreeList.CreateRange<int>(Enumerable.Range(0, 10)).ToBuilder();
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
            int value = Generator.GetInt32();

            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            list.Add(value);
            Assert.Single(list);
            Assert.Equal(value, list[0]);
            int[] expected = { value };
            int[] actual = list.ToArray();
            Assert.Equal(expected, actual);

            Assert.True(list.Add(value, addIfPresent: true));
            Assert.Equal(new[] { value, value }, list);

            Assert.False(list.Add(value, addIfPresent: false));
            Assert.Equal(new[] { value, value }, list);
        }

        [Fact]
        public void TestAddStaysPacked()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                // This test assumes items are being added are already ordered by hash code
                Assert.Equal(i, i.GetHashCode());

                list.Add(i);
                list.Validate(ValidationRules.RequirePacked);
            }
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = { 600, 601, 602, 603, 700, 701, 702, 703, 800, 801, 802, 803 };

            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
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

            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
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
            ImmutableSortedTreeList<StrongBox<int>>.Builder list = ImmutableSortedTreeList.CreateBuilder(comparer);
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
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, items: Enumerable.Range(0, 100)).ToBuilder();
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
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(0, 100)).ToBuilder();
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
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(0, 100)).ToBuilder();
            var reference = new List<int>(Enumerable.Range(0, 100));

            ImmutableSortedTreeList<int> subList = list.GetRange(10, 80);
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
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            var reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
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

            ImmutableSortedTreeList<int>.Builder empty = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Equal(~0, empty.BinarySearch(0));
        }

        [Fact]
        public void TestIndexOf()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            var reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = Generator.GetInt32(list.Count + 1);
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

            ImmutableSortedTreeList<int>.Builder empty = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Equal(-1, empty.IndexOf(0));
        }

        [Fact]
        public void TestLastIndexOf()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            var reference = new List<int>();
            Assert.Equal(-1, list.LastIndexOf(0, -1, 0));
            Assert.Equal(-1, reference.LastIndexOf(0, -1, 0));

            Assert.Throws<ArgumentException>(() => list.LastIndexOf(0, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(0, -40, -50));
            Assert.Throws<ArgumentOutOfRangeException>(() => list.LastIndexOf(0, 40, 50));

            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = Generator.GetInt32(list.Count + 1);
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
            var single = ImmutableSortedTreeList.CreateRange(Enumerable.Range(1, 1)).ToBuilder();
            Assert.Throws<ArgumentException>(() => single.LastIndexOf(0, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => single.LastIndexOf(0, 0, 2));
        }

        [Fact]
        public void TestFindIndex()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            var reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = Generator.GetInt32(list.Count + 1);
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

            ImmutableSortedTreeList<int>.Builder empty = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Equal(-1, empty.FindIndex(i => true));
        }

        [Fact]
        public void TestFindLastIndex()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            var reference = new List<int>();
            Assert.Throws<ArgumentOutOfRangeException>(() => list.FindLastIndex(0, 0, i => true));
            Assert.Throws<ArgumentOutOfRangeException>(() => reference.FindLastIndex(0, 0, i => true));
            Assert.Equal(-1, list.FindLastIndex(-1, 0, i => true));
            Assert.Equal(-1, reference.FindLastIndex(-1, 0, i => true));

            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = Generator.GetInt32(list.Count + 1);
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
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            List<int> reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int value = Generator.GetInt32(list.Count + 1);
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

            ImmutableSortedTreeList<int>.Builder empty = ImmutableSortedTreeList.CreateBuilder<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            var single = ImmutableSortedTreeList.CreateRange(Enumerable.Range(0, 1)).ToBuilder();
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            ImmutableSortedTreeList<int>.Builder binary = ImmutableSortedTreeList.CreateBuilder<int>();
            for (int i = 5000; i >= 0; i--)
                binary.Add(i);

            binary.TrimExcess();
            binary.Validate(ValidationRules.RequirePacked);

            // Construct a poorly-packed list with several levels
            ImmutableSortedTreeList<int>.Builder ternary = ImmutableSortedTreeList.CreateRange<int>(comparer: null, Enumerable.Range(0, 5000)).ToBuilder();
            for (int i = 5000; i >= 0; i--)
                ternary.Add(i);

            ternary.TrimExcess();
            ternary.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestRemoveAt()
        {
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            var reference = new List<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
            {
                int item = Generator.GetInt32();
                list.Add(item);
                reference.Add(item);
            }

            reference.Sort();

            while (list.Count > 0)
            {
                int index = Generator.GetInt32(list.Count);
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
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            ImmutableSortedTreeList<int>.Enumerator enumerator = list.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list invalidates it, but Current is still unchecked
            CollectionAssert.EnumeratorInvalidated(list, () => list.Add(1));
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
        public void TestRemoveValue()
        {
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(0, 10)).ToBuilder();
            Assert.False(list.Remove(-1));
            Assert.Equal(10, list.Count);
            Assert.True(list.Remove(3));
            Assert.Equal(9, list.Count);
        }

        [Fact]
        public void TestRemoveAll()
        {
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(0, 10)).ToBuilder();
            Assert.Throws<ArgumentNullException>(() => list.RemoveAll(null!));

            Assert.Equal(5, list.RemoveAll(i => (i % 2) == 0));
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, list);
            Assert.Equal(0, list.RemoveAll(i => i < 0));
            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void TestExists()
        {
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(0, 10)).ToBuilder();
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
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(1, 10)).ToBuilder();
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
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(0, 10)).ToBuilder();
            Assert.Throws<ArgumentNullException>(() => list.FindAll(null!));

            ImmutableSortedTreeList<int> found = list.FindAll(i => (i % 2) == 0);

            Assert.Equal(10, list.Count);
            Assert.Equal(5, found.Count);
            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, found);

            Assert.Empty(list.FindAll(i => i < 0));
        }

        [Fact]
        public void TestFindLast()
        {
            var list = ImmutableSortedTreeList.CreateRange(comparer: null, Enumerable.Range(1, 10)).ToBuilder();
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
        public void TestToImmutable()
        {
            int value = Generator.GetInt32();

            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.Empty(list);
            Assert.Same(ImmutableSortedTreeList<int>.Empty, list.ToImmutable());

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
            ImmutableSortedTreeList<int>.Builder list = ImmutableSortedTreeList.CreateBuilder<int>();
            Assert.True(list.TrueForAll(i => false));
            Assert.Throws<ArgumentNullException>(() => list.TrueForAll(null!));

            list.Add(1);
            Assert.True(list.TrueForAll(i => i > 0));
            Assert.False(list.TrueForAll(i => i <= 0));
        }
    }
}
