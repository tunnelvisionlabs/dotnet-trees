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
                throw new NotImplementedException();
            }

            internal static Node RemoveAt(Node root, int index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= root.Count)
                    throw new ArgumentException();

                throw new NotImplementedException();
            }

            internal static Node RemoveRange(Node root, int index, int count)
            {
                throw new NotImplementedException();
            }

            internal static Node RemoveAll(Node root, Predicate<T> match)
            {
                throw new NotImplementedException();
            }

            internal static Node TrimExcess(Node root)
            {
                throw new NotImplementedException();
            }

            internal abstract int IndexOf(T item, int index, int count);

            internal abstract Node Insert(int branchingFactor, bool isAppend, int index, T item);

            internal TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter)
            {
                Debug.Assert(NextNode == null, $"Assertion failed: {nameof(NextNode)} == null");

                return ConvertAll(converter, null);
            }

            internal abstract TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, TreeList<TOutput>.Node convertedNextNode);

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

                internal override Node Insert(int branchingFactor, bool isAppend, int index, T item)
                {
                    Debug.Assert(index == 0 && isAppend, "index == 0 && isAppend");
                    LeafNode node = new LeafNode(branchingFactor);
                    node.Insert(branchingFactor, isAppend, index, item);
                    return node;
                }

                internal override TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, TreeList<TOutput>.Node convertedNextNode)
                {
                    return TreeList<TOutput>.Node.Empty;
                }
            }
        }
    }
}
