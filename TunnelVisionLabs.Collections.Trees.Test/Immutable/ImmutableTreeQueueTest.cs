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

    public class ImmutableTreeQueueTest
    {
        [Fact]
        public void TestEmptyImmutableTreeQueue()
        {
            var queue = ImmutableTreeQueue.Create<int>();
            Assert.Same(ImmutableTreeQueue<int>.Empty, queue);
            Assert.Empty(queue);
        }

        [Fact]
        public void TestSingleElementQueue()
        {
            var value = Generator.GetInt32();
            var queue = ImmutableTreeQueue.Create(value);
            Assert.Equal(new[] { value }, queue);
        }

        [Fact]
        public void TestMultipleElementQueue()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var queue = ImmutableTreeQueue.Create(values);
            Assert.Equal(values, queue);
        }

        [Fact]
        public void TestImmutableTreeQueueCreateValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableTreeQueue.Create<int>(null));
        }

        [Fact]
        public void TestImmutableTreeQueueCreateRange()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var queue = ImmutableTreeQueue.CreateRange(values);
            Assert.Equal(values, queue);
        }

        [Fact]
        public void TestImmutableTreeQueueCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableTreeQueue.CreateRange<int>(null));
        }

        [Fact]
        public void TestEnqueue()
        {
            int value = Generator.GetInt32();

            var queue = ImmutableTreeQueue.Create<int>();
            Assert.Empty(queue);

            // Enqueue doesn't change the original queue
            queue.Enqueue(value);
            Assert.Empty(queue);

            queue = queue.Enqueue(value);
            Assert.Single(queue);
            Assert.Equal(value, queue.Peek());
            int[] expected = { value };
            int[] actual = queue.ToArray();
            Assert.Equal(expected, actual);

            // Test through the IImmutableQueue<T> interface
            IImmutableQueue<int> immutableQueue = queue;
            immutableQueue.Enqueue(Generator.GetInt32());
            Assert.Equal(expected, immutableQueue);

            int nextValue = Generator.GetInt32();
            immutableQueue = immutableQueue.Enqueue(nextValue);
            Assert.Equal(new[] { value, nextValue }, immutableQueue);
        }

        [Fact]
        public void TestEnqueueStaysPacked()
        {
            var queue = ImmutableTreeQueue.Create<int>();
            for (int i = 0; i < 4 * 8 * 8; i++)
                queue.Enqueue(i);

            queue.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestClear()
        {
            ImmutableTreeQueue<int> queue = ImmutableTreeQueue<int>.Empty;

            Assert.Same(queue, queue.Clear());

            foreach (int item in Enumerable.Range(0, 10))
                queue = queue.Enqueue(item);

            Assert.NotEmpty(queue);
            Assert.Same(ImmutableTreeQueue<int>.Empty, queue.Clear());

            // Test through the IImmutableQueue<T> interface
            IImmutableQueue<int> immutableQueue = queue;
            Assert.NotEmpty(immutableQueue);
            Assert.Same(ImmutableTreeQueue<int>.Empty, immutableQueue.Clear());
        }

        [Fact]
        public void TestEmptyQueue()
        {
            ImmutableTreeQueue<int> queue = ImmutableTreeQueue<int>.Empty;
            Assert.True(queue.IsEmpty);
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

        [Fact]
        public void TestQueueLikeBehavior()
        {
            var queue = ImmutableTreeQueue.Create<int>();
            var reference = new Queue<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = Generator.GetInt32();
                queue = queue.Enqueue(item);
                reference.Enqueue(item);
            }

            while (!queue.IsEmpty)
            {
                var expected = reference.Peek();
                Assert.Equal(expected, queue.Peek());
                Assert.Equal(expected, reference.Dequeue());

                IImmutableQueue<int> immutableQueue = queue;

                queue = queue.Dequeue(out int value);
                Assert.Equal(expected, value);
                queue.Validate(ValidationRules.None);

                Assert.Equal(reference, queue);

                // Test through the IImmutableQueue<T> interface (initialized above)
                immutableQueue = immutableQueue.Dequeue(out value);
                Assert.Equal(expected, value);
                Assert.Equal(reference, immutableQueue);
            }

            Assert.Empty(queue);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableTreeQueue<int> queue = ImmutableTreeQueue<int>.Empty;
            ImmutableTreeQueue<int>.Enumerator enumerator = queue.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the queue does not invalidate an enumerator
            CollectionAssert.EnumeratorNotInvalidated(queue, () => queue.Enqueue(1));

            queue = queue.Enqueue(1);
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
            var queue = ImmutableTreeQueue.Create<int>();
            IEnumerator<int> enumerator = queue.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the queue does not invalidate an enumerator
            CollectionAssert.EnumeratorNotInvalidated(queue, () => queue.Enqueue(1));

            queue = queue.Enqueue(1);
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
    }
}
