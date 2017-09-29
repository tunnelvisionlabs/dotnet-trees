// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Diagnostics;

    public partial class TreeList<T>
    {
        private sealed class LeafNode : Node
        {
            private readonly T[] _data;
            private LeafNode _next;
            private int _count;

            internal LeafNode(int branchingFactor)
            {
                _data = new T[branchingFactor];
            }

            internal override int Count => _count;

            internal override LeafNode FirstLeaf => this;

            internal LeafNode Next => _next;

            internal override T this[int index]
            {
                get
                {
                    return _data[index];
                }

                set
                {
                    _data[index] = value;
                }
            }

            internal override int IndexOf(T item, int index, int count)
            {
                Debug.Assert(index >= 0, $"Assertion failed: {nameof(index)} >= 0");
                Debug.Assert(count >= 0 && index <= Count - count, $"Assertion failed: {nameof(count)} >= 0 && {nameof(index)} <= {nameof(Count)} - {nameof(count)}");

                return Array.IndexOf(_data, item, 0, _count);
            }

            internal override Node Insert(int branchingFactor, bool isAppend, int index, T item)
            {
                if (_count < _data.Length)
                {
                    if (index < _count)
                        Array.Copy(_data, index, _data, index + 1, _count - index);

                    _data[index] = item;
                    _count++;
                    return null;
                }

                if (isAppend)
                {
                    // optimize the case of adding at the end of the overall list
                    return Empty.Insert(branchingFactor, isAppend, 0, item);
                }
                else
                {
                    // split the node
                    LeafNode splitNode = new LeafNode(branchingFactor);
                    int splitPoint = _count / 2;
                    Array.Copy(_data, splitPoint, splitNode._data, 0, _count - splitPoint);
                    Array.Clear(_data, splitPoint, _count - splitPoint);

                    splitNode._count = _count - splitPoint;
                    _count = splitPoint;

                    // insert the new element into the correct half
                    if (index <= splitPoint)
                    {
                        Insert(branchingFactor, false, index, item);
                    }
                    else
                    {
                        splitNode.Insert(branchingFactor, false, index - splitPoint, item);
                    }

                    splitNode._next = _next;
                    _next = splitNode;
                    return splitNode;
                }
            }
        }
    }
}
