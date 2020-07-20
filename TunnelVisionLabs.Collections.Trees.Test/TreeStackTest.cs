// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using ICollection = System.Collections.ICollection;

    public class TreeStackTest
    {
        [Fact]
        public void TestTreeStackConstructor()
        {
            TreeStack<int> stack = new TreeStack<int>();
            Assert.Empty(stack);
        }

        [Fact]
        public void TestTreeStackBranchingFactorConstructor()
        {
            TreeStack<int> stack = new TreeStack<int>(8);
            Assert.Empty(stack);

            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeStack<int>(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeStack<int>(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new TreeStack<int>(1));
        }

        [Fact]
        public void TestICollectionInterface()
        {
            TestICollectionInterfaceImpl(CreateTreeStack(new int[] { 600, 601 }), isOwnSyncRoot: true, supportsNullValues: false);
            TestICollectionInterfaceImpl(CreateTreeStack(new int?[] { 600, 601 }), isOwnSyncRoot: true, supportsNullValues: true);
            TestICollectionInterfaceImpl(CreateTreeStack(new object[] { 600, 601 }), isOwnSyncRoot: true, supportsNullValues: true);

            // Run the same set of tests on Stack<T> to ensure consistent behavior
            TestICollectionInterfaceImpl(CreateStack(new int[] { 600, 601 }), isOwnSyncRoot: false, supportsNullValues: false);
            TestICollectionInterfaceImpl(CreateStack(new int?[] { 600, 601 }), isOwnSyncRoot: false, supportsNullValues: true);
            TestICollectionInterfaceImpl(CreateStack(new object[] { 600, 601 }), isOwnSyncRoot: false, supportsNullValues: true);
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
                Assert.Equal(601, copy[0]);
                Assert.Equal(600, copy[1]);

                copy = new object[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Null(copy[0]);
                Assert.Equal(601, copy[1]);
                Assert.Equal(600, copy[2]);
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
                Assert.Equal(601, copy[0]);
                Assert.Equal(600, copy[1]);

                copy = new int[collection.Count + 2];
                collection.CopyTo(copy, 1);
                Assert.Equal(0, copy[0]);
                Assert.Equal(601, copy[1]);
                Assert.Equal(600, copy[2]);
                Assert.Equal(0, copy[3]);

                Assert.Throws<ArgumentException>(() => collection.CopyTo(new string[collection.Count], 0));
            }
        }

        [Fact]
        public void TestCopyToValidation()
        {
            TreeStack<int> stack = CreateTreeStack(Enumerable.Range(0, 10));
            Assert.Throws<ArgumentNullException>("dest", () => stack.CopyTo(null!, 0));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => stack.CopyTo(new int[stack.Count], -1));
            Assert.Throws<ArgumentException>(string.Empty, () => stack.CopyTo(new int[stack.Count], 1));

            ICollection collection = stack;
            Assert.Throws<ArgumentNullException>("dest", () => collection.CopyTo(null!, 0));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => collection.CopyTo(new int[collection.Count], -1));
            Assert.Throws<ArgumentOutOfRangeException>("dstIndex", () => collection.CopyTo(Array.CreateInstance(typeof(int), new[] { stack.Count }, new[] { 1 }), 0));
            Assert.Throws<ArgumentException>(string.Empty, () => collection.CopyTo(new int[collection.Count], collection.Count + 1));
            Assert.Throws<ArgumentException>(null, () => collection.CopyTo(new int[stack.Count, 1], 0));
            collection.CopyTo(Array.CreateInstance(typeof(int), new[] { stack.Count }, new[] { 1 }), 1);
        }

        [Fact]
        public void TestPush()
        {
            const int Value = 600;

            TreeStack<int> stack = new TreeStack<int>();
            Assert.Empty(stack);
            stack.Push(Value);
            Assert.Single(stack);
            Assert.Equal(Value, stack.Peek());
            int[] expected = { Value };
            int[] actual = stack.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestClear()
        {
            var stack = new TreeStack<int>(branchingFactor: 3);

            stack.Clear();
            Assert.Empty(stack);

            foreach (int item in Enumerable.Range(0, 10))
                stack.Push(item);

            Assert.NotEmpty(stack);
            stack.Clear();
            Assert.Empty(stack);
        }

        [Fact]
        public void TestContains()
        {
            Random random = new Random();
            var stack = new TreeStack<int>(branchingFactor: 4);
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int value = random.Next(stack.Count + 1);
                stack.Push(i);

                // Use stack.Contains(i) since this is a targeted collection API test
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection
                Assert.True(stack.Contains(i));
#pragma warning restore xUnit2017 // Do not use Contains() to check if a value exists in a collection
            }

            stack.Validate(ValidationRules.None);
        }

        [Fact]
        public void TestEmptyStack()
        {
            var stack = new TreeStack<int>();
            Assert.Throws<InvalidOperationException>(() => stack.Peek());
            Assert.Throws<InvalidOperationException>(() => stack.Pop());

            Assert.False(stack.TryPeek(out var result));
            Assert.Equal(0, result);

            Assert.False(stack.TryPop(out result));
            Assert.Equal(0, result);
        }

        [Fact]
        public void TestTrimExcess()
        {
            Random random = new Random();
            TreeStack<int> stack = new TreeStack<int>(branchingFactor: 4);
            Stack<int> reference = new Stack<int>();
            for (int i = 0; i < (2 * 4 * 4) + 2; i++)
            {
                stack.Push(i);
                reference.Push(i);
            }

            for (int i = 0; i < 2; i++)
            {
                stack.Pop();
                reference.Pop();
            }

            stack.Validate(ValidationRules.None);

            // In the first call to TrimExcess, items will move
            stack.TrimExcess();
            stack.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, stack);

            // In the second call, the list is already packed so nothing will move
            stack.TrimExcess();
            stack.Validate(ValidationRules.RequirePacked);
            Assert.Equal(reference, stack);

            TreeStack<int> empty = new TreeStack<int>();
            empty.Validate(ValidationRules.RequirePacked);
            empty.TrimExcess();
            empty.Validate(ValidationRules.RequirePacked);

            TreeStack<int> single = CreateTreeStack(Enumerable.Range(0, 1));
            single.Validate(ValidationRules.RequirePacked);
            single.TrimExcess();
            single.Validate(ValidationRules.RequirePacked);
        }

        [Fact]
        public void TestStackLikeBehavior()
        {
            Random random = new Random();
            TreeStack<int> stack = new TreeStack<int>(branchingFactor: 4);
            Stack<int> reference = new Stack<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = random.Next();
                stack.Push(item);
                reference.Push(item);
            }

            while (stack.Count > 0)
            {
                var expected = reference.Peek();
                Assert.Equal(expected, stack.Peek());
                Assert.Equal(expected, reference.Pop());
                Assert.Equal(expected, stack.Pop());
                stack.Validate(ValidationRules.None);

                Assert.Equal(reference, stack);
            }

            Assert.Empty(stack);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestEnumerator()
        {
            var stack = new TreeStack<int>();
            TreeStack<int>.Enumerator enumerator = stack.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the stack invalidates it, but Current is still unchecked
            stack.Push(1);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            enumerator = stack.GetEnumerator();
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
            var stack = new TreeStack<int>();
            IEnumerator<int> enumerator = stack.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the stack invalidates it, but Current is still unchecked
            stack.Push(1);
            Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
            Assert.Equal(0, enumerator.Current);

            enumerator = stack.GetEnumerator();
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

        private static TreeStack<T> CreateTreeStack<T>(IEnumerable<T> source)
        {
            var result = new TreeStack<T>();
            foreach (T item in source)
            {
                result.Push(item);
            }

            return result;
        }

        private static Stack<T> CreateStack<T>(IEnumerable<T> source)
        {
            var result = new Stack<T>();
            foreach (T item in source)
            {
                result.Push(item);
            }

            return result;
        }
    }
}
