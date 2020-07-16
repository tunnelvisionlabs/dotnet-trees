// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit;

    public abstract class AbstractImmutableSetTest
    {
        [Fact]
        public void TestUnion()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.Union(null));

            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 7)));
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(5, 5)));
            Assert.Equal(Enumerable.Range(0, 10), set);
        }

        [Fact]
        public void TestExcept()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.Except(null));

            // Return without iterating if the set is already empty
            Assert.Same(set, set.Except(EverythingThrowsEnumerable<int>.Instance));

            // Test ExceptWith self
            set = set.Add(1);
            set = set.Except(TransformEnumerableForSetOperation(set));
            Assert.Empty(set);

            // Test ExceptWith subset
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            set = set.Except(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 }));
            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, set);
            set = set.Except(TransformEnumerableForSetOperation(new[] { 0, 2, 4, 6, 8 }));
            Assert.Empty(set);
        }

        [Fact]
        public void TestIntersect()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.Intersect(null));

            // Return without iterating if the set is already empty
            Assert.Same(set, set.Intersect(EverythingThrowsEnumerable<int>.Instance));

            // Test IntersectWith self
            set = set.Add(1);
            set = set.Intersect(TransformEnumerableForSetOperation(set));
            Assert.Equal(new[] { 1 }, set);

            // Test IntersectWith array
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            set = set.Intersect(TransformEnumerableForSetOperation(new[] { 1, 3, 11, 5, 7, 9 }));
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, set);

            // Test IntersectWith same set type
            IImmutableSet<int> other = CreateSet<int>();
            other = other.Union(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            set = set.Intersect(TransformEnumerableForSetOperation(other));
            Assert.Equal(new[] { 3, 5, 7 }, set);
        }

        [Fact]
        public void TestSymmetricExcept()
        {
            IImmutableSet<int> set = CreateSet<int>();
            IImmutableSet<int> second = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.SymmetricExcept(null));

            // Test behavior when the current set is empty
            set = set.SymmetricExcept(TransformEnumerableForSetOperation(new[] { 1, 5, 3 }));
            second = second.Union(TransformEnumerableForSetOperation(new[] { 1, 5, 3 }));
            Assert.Equal(second.ToArray(), set);

            // Test SymmetricExceptWith self
            Assert.NotEmpty(set);
            set = set.SymmetricExcept(TransformEnumerableForSetOperation(set));
            Assert.Empty(set);
            set = set.SymmetricExcept(TransformEnumerableForSetOperation(set));
            Assert.Empty(set);

            // Test SymmetricExceptWith same set type
            IImmutableSet<int> other = CreateSet<int>();
            set = set.Union(TransformEnumerableForSetOperation(new[] { 1, 3, 5 }));
            other = other.Union(TransformEnumerableForSetOperation(new[] { 3, 5, 7 }));
            set = set.SymmetricExcept(TransformEnumerableForSetOperation(other));
            Assert.Equal(new[] { 1, 7 }, set);

            // Test SymmetricExceptWith same set type
            set = set.Clear();
            other = CreateSet<int>();
            set = set.Union(TransformEnumerableForSetOperation(new[] { 1, 3, 5 }));
            other = other.Union(TransformEnumerableForSetOperation(new[] { 3, 5, 7 }));
            set = set.SymmetricExcept(TransformEnumerableForSetOperation(other.ToArray()));
            Assert.Equal(new[] { 1, 7 }, set);
        }

        [Fact]
        public void TestIsProperSubsetOf()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsProperSubsetOf(null));

            // Test behavior when the current set is empty
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(Enumerable.Empty<int>())));
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 1))));

            // Test IsProperSubsetOf self
            set = set.Add(1);
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(set)));
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsProperSubsetOf array
            set = set.Union(new[] { 3, 5, 7 });
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 7, 9 })));
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsProperSubsetOf same set type
            set = set.Clear();
            set = set.Union(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            IImmutableSet<int> other = CreateSet<int>();
            other = other.Union(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsProperSubsetOf(TransformEnumerableForSetOperation(set)));

            set = set.Remove(5);
            set = set.Add(8);
            Assert.True(set.Count < other.Count);
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestIsProperSupersetOf()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsProperSupersetOf(null));

            // Return without iterating if the set is already empty
            Assert.False(set.IsProperSupersetOf(EverythingThrowsEnumerable<int>.Instance));

            // Test IsProperSupersetOf self
            set = set.Add(1);
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(set)));
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsProperSupersetOf empty
            Assert.NotEmpty(set);
            Assert.True(set.IsProperSupersetOf(TransformEnumerableForSetOperation(new int[0])));

            // Test IsProperSupersetOf array
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.IsProperSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 11, 5, 7, 9 })));
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsProperSupersetOf same set type
            set = set.Clear();
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            IImmutableSet<int> other = CreateSet<int>();
            other = other.Union(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            Assert.True(set.IsProperSupersetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsProperSupersetOf(TransformEnumerableForSetOperation(set)));

            other = other.Remove(5);
            other = other.Add(8);
            Assert.True(set.Count > other.Count);
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestIsSubsetOf()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsSubsetOf(null));

            // Return without iterating if the set is already empty
            Assert.True(set.IsSubsetOf(EverythingThrowsEnumerable<int>.Instance));

            // Test IsSubsetOf self
            set = set.Add(1);
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(set)));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsSubsetOf array
            set = set.Union(TransformEnumerableForSetOperation(new[] { 3, 5, 7 }));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 7, 9 })));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsSubsetOf same set type
            set = set.Clear();
            set = set.Union(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            IImmutableSet<int> other = CreateSet<int>();
            other = other.Union(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsSubsetOf(TransformEnumerableForSetOperation(set)));

            set = set.Remove(5);
            set = set.Add(8);
            Assert.True(set.Count < other.Count);
            Assert.False(set.IsSubsetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestIsSupersetOf()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsSupersetOf(null));

            // Test IsSupersetOf self
            set = set.Add(1);
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(set)));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsSupersetOf empty
            Assert.NotEmpty(set);
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(new int[0])));

            // Test IsSupersetOf array
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 11, 5, 7, 9 })));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsSupersetOf same set type
            set = set.Clear();
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            IImmutableSet<int> other = CreateSet<int>();
            other = other.Union(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsSupersetOf(TransformEnumerableForSetOperation(set)));

            other = other.Remove(5);
            other = other.Add(8);
            Assert.True(set.Count > other.Count);
            Assert.False(set.IsSupersetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestOverlaps()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.Overlaps(null));

            // Return without iterating if the set is already empty
            Assert.False(set.Overlaps(EverythingThrowsEnumerable<int>.Instance));

            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.False(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Empty<int>())));
            Assert.False(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(-2, 2))));
            Assert.False(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(10, 2))));
            Assert.True(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(-2, 3))));
            Assert.True(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(9, 3))));
            Assert.True(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(3, 1))));
            Assert.True(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(0, 1))));
            Assert.True(set.Overlaps(TransformEnumerableForSetOperation(Enumerable.Range(9, 1))));
        }

        [Fact]
        public void TestSetEquals()
        {
            IImmutableSet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.SetEquals(null));

            // Test behavior when the current set is empty
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(Enumerable.Empty<int>())));
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(Enumerable.Range(0, 1))));

            // Test SetEquals self
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(set)));

            // Test with same set type
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            IImmutableSet<int> other = CreateSet<int>();
            other = other.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(other)));

            other = other.Remove(0);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));
            other = other.Add(-1);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));
            other = other.Remove(-1);
            other = other.Add(0);
            other = other.Remove(8);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));
            other = other.Add(11);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));

            // Test with different set type
            set = set.Clear();
            other = other.Clear();
            set = set.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            other = other.Union(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(other.Concat(other).ToArray())));

            other = other.Remove(0);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            other = other.Add(-1);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            other = other.Remove(-1);
            other = other.Add(0);
            other = other.Remove(8);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            other = other.Add(11);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
        }

        protected abstract IImmutableSet<T> CreateSet<T>();

        protected virtual IEnumerable<T> TransformEnumerableForSetOperation<T>(IEnumerable<T> enumerable)
            => enumerable;

        protected static void TestICollectionInterfaceImpl(ICollection collection, bool supportsNullValues)
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

                // TODO: One of these applies to int?, while the other applies to object. Need to resolve.
                ////Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[collection.Count], 0));
                ////Assert.Throws<InvalidCastException>(() => collection.CopyTo(new string[collection.Count], 0));
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
    }
}
