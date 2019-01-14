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

    public class ImmutableTreeSetTest
    {
        [Fact]
        public void TestEmptyImmutableTreeSet()
        {
            var list = ImmutableTreeSet.Create<int>();
            Assert.Same(ImmutableTreeSet<int>.Empty, list);
            Assert.Empty(list);
        }

        [Fact]
        public void TestSingleElementList()
        {
            var value = Generator.GetInt32().ToString();
            var list = ImmutableTreeSet.Create(value);
            Assert.Equal(new[] { value }, list);

            list = ImmutableTreeSet.Create(equalityComparer: null, value);
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(new[] { value }, list);

            list = ImmutableTreeSet.Create(StringComparer.OrdinalIgnoreCase, value);
            Assert.Same(StringComparer.OrdinalIgnoreCase, list.KeyComparer);
            Assert.Equal(new[] { value }, list);
        }

        [Fact]
        public void TestMultipleElementList()
        {
            var values = new[] { Generator.GetInt32().ToString(), Generator.GetInt32().ToString(), Generator.GetInt32().ToString() };

            // Construction using ImmutableTreeSet.Create
            var list = ImmutableTreeSet.Create(values);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = ImmutableTreeSet.Create<string>(equalityComparer: null, values);
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = ImmutableTreeSet.Create(StringComparer.OrdinalIgnoreCase, values);
            Assert.Same(StringComparer.OrdinalIgnoreCase, list.KeyComparer);
            Assert.Equal(values.OrderBy(StringComparer.OrdinalIgnoreCase.GetHashCode), list);

            // Construction using ImmutableTreeSet.ToImmutableTreeSet
            list = values.ToImmutableTreeSet();
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = values.ToImmutableTreeSet(equalityComparer: null);
            Assert.Same(EqualityComparer<string>.Default, list.KeyComparer);
            Assert.Equal(values.OrderBy(EqualityComparer<string>.Default.GetHashCode), list);

            list = values.ToImmutableTreeSet(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, list.KeyComparer);
            Assert.Equal(values.OrderBy(StringComparer.OrdinalIgnoreCase.GetHashCode), list);
        }

        [Fact]
        public void TestImmutableTreeSetCreateValidation()
        {
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.Create(default(int[])));
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.Create(EqualityComparer<int>.Default, default(int[])));
        }

        [Fact]
        public void TestImmutableTreeSetCreateRange()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var list = ImmutableTreeSet.CreateRange(values);
            Assert.Equal(values.OrderBy(x => x), list);
        }

        [Fact]
        public void TestImmutableTreeSetCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.CreateRange<int>(null));
            Assert.Throws<ArgumentNullException>("other", () => ImmutableTreeSet.CreateRange(EqualityComparer<int>.Default, null));
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<int>).ToImmutableTreeSet());
            Assert.Throws<ArgumentNullException>("source", () => default(IEnumerable<int>).ToImmutableTreeSet(EqualityComparer<int>.Default));
        }

        [Fact]
        public void TestCollectionConstructorUsesCorrectComparer()
        {
            object instance1 = new object();
            object instance2 = new object();
            var objectSet = ImmutableTreeSet.Create<object>(ZeroHashCodeEqualityComparer<object>.Default, new[] { instance1, instance2, instance1 });
            Assert.Same(ZeroHashCodeEqualityComparer<object>.Default, objectSet.KeyComparer);
            Assert.Equal(2, objectSet.Count);
            Assert.Equal(new[] { instance1, instance2 }, objectSet);

            var stringSet = ImmutableTreeSet.Create<string>();
            Assert.Same(EqualityComparer<string>.Default, stringSet.KeyComparer);

            stringSet = ImmutableTreeSet.Create<string>(StringComparer.OrdinalIgnoreCase);
            Assert.Same(StringComparer.OrdinalIgnoreCase, stringSet.KeyComparer);
        }

        [Fact]
        public void TestDefaultComparer()
        {
            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.Create<object>().KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.Create<int>().KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.Create<IComparable>().KeyComparer);

            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.CreateRange<object>(Enumerable.Empty<object>()).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.CreateRange<int>(Enumerable.Empty<int>()).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.CreateRange<IComparable>(Enumerable.Empty<IComparable>()).KeyComparer);

            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.Create<object>(equalityComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.Create<int>(equalityComparer: null).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.Create<IComparable>(equalityComparer: null).KeyComparer);

            Assert.Same(EqualityComparer<object>.Default, ImmutableTreeSet.CreateRange<object>(equalityComparer: null, Enumerable.Empty<object>()).KeyComparer);
            Assert.Same(EqualityComparer<int>.Default, ImmutableTreeSet.CreateRange<int>(equalityComparer: null, Enumerable.Empty<int>()).KeyComparer);
            Assert.Same(EqualityComparer<IComparable>.Default, ImmutableTreeSet.CreateRange<IComparable>(equalityComparer: null, Enumerable.Empty<IComparable>()).KeyComparer);
        }

        [Fact]
        public void TestExplicitComparer()
        {
            ZeroHashCodeEqualityComparer<object> objComparer = ZeroHashCodeEqualityComparer<object>.Default;
            ZeroHashCodeEqualityComparer<int> intComparer = ZeroHashCodeEqualityComparer<int>.Default;
            ZeroHashCodeEqualityComparer<IComparable> comparableComparer = ZeroHashCodeEqualityComparer<IComparable>.Default;

            Assert.Same(objComparer, ImmutableTreeSet.Create<object>(equalityComparer: objComparer).KeyComparer);
            Assert.Same(intComparer, ImmutableTreeSet.Create<int>(equalityComparer: intComparer).KeyComparer);
            Assert.Same(comparableComparer, ImmutableTreeSet.Create<IComparable>(equalityComparer: comparableComparer).KeyComparer);

            Assert.Same(objComparer, ImmutableTreeSet.CreateRange<object>(equalityComparer: objComparer, Enumerable.Empty<object>()).KeyComparer);
            Assert.Same(intComparer, ImmutableTreeSet.CreateRange<int>(equalityComparer: intComparer, Enumerable.Empty<int>()).KeyComparer);
            Assert.Same(comparableComparer, ImmutableTreeSet.CreateRange<IComparable>(equalityComparer: comparableComparer, Enumerable.Empty<IComparable>()).KeyComparer);
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(ImmutableTreeSet.Create(600, 601), supportsNullValues: false);
            TestICollectionInterfaceImpl(ImmutableTreeSet.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionInterfaceImpl(ImmutableTreeSet.Create<object>(600, 601), supportsNullValues: true);

            ICollection collection = ImmutableTreeSet<int>.Empty;
            collection.CopyTo(new int[0], 0);

            // Type checks are only performed if the collection has items
            collection.CopyTo(new string[0], 0);

            collection = ImmutableTreeSet.CreateRange(Enumerable.Range(0, 100));
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

            Assert.Throws<ArgumentNullException>("array", () => collection.CopyTo(null, 0));
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
            }
            else
            {
                var copy = new int[collection.Count];

                Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(copy, -1));
                Assert.All(copy, item => Assert.Equal(0, item));
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
            }
        }

        [Fact]
        public void TestICollectionTInterface()
        {
            TestICollectionTInterfaceImpl(ImmutableTreeSet.Create(600, 601), supportsNullValues: false);
            TestICollectionTInterfaceImpl(ImmutableTreeSet.Create<int?>(600, 601), supportsNullValues: true);
            TestICollectionTInterfaceImpl(ImmutableTreeSet.Create<object>(600, 601), supportsNullValues: true);

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
            var set = ImmutableTreeSet.CreateRange(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("array", () => ((ICollection<int>)set).CopyTo(array: null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("arrayIndex", () => ((ICollection<int>)set).CopyTo(new int[set.Count], -1));
            Assert.Throws<ArgumentException>(string.Empty, () => ((ICollection<int>)set).CopyTo(new int[set.Count - 1], 0));
            Assert.Throws<ArgumentException>(() => ((ICollection<int>)set).CopyTo(new int[set.Count], 1));
        }

        [Fact]
        public void TestAdd()
        {
            const int Value = 600;

            var set = ImmutableTreeSet.Create<int>();
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

            IImmutableSet<int> set = ImmutableTreeSet.Create<int>();
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
            var set = ImmutableTreeSet.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
                set.Add(i);

            set.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestAddMany()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            var set = ImmutableTreeSet.Create<int>();
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
        public void TestUnion()
        {
            int[] expected = Enumerable.Range(600, 8 * 9).ToArray();

            var set = ImmutableTreeSet.Create<int>();
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

            IImmutableSet<int> set = ImmutableTreeSet.Create<int>();
            set = set.Union(expected);

            Assert.Equal(expected.Length, set.Count);

            int[] actual = set.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            var set = ImmutableTreeSet.CreateRange(Enumerable.Range(600, 8 * 9));
            Assert.Equal(8 * 9, set.Count);
            Assert.Empty(set.Clear());
            Assert.Same(ImmutableTreeSet<int>.Empty, set.Clear());

            var stringList = ImmutableTreeSet.CreateRange(StringComparer.Ordinal, new[] { "a", "b" });
            ImmutableTreeSet<string> emptyStringList = stringList.Clear();
            Assert.Same(stringList.KeyComparer, emptyStringList.KeyComparer);
            Assert.Same(emptyStringList, emptyStringList.Clear());
        }

        [Fact]
        public void TestClearViaInterface()
        {
            IImmutableSet<int> set = ImmutableTreeSet.CreateRange(Enumerable.Range(600, 8 * 9));
            Assert.Equal(8 * 9, set.Count);
            Assert.Empty(set.Clear());
            Assert.Same(ImmutableTreeSet<int>.Empty, set.Clear());
        }

        [Fact]
        public void TestExcept()
        {
            var random = new Random();
            var set = ImmutableTreeSet.Create<int>();
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

            ImmutableTreeSet<int> pruned = set.Except(itemsToRemove);
            ImmutableList<int> referencePruned = reference.RemoveRange(itemsToRemove);
            Assert.Equal(referencePruned, pruned);

            IImmutableSet<int> immutableSet = set;
            IImmutableSet<int> prunedViaInterface = immutableSet.Except(itemsToRemove);
            Assert.IsType<ImmutableTreeSet<int>>(prunedViaInterface);
            Assert.Equal(referencePruned, prunedViaInterface);

            Assert.Throws<ArgumentNullException>("other", () => set.Except(other: null));
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableTreeSet<int> set = ImmutableTreeSet.Create<int>();
            ImmutableTreeSet<int>.Enumerator enumerator = set.GetEnumerator();
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
            var set = ImmutableTreeSet.Create<int>();
            IEnumerator<int> enumerator = set.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the list doesn't affect the enumerator
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
            var set = ImmutableTreeSet.CreateRange(Enumerable.Range(0, 8 * 4));
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
    }
}
