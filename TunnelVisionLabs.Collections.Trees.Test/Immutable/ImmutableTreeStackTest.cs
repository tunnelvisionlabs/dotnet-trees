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

    public class ImmutableTreeStackTest
    {
        [Fact]
        public void TestEmptyImmutableTreeStack()
        {
            var queue = ImmutableTreeStack.Create<int>();
            Assert.Same(ImmutableTreeStack<int>.Empty, queue);
            Assert.Empty(queue);
        }

        [Fact]
        public void TestSingleElementStack()
        {
            var value = Generator.GetInt32();
            var queue = ImmutableTreeStack.Create(value);
            Assert.Equal(new[] { value }, queue);
        }

        [Fact]
        public void TestMultipleElementStack()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var queue = ImmutableTreeStack.Create(values);
            Assert.Equal(values.Reverse(), queue);
        }

        [Fact]
        public void TestImmutableTreeStackCreateValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableTreeStack.Create<int>(null!));
        }

        [Fact]
        public void TestImmutableTreeStackCreateRange()
        {
            var values = new[] { Generator.GetInt32(), Generator.GetInt32(), Generator.GetInt32() };
            var queue = ImmutableTreeStack.CreateRange(values);
            Assert.Equal(values.Reverse(), queue);
        }

        [Fact]
        public void TestImmutableTreeStackCreateRangeValidation()
        {
            Assert.Throws<ArgumentNullException>("items", () => ImmutableTreeStack.CreateRange<int>(null!));
        }

        [Fact]
        public void TestPush()
        {
            int value = Generator.GetInt32();

            var stack = ImmutableTreeStack.Create<int>();
            Assert.Empty(stack);

            // Push doesn't change the original stack
            stack.Push(value);
            Assert.Empty(stack);

            stack = stack.Push(value);
            Assert.Single(stack);
            Assert.Equal(value, stack.Peek());
            int[] expected = { value };
            int[] actual = stack.ToArray();
            Assert.Equal(expected, actual);

            // Test through the IImmutableStack<T> interface
            IImmutableStack<int> immutableStack = stack;
            immutableStack.Push(Generator.GetInt32());
            Assert.Equal(expected, immutableStack);

            int nextValue = Generator.GetInt32();
            immutableStack = immutableStack.Push(nextValue);
            Assert.Equal(new[] { nextValue, value }, immutableStack);
        }

        [Fact]
        public void TestClear()
        {
            ImmutableTreeStack<int> stack = ImmutableTreeStack<int>.Empty;

            Assert.Same(stack, stack.Clear());

            foreach (int item in Enumerable.Range(0, 10))
                stack = stack.Push(item);

            Assert.NotEmpty(stack);
            Assert.Same(ImmutableTreeStack<int>.Empty, stack.Clear());

            // Test through the IImmutableStack<T> interface
            IImmutableStack<int> immutableStack = stack;
            Assert.NotEmpty(immutableStack);
            Assert.Same(ImmutableTreeStack<int>.Empty, immutableStack.Clear());
        }

        [Fact]
        public void TestEmptyStack()
        {
            ImmutableTreeStack<int> stack = ImmutableTreeStack<int>.Empty;
            Assert.True(stack.IsEmpty);
            Assert.Throws<InvalidOperationException>(() => stack.Peek());
            Assert.Throws<InvalidOperationException>(() => stack.Pop());
        }

        [Fact]
        public void TestStackLikeBehavior()
        {
            var stack = ImmutableTreeStack.Create<int>();
            var reference = new Stack<int>();
            for (int i = 0; i < 2 * 4 * 4; i++)
            {
                int item = Generator.GetInt32();
                stack = stack.Push(item);
                reference.Push(item);
            }

            while (!stack.IsEmpty)
            {
                var expected = reference.Peek();
                Assert.Equal(expected, stack.Peek());
                Assert.Equal(expected, reference.Pop());

                IImmutableStack<int> immutableStack = stack;

                stack = stack.Pop(out int value);
                Assert.Equal(expected, value);
                stack.Validate(ValidationRules.None);

                Assert.Equal(reference, stack);

                // Test through the IImmutableStack<T> interface (initialized above)
                immutableStack = immutableStack.Pop(out value);
                Assert.Equal(expected, value);
                Assert.Equal(reference, immutableStack);
            }

            Assert.Empty(stack);
            Assert.Empty(reference);
        }

        [Fact]
        public void TestEnumerator()
        {
            ImmutableTreeStack<int> stack = ImmutableTreeStack<int>.Empty;
            ImmutableTreeStack<int>.Enumerator enumerator = stack.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the stack does not invalidate an enumerator
            CollectionAssert.EnumeratorNotInvalidated(stack, () => stack.Push(1));

            stack = stack.Push(1);
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
            var stack = ImmutableTreeStack.Create<int>();
            IEnumerator<int> enumerator = stack.GetEnumerator();
            Assert.Equal(0, enumerator.Current);
            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);

            // Adding an item to the stack does not invalidate an enumerator
            CollectionAssert.EnumeratorNotInvalidated(stack, () => stack.Push(1));

            stack = stack.Push(1);
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
    }
}
