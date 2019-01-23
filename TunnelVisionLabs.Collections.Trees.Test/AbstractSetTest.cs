// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Xunit;

    public abstract class AbstractSetTest
    {
        [Fact]
        public void TestUnionWith()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.UnionWith(null));

            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 7)));
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(5, 5)));
            Assert.Equal(Enumerable.Range(0, 10), set);
        }

        [Fact]
        public void TestExceptWith()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.ExceptWith(null));

            // Return without iterating if the set is already empty
            set.ExceptWith(EverythingThrowsEnumerable<int>.Instance);

            // Test ExceptWith self
            set.Add(1);
            set.ExceptWith(TransformEnumerableForSetOperation(set));
            Assert.Empty(set);

            // Test ExceptWith subset
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            set.ExceptWith(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 }));
            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, set);
            set.ExceptWith(TransformEnumerableForSetOperation(new[] { 0, 2, 4, 6, 8 }));
            Assert.Empty(set);
        }

        [Fact]
        public void TestIntersectWith()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IntersectWith(null));

            // Return without iterating if the set is already empty
            set.IntersectWith(EverythingThrowsEnumerable<int>.Instance);

            // Test IntersectWith self
            set.Add(1);
            set.IntersectWith(TransformEnumerableForSetOperation(set));
            Assert.Equal(new[] { 1 }, set);

            // Test IntersectWith array
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            set.IntersectWith(TransformEnumerableForSetOperation(new[] { 1, 3, 11, 5, 7, 9 }));
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, set);

            // Test IntersectWith same set type
            ISet<int> other = CreateSet<int>();
            other.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            set.IntersectWith(TransformEnumerableForSetOperation(other));
            Assert.Equal(new[] { 3, 5, 7 }, set);
        }

        [Fact]
        public void TestSymmetricExceptWith()
        {
            ISet<int> set = CreateSet<int>();
            ISet<int> second = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.SymmetricExceptWith(null));

            // Test behavior when the current set is empty
            set.SymmetricExceptWith(TransformEnumerableForSetOperation(new[] { 1, 5, 3 }));
            second.UnionWith(TransformEnumerableForSetOperation(new[] { 1, 5, 3 }));
            Assert.Equal(second.ToArray(), set);

            // Test SymmetricExceptWith self
            Assert.NotEmpty(set);
            set.SymmetricExceptWith(TransformEnumerableForSetOperation(set));
            Assert.Empty(set);
            set.SymmetricExceptWith(TransformEnumerableForSetOperation(set));
            Assert.Empty(set);

            // Test SymmetricExceptWith same set type
            ISet<int> other = CreateSet<int>();
            set.UnionWith(TransformEnumerableForSetOperation(new[] { 1, 3, 5 }));
            other.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5, 7 }));
            set.SymmetricExceptWith(TransformEnumerableForSetOperation(other));
            Assert.Equal(new[] { 1, 7 }, set);

            // Test SymmetricExceptWith same set type
            set.Clear();
            other = CreateSet<int>();
            set.UnionWith(TransformEnumerableForSetOperation(new[] { 1, 3, 5 }));
            other.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5, 7 }));
            set.SymmetricExceptWith(TransformEnumerableForSetOperation(other.ToArray()));
            Assert.Equal(new[] { 1, 7 }, set);
        }

        [Fact]
        public void TestIsProperSubsetOf()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsProperSubsetOf(null));

            // Test behavior when the current set is empty
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(Enumerable.Empty<int>())));
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 1))));

            // Test IsProperSubsetOf self
            set.Add(1);
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(set)));
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsProperSubsetOf array
            set.UnionWith(new[] { 3, 5, 7 });
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 7, 9 })));
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsProperSubsetOf same set type
            set.Clear();
            set.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            Assert.True(set.IsProperSubsetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsProperSubsetOf(TransformEnumerableForSetOperation(set)));

            set.Remove(5);
            set.Add(8);
            Assert.True(set.Count < other.Count);
            Assert.False(set.IsProperSubsetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestIsProperSupersetOf()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsProperSupersetOf(null));

            // Return without iterating if the set is already empty
            Assert.False(set.IsProperSupersetOf(EverythingThrowsEnumerable<int>.Instance));

            // Test IsProperSupersetOf self
            set.Add(1);
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(set)));
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsProperSupersetOf empty
            Assert.NotEmpty(set);
            Assert.True(set.IsProperSupersetOf(TransformEnumerableForSetOperation(new int[0])));

            // Test IsProperSupersetOf array
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.IsProperSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 11, 5, 7, 9 })));
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsProperSupersetOf same set type
            set.Clear();
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            Assert.True(set.IsProperSupersetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsProperSupersetOf(TransformEnumerableForSetOperation(set)));

            other.Remove(5);
            other.Add(8);
            Assert.True(set.Count > other.Count);
            Assert.False(set.IsProperSupersetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestIsSubsetOf()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsSubsetOf(null));

            // Return without iterating if the set is already empty
            Assert.True(set.IsSubsetOf(EverythingThrowsEnumerable<int>.Instance));

            // Test IsSubsetOf self
            set.Add(1);
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(set)));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsSubsetOf array
            set.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5, 7 }));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsSubsetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 7, 9 })));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsSubsetOf same set type
            set.Clear();
            set.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            Assert.True(set.IsSubsetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsSubsetOf(TransformEnumerableForSetOperation(set)));

            set.Remove(5);
            set.Add(8);
            Assert.True(set.Count < other.Count);
            Assert.False(set.IsSubsetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestIsSupersetOf()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsSupersetOf(null));

            // Test IsSupersetOf self
            set.Add(1);
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(set)));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(set.ToArray())));

            // Test IsSupersetOf empty
            Assert.NotEmpty(set);
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(new int[0])));

            // Test IsSupersetOf array
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 5, 7, 9 })));
            Assert.False(set.IsSupersetOf(TransformEnumerableForSetOperation(new[] { 1, 3, 11, 5, 7, 9 })));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(Enumerable.Range(0, 10))));

            // Test IsSupersetOf same set type
            set.Clear();
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(3, 5)));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(TransformEnumerableForSetOperation(new[] { 3, 5 }));
            Assert.True(set.IsSupersetOf(TransformEnumerableForSetOperation(other)));
            Assert.False(other.IsSupersetOf(TransformEnumerableForSetOperation(set)));

            other.Remove(5);
            other.Add(8);
            Assert.True(set.Count > other.Count);
            Assert.False(set.IsSupersetOf(TransformEnumerableForSetOperation(other)));
        }

        [Fact]
        public void TestOverlaps()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.Overlaps(null));

            // Return without iterating if the set is already empty
            Assert.False(set.Overlaps(EverythingThrowsEnumerable<int>.Instance));

            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
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
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.SetEquals(null));

            // Test behavior when the current set is empty
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(Enumerable.Empty<int>())));
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(Enumerable.Range(0, 1))));

            // Test SetEquals self
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(set)));

            // Test with same set type
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(other)));

            other.Remove(0);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));
            other.Add(-1);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));
            other.Remove(-1);
            other.Add(0);
            other.Remove(8);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));
            other.Add(11);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other)));

            // Test with different set type
            set.Clear();
            other.Clear();
            set.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            other.UnionWith(TransformEnumerableForSetOperation(Enumerable.Range(0, 10)));
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            Assert.True(set.SetEquals(TransformEnumerableForSetOperation(other.Concat(other).ToArray())));

            other.Remove(0);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            other.Add(-1);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            other.Remove(-1);
            other.Add(0);
            other.Remove(8);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
            other.Add(11);
            Assert.False(set.SetEquals(TransformEnumerableForSetOperation(other.ToArray())));
        }

        protected abstract ISet<T> CreateSet<T>();

        protected virtual IEnumerable<T> TransformEnumerableForSetOperation<T>(IEnumerable<T> enumerable)
            => enumerable;

        protected static void TestICollectionInterfaceImpl(ICollection collection, bool isOwnSyncRoot, bool supportsNullValues)
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

                Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null, 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(new object[1, collection.Count], 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(Array.CreateInstance(typeof(object), lengths: new[] { collection.Count }, lowerBounds: new[] { -1 }), 0));
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

                Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null, 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(new int[1, collection.Count], 0));
                Assert.Throws<ArgumentException>(() => collection.CopyTo(Array.CreateInstance(typeof(int), lengths: new[] { collection.Count }, lowerBounds: new[] { -1 }), 0));
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
    }
}
