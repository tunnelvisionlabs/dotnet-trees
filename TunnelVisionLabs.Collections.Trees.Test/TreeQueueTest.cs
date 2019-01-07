// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using ICollection = System.Collections.ICollection;

    public class TreeQueueTest
    {
        [Fact]
        public void TestTreeQueueConstructor()
        {
            TreeQueue<int> queue = new TreeQueue<int>();
            Assert.Empty(queue);
        }

        [Fact]
        public void TestTreeQueueBranchingFactorConstructor()
        {
            TreeQueue<int> queue = new TreeQueue<int>(8);
            Assert.Empty(queue);

            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeQueue<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeQueue<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeQueue<int>(1));
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(CreateTreeQueue(new int[] { 600, 601 }), isOwnSyncRoot: true, supportsNullValues: false);
            TestICollectionInterfaceImpl(CreateTreeQueue(new int?[] { 600, 601 }), isOwnSyncRoot: true, supportsNullValues: true);
            TestICollectionInterfaceImpl(CreateTreeQueue(new object[] { 600, 601 }), isOwnSyncRoot: true, supportsNullValues: true);

            // Run the same set of tests on Queue<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(CreateQueue(new int[] { 600, 601 }), isOwnSyncRoot: false, supportsNullValues: false);
            TestICollectionInterfaceImpl(CreateQueue(new int?[] { 600, 601 }), isOwnSyncRoot: false, supportsNullValues: true);
            TestICollectionInterfaceImpl(CreateQueue(new object[] { 600, 601 }), isOwnSyncRoot: false, supportsNullValues: true);
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
        public void TestCopyToValidation()
        {
            TreeQueue<int> queue = CreateTreeQueue(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("dest", () => queue.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => queue.CopyTo(new int[queue.Count], -1));
            Assert.Throws<ArgumentException>(string.Empty, () => queue.CopyTo(new int[queue.Count], 1));

            ICollection collection = queue;
            Assert.Throws<ArgumentNullException>("dest", () => collection.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => collection.CopyTo(new int[collection.Count], -1));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => collection.CopyTo(Array.CreateInstance(typeof(int), new[] { queue.Count }, new[] { 1 }), 0));
            Assert.Throws<ArgumentException>(string.Empty, () => collection.CopyTo(new int[collection.Count], collection.Count + 1));
            Assert.Throws<ArgumentException>(null, () => collection.CopyTo(new int[queue.Count, 1], 0));
            collection.CopyTo(Array.CreateInstance(typeof(int), new[] { queue.Count }, new[] { 1 }), 1);
        }

        [Fact]
        public void TestEnqueue()
        {
            const int Value = 600;

            TreeQueue<int> queue = new TreeQueue<int>();
            Assert.Empty(queue);
            queue.Enqueue(Value);
            Assert.Single(queue);
            Assert.Equal(Value, queue.Peek());
            int[] expected = { Value };
            int[] actual = queue.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestEnqueueStaysPacked()
        {
            TreeQueue<int> queue = new TreeQueue<int>(branchingFactor: 4);
            for (int i = 0; i < 2 * 4 * 4; i++)
                queue.Enqueue(i);

            queue.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            TreeQueue<int> queue = new TreeQueue<int>(branchingFactor: 4);
            Queue<int> reference = new Queue<int>();
            for (int i = 0; i < (2 * 4 * 4) + 2; i++)
            {
                queue.Enqueue(i);
                reference.Enqueue(i);
            }

            for (int i = 0; i < 2; i++)
            {
                queue.Dequeue();
                reference.Dequeue();
            }

            queue.Validate(ValidationRules.None);

            // In the first call to TrimExcess, items will move
            queue.TrimExcess();
            queue.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, queue);

            // In the second call, the list is already packed so nothing will move
            queue.TrimExcess();
            queue.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, queue);

            TreeQueue<int> empty = new TreeQueue<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            TreeQueue<int> single = CreateTreeQueue(Enumerable.Range(0, 1));
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestQueueLikeBehavior()
        {
            Random random = new Random();
            TreeQueue<int> queue = new TreeQueue<int>(branchingFactor: 4);
            Queue<int> reference = new Queue<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next();
                queue.Enqueue(item);
                reference.Enqueue(item);
            }

            while (queue.Count > 0)
            {
                var expected = reference.Peek();
                Assert.Equal(expected, queue.Peek());
                Assert.Equal(expected, reference.Dequeue());
                Assert.Equal(expected, queue.Dequeue());
                queue.Validate(ValidationRules.None);

                Assert.Equal(reference, queue);
            }

            Assert.Empty(queue);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestEnumerator()
        {
            var queue = new TreeQueue<int>();
            TreeQueue<int>.Enumerator enumerator = queue.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the queue invalidates it, but Current is still unchecked
            queue.Enqueue(1);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            enumerator = queue.GetEnumerator();
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
            var queue = new TreeQueue<int>();
            IEnumerator<int> enumerator = queue.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the queue invalidates it, but Current is still unchecked
            queue.Enqueue(1);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            Assert.Equal(0, enumerator.Current);

            enumerator = queue.GetEnumerator();
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

        private static TreeQueue<T> CreateTreeQueue<T>(IEnumerable<T> source)
        {
            var result = new TreeQueue<T>();
            foreach (T item in source)
            {
                result.Enqueue(item);
            }

            return result;
        }

        private static Queue<T> CreateQueue<T>(IEnumerable<T> source)
        {
            var result = new Queue<T>();
            foreach (T item in source)
            {
                result.Enqueue(item);
            }

            return result;
        }
    }
}
