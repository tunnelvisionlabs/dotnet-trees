// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
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

            internal abstract T this[int index]
            {
                get;
                set;
            }

            internal static Node Insert(Node root, int branchingFactor, int index, T item)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
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

            internal static Node RemoveAt(Node root, int index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index");
                if (index >= root.Count)
                    throw new ArgumentException();

                throw new NotImplementedException();
            }

            internal abstract int IndexOf(T item);

            internal abstract Node Insert(int branchingFactor, bool isAppend, int index, T item);

            private sealed class EmptyNode : Node
            {
                internal override int Count
                {
                    get
                    {
                        return 0;
                    }
                }

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

                internal override int IndexOf(T item)
                {
                    return -1;
                }

                internal override Node Insert(int branchingFactor, bool isAppend, int index, T item)
                {
                    Debug.Assert(index == 0 && isAppend, "index == 0 && isAppend");
                    LeafNode node = new LeafNode(branchingFactor);
                    node.Insert(branchingFactor, isAppend, index, item);
                    return node;
                }
            }
        }
    }
}
