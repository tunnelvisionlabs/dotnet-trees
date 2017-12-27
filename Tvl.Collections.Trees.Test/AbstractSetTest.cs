// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees.Test
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
        public void TestExceptWith()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.ExceptWith(null));

            // Return without iterating if the set is already empty
            set.ExceptWith(EverythingThrowsEnumerable<int>.Instance);

            // Test ExceptWith self
            set.Add(1);
            set.ExceptWith(set);
            Assert.Empty(set);

            // Test ExceptWith subset
            set.UnionWith(Enumerable.Range(0, 10));
            set.ExceptWith(new[] { 1, 3, 5, 7, 9 });
            Assert.Equal(new[] { 0, 2, 4, 6, 8 }, set);
            set.ExceptWith(new[] { 0, 2, 4, 6, 8 });
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
            set.IntersectWith(set);
            Assert.Equal(new[] { 1 }, set);

            // Test IntersectWith array
            set.UnionWith(Enumerable.Range(0, 10));
            set.IntersectWith(new[] { 1, 3, 11, 5, 7, 9 });
            Assert.Equal(new[] { 1, 3, 5, 7, 9 }, set);

            // Test IntersectWith same set type
            ISet<int> other = CreateSet<int>();
            other.UnionWith(Enumerable.Range(3, 5));
            set.IntersectWith(other);
            Assert.Equal(new[] { 3, 5, 7 }, set);
        }

        [Fact]
        public void TestSymmetricExceptWith()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.SymmetricExceptWith(null));

            // Test behavior when the current set is empty
            set.SymmetricExceptWith(new[] { 1, 5, 3 });
            Assert.Equal(new[] { 1, 3, 5 }, set);

            // Test SymmetricExceptWith self
            Assert.NotEmpty(set);
            set.SymmetricExceptWith(set);
            Assert.Empty(set);
            set.SymmetricExceptWith(set);
            Assert.Empty(set);

            // Test SymmetricExceptWith same set type
            ISet<int> other = CreateSet<int>();
            set.UnionWith(new[] { 1, 3, 5 });
            other.UnionWith(new[] { 3, 5, 7 });
            set.SymmetricExceptWith(other);
            Assert.Equal(new[] { 1, 7 }, set);

            // Test SymmetricExceptWith same set type
            set.Clear();
            other = CreateSet<int>();
            set.UnionWith(new[] { 1, 3, 5 });
            other.UnionWith(new[] { 3, 5, 7 });
            set.SymmetricExceptWith(other.ToArray());
            Assert.Equal(new[] { 1, 7 }, set);
        }

        [Fact]
        public void TestIsProperSubsetOf()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsProperSubsetOf(null));

            // Test behavior when the current set is empty
            Assert.False(set.IsProperSubsetOf(Enumerable.Empty<int>()));
            Assert.True(set.IsProperSubsetOf(Enumerable.Range(0, 1)));

            // Test IsProperSubsetOf self
            set.Add(1);
            Assert.False(set.IsProperSubsetOf(set));
            Assert.False(set.IsProperSubsetOf(set.ToArray()));

            // Test IsProperSubsetOf array
            set.UnionWith(new[] { 3, 5, 7 });
            Assert.True(set.IsProperSubsetOf(new[] { 1, 3, 5, 7, 9 }));
            Assert.False(set.IsProperSubsetOf(new[] { 1, 3, 7, 9 }));
            Assert.True(set.IsProperSubsetOf(Enumerable.Range(0, 10)));

            // Test IsProperSubsetOf same set type
            set.Clear();
            set.UnionWith(new[] { 3, 5 });
            ISet<int> other = CreateSet<int>();
            other.UnionWith(Enumerable.Range(3, 5));
            Assert.True(set.IsProperSubsetOf(other));
            Assert.False(other.IsProperSubsetOf(set));

            set.Remove(5);
            set.Add(8);
            Assert.True(set.Count < other.Count);
            Assert.False(set.IsProperSubsetOf(other));
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
            Assert.False(set.IsProperSupersetOf(set));
            Assert.False(set.IsProperSupersetOf(set.ToArray()));

            // Test IsProperSupersetOf empty
            Assert.NotEmpty(set);
            Assert.True(set.IsProperSupersetOf(new int[0]));

            // Test IsProperSupersetOf array
            set.UnionWith(Enumerable.Range(0, 10));
            Assert.True(set.IsProperSupersetOf(new[] { 1, 3, 5, 7, 9 }));
            Assert.False(set.IsProperSupersetOf(new[] { 1, 3, 11, 5, 7, 9 }));
            Assert.False(set.IsProperSupersetOf(Enumerable.Range(0, 10)));

            // Test IsProperSupersetOf same set type
            set.Clear();
            set.UnionWith(Enumerable.Range(3, 5));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(new[] { 3, 5 });
            Assert.True(set.IsProperSupersetOf(other));
            Assert.False(other.IsProperSupersetOf(set));

            other.Remove(5);
            other.Add(8);
            Assert.True(set.Count > other.Count);
            Assert.False(set.IsProperSupersetOf(other));
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
            Assert.True(set.IsSubsetOf(set));
            Assert.True(set.IsSubsetOf(set.ToArray()));

            // Test IsSubsetOf array
            set.UnionWith(new[] { 3, 5, 7 });
            Assert.True(set.IsSubsetOf(new[] { 1, 3, 5, 7, 9 }));
            Assert.False(set.IsSubsetOf(new[] { 1, 3, 7, 9 }));
            Assert.True(set.IsSubsetOf(Enumerable.Range(0, 10)));

            // Test IsSubsetOf same set type
            set.Clear();
            set.UnionWith(new[] { 3, 5 });
            ISet<int> other = CreateSet<int>();
            other.UnionWith(Enumerable.Range(3, 5));
            Assert.True(set.IsSubsetOf(other));
            Assert.False(other.IsSubsetOf(set));

            set.Remove(5);
            set.Add(8);
            Assert.True(set.Count < other.Count);
            Assert.False(set.IsSubsetOf(other));
        }

        [Fact]
        public void TestIsSupersetOf()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.IsSupersetOf(null));

            // Test IsSupersetOf self
            set.Add(1);
            Assert.True(set.IsSupersetOf(set));
            Assert.True(set.IsSupersetOf(set.ToArray()));

            // Test IsSupersetOf empty
            Assert.NotEmpty(set);
            Assert.True(set.IsSupersetOf(new int[0]));

            // Test IsSupersetOf array
            set.UnionWith(Enumerable.Range(0, 10));
            Assert.True(set.IsSupersetOf(new[] { 1, 3, 5, 7, 9 }));
            Assert.False(set.IsSupersetOf(new[] { 1, 3, 11, 5, 7, 9 }));
            Assert.True(set.IsSupersetOf(Enumerable.Range(0, 10)));

            // Test IsSupersetOf same set type
            set.Clear();
            set.UnionWith(Enumerable.Range(3, 5));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(new[] { 3, 5 });
            Assert.True(set.IsSupersetOf(other));
            Assert.False(other.IsSupersetOf(set));

            other.Remove(5);
            other.Add(8);
            Assert.True(set.Count > other.Count);
            Assert.False(set.IsSupersetOf(other));
        }

        [Fact]
        public void TestOverlaps()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.Overlaps(null));

            // Return without iterating if the set is already empty
            Assert.False(set.Overlaps(EverythingThrowsEnumerable<int>.Instance));

            set.UnionWith(Enumerable.Range(0, 10));
            Assert.False(set.Overlaps(Enumerable.Empty<int>()));
            Assert.False(set.Overlaps(Enumerable.Range(-2, 2)));
            Assert.False(set.Overlaps(Enumerable.Range(10, 2)));
            Assert.True(set.Overlaps(Enumerable.Range(-2, 3)));
            Assert.True(set.Overlaps(Enumerable.Range(9, 3)));
            Assert.True(set.Overlaps(Enumerable.Range(3, 1)));
            Assert.True(set.Overlaps(Enumerable.Range(0, 1)));
            Assert.True(set.Overlaps(Enumerable.Range(9, 1)));
        }

        [Fact]
        public void TestSetEquals()
        {
            ISet<int> set = CreateSet<int>();
            Assert.Throws<ArgumentNullException>(() => set.SetEquals(null));

            // Test behavior when the current set is empty
            Assert.True(set.SetEquals(Enumerable.Empty<int>()));
            Assert.False(set.SetEquals(Enumerable.Range(0, 1)));

            // Test SetEquals self
            Assert.True(set.SetEquals(set));

            // Test with same set type
            set.UnionWith(Enumerable.Range(0, 10));
            ISet<int> other = CreateSet<int>();
            other.UnionWith(Enumerable.Range(0, 10));
            Assert.True(set.SetEquals(other));

            other.Remove(0);
            Assert.False(set.SetEquals(other));
            other.Add(-1);
            Assert.False(set.SetEquals(other));
            other.Remove(-1);
            other.Add(0);
            other.Remove(8);
            Assert.False(set.SetEquals(other));
            other.Add(11);
            Assert.False(set.SetEquals(other));

            // Test with different set type
            set.Clear();
            other.Clear();
            set.UnionWith(Enumerable.Range(0, 10));
            other.UnionWith(Enumerable.Range(0, 10));
            Assert.True(set.SetEquals(other.ToArray()));
            Assert.True(set.SetEquals(other.Concat(other).ToArray()));

            other.Remove(0);
            Assert.False(set.SetEquals(other.ToArray()));
            other.Add(-1);
            Assert.False(set.SetEquals(other.ToArray()));
            other.Remove(-1);
            other.Add(0);
            other.Remove(8);
            Assert.False(set.SetEquals(other.ToArray()));
            other.Add(11);
            Assert.False(set.SetEquals(other.ToArray()));
        }

        protected abstract ISet<T> CreateSet<T>();

        protected static void TestICollectionInterfaceImpl(ICollection collection, bool supportsNullValues)
        {
            Assert.False(collection.IsSynchronized);

            Assert.IsType<object>(collection.SyncRoot);
            Assert.Same(collection.SyncRoot, collection.SyncRoot);

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

        private sealed class EverythingThrowsEnumerable<T> : IEnumerable<T>
        {
            public static readonly EverythingThrowsEnumerable<T> Instance = new EverythingThrowsEnumerable<T>();

            [ExcludeFromCodeCoverage]
            public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

            [ExcludeFromCodeCoverage]
            IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
        }
    }
}
