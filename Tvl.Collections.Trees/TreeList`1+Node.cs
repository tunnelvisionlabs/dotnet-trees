// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class TreeList<T>
    {
        private abstract class Node
        {
            internal static readonly Node Empty = new EmptyNode();

            internal abstract int Count
            {
                get;
            }

            internal abstract LeafNode FirstLeaf
            {
                get;
            }

            internal abstract Node NextNode
            {
                get;
            }

            internal abstract Node FirstChild
            {
                get;
            }

            internal abstract T this[int index]
            {
                get;
                set;
            }

            internal static Node Insert(Node root, int branchingFactor, int index, T item)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index > root.Count)
                    throw new ArgumentException();

                Node splitNode = root.Insert(branchingFactor, index == root.Count, index, item);
                if (splitNode == null)
                    return root;

                if (root == Empty)
                {
                    // this was the first item
                    return splitNode;
                }
                else
                {
                    return new IndexNode(branchingFactor, root, splitNode);
                }
            }

            internal static Node InsertRange(Node root, int branchingFactor, int index, IEnumerable<T> collection)
            {
                if (collection == null)
                    throw new ArgumentNullException(nameof(collection));
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index > root.Count)
                    throw new ArgumentOutOfRangeException();

                // We can't insert a range into the empty node
                if (root == Empty)
                    root = new LeafNode(branchingFactor);

                Node splitNode = root.InsertRange(branchingFactor, index == root.Count, index, collection);
                while (splitNode != null)
                {
                    // Make a new level, walking nodes on the previous root level from 'node' to 'splitNode'
                    IndexNode newRoot = new IndexNode(branchingFactor, root, splitNode, out IndexNode newSplitNode);
                    root = newRoot;
                    splitNode = newSplitNode == newRoot ? null : newSplitNode;
                }

                return root;
            }

            internal static Node RemoveAt(Node root, int index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= root.Count)
                    throw new ArgumentOutOfRangeException();

                bool removedNode = root.RemoveAt(index);
                if (!removedNode)
                    return root;

                throw new NotImplementedException();
            }

            internal static Node RemoveRange(Node root, int index, int count)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));
                if (index > root.Count - count)
                    throw new ArgumentOutOfRangeException();

                if (count == root.Count)
                    return Empty;

                Node result = root;
                for (int i = 0; i < count; i++)
                {
                    result = RemoveAt(result, index + count - i - 1);
                }

                return result;
            }

            internal static Node RemoveAll(Node root, Predicate<T> match)
            {
                throw new NotImplementedException();
            }

            internal static Node TrimExcess(Node root)
            {
                throw new NotImplementedException();
            }

            internal void Reverse(int index, int count)
            {
                int firstIndex = index;
                int lastIndex = firstIndex + count - 1;
                while (lastIndex > firstIndex)
                {
                    T temp = this[firstIndex];
                    this[firstIndex] = this[lastIndex];
                    this[lastIndex] = temp;
                    firstIndex++;
                    lastIndex--;
                }
            }

            internal abstract int IndexOf(T item, int index, int count);

            internal abstract int LastIndexOf(T item, int index, int count);

            internal abstract Node Insert(int branchingFactor, bool isAppend, int index, T item);

            internal abstract Node InsertRange(int branchingFactor, bool isAppend, int index, IEnumerable<T> collection);

            internal abstract bool RemoveAt(int index);

            internal abstract void Sort(int index, int count, IComparer<T> comparer);

            internal abstract int FindIndex(int startIndex, int count, Predicate<T> match);

            internal abstract int BinarySearch(int index, int count, T item, IComparer<T> comparer);

            internal TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter)
            {
                Debug.Assert(NextNode == null, $"Assertion failed: {nameof(NextNode)} == null");

                return ConvertAll(converter, null);
            }

            internal abstract TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, TreeList<TOutput>.Node convertedNextNode);

            internal abstract void Validate(ValidationRules rules);

            private sealed class EmptyNode : Node
            {
                internal override int Count
                {
                    get
                    {
                        return 0;
                    }
                }

                internal override LeafNode FirstLeaf => null;

                internal override Node NextNode => null;

                internal override Node FirstChild => null;

                internal override T this[int index]
                {
                    get
                    {
                        throw new InvalidOperationException();
                    }

                    set
                    {
                        throw new InvalidOperationException();
                    }
                }

                internal override int IndexOf(T item, int index, int count)
                {
                    Debug.Assert(index == 0, $"Assertion failed: {nameof(index)} == 0");
                    Debug.Assert(count == 0, $"Assertion failed: {nameof(count)} == 0");

                    return -1;
                }

                internal override int LastIndexOf(T item, int index, int count)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                internal override Node Insert(int branchingFactor, bool isAppend, int index, T item)
                {
                    Debug.Assert(index == 0 && isAppend, "index == 0 && isAppend");
                    LeafNode node = new LeafNode(branchingFactor);
                    node.Insert(branchingFactor, isAppend, index, item);
                    return node;
                }

                internal override Node InsertRange(int branchingFactor, bool isAppend, int index, IEnumerable<T> collection)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                internal override bool RemoveAt(int index)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                internal override void Sort(int index, int count, IComparer<T> comparer)
                {
                    Debug.Assert(index == 0, $"Assertion failed: {nameof(index)} == 0");
                    Debug.Assert(count == 0, $"Assertion failed: {nameof(count)} == 0");
                    Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");
                }

                internal override int FindIndex(int startIndex, int count, Predicate<T> match)
                {
                    Debug.Assert(startIndex == 0, $"Assertion failed: {nameof(startIndex)} == 0");
                    Debug.Assert(count == 0, $"Assertion failed: {nameof(count)} == 0");
                    Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                    return -1;
                }

                internal override int BinarySearch(int index, int count, T item, IComparer<T> comparer)
                {
                    Debug.Assert(index == 0, $"Assertion failed: {nameof(index)} == 0");
                    Debug.Assert(count == 0, $"Assertion failed: {nameof(count)} == 0");
                    Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                    return ~0;
                }

                internal override TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, TreeList<TOutput>.Node convertedNextNode)
                {
                    return TreeList<TOutput>.Node.Empty;
                }

                internal override void Validate(ValidationRules rules)
                {
                    Debug.Assert(this == Empty, $"Assertion failed: this == {nameof(Empty)}");
                }
            }
        }
    }
}
