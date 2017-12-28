// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class ImmutableTreeList<T>
    {
        private sealed class LeafNode : Node
        {
            private FixedArray8<T> _data;
            private int _count;
            private bool _frozen;

            internal LeafNode()
            {
            }

            private LeafNode(LeafNode node)
            {
                node._data.Copy(ref _data, node.Count);
                _count = node.Count;
            }

            internal override int Count => _count;

            internal override int NodeCount => _count;

            internal override LeafNode FirstLeaf => this;

            internal override Node FirstChild => null;

            internal override bool IsFrozen => _frozen;

            internal override T this[int index] => _data[index];

            internal override void Freeze()
            {
                _frozen = true;
            }

            private LeafNode AsMutable()
            {
                return IsFrozen ? new LeafNode(this) : this;
            }

            internal override Node SetItem(int index, T value)
            {
                LeafNode mutableNode = AsMutable();
                mutableNode._data[index] = value;
                return mutableNode;
            }

            internal void CopyToArray(Array array, int index)
            {
                _data.Copy(0, array, index, _count);
            }

            internal override (LeafNode leafNode, int offset) GetLeafNode(int index)
            {
                Debug.Assert(index >= 0, $"Assertion failed: {nameof(index)} >= 0");
                Debug.Assert(index < Count, $"Assertion failed: {nameof(index)} < {nameof(Count)}");

                return (this, index);
            }

            internal override int IndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                return _data.IndexOf(item, span.Start, span.Count, equalityComparer);
            }

            internal override int LastIndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                return _data.LastIndexOf(item, span.EndInclusive, span.Count, equalityComparer);
            }

            internal override ImmutableTreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, ImmutableTreeList<TOutput>.Node convertedNextNode)
            {
                var result = new ImmutableTreeList<TOutput>.LeafNode();

                for (int i = _count - 1; i >= 0; i--)
                {
                    result._data[i] = converter(_data[i]);
                }

                result._count = _count;
                return result;
            }

            internal override (Node currentNode, Node splitNode) Insert(bool isAppend, int index, T item)
            {
                if (_count < _data.Length)
                {
                    LeafNode mutableNode = AsMutable();
                    if (index < _count)
                        mutableNode._data.Copy(index, ref mutableNode._data, index + 1, _count - index);

                    mutableNode._data[index] = item;
                    mutableNode._count++;
                    return (currentNode: mutableNode, splitNode: null);
                }

                if (isAppend)
                {
                    // optimize the case of adding at the end of the overall list
                    var (splitNode, _) = Empty.Insert(isAppend, 0, item);
                    return (currentNode: this, splitNode);
                }
                else
                {
                    return AsMutable().InsertWithSplit(isAppend, index, item);
                }
            }

            private (Node currentNode, Node splitNode) InsertWithSplit(bool isAppend, int index, T item)
            {
                Debug.Assert(!IsFrozen, $"Assertion failed: {nameof(IsFrozen)}");

                // split the node
                LeafNode splitNode = new LeafNode();
                int splitPoint = _count / 2;

                bool forceNext = false;
                if ((_count + 1) / 2 > splitPoint && index > splitPoint)
                {
                    // When splitting a node with an odd branching factor, prior to insertion one split node will
                    // have (b-1)/2 nodes and the other will have (b+1)/2 nodes. Since the minimum number of nodes
                    // after insertion is (b+1)/2, the split point uniquely determines the insertion point. This
                    // block handles the case where the insertion point is index (b+1)/2 by forcing it to the first
                    // node of the next page instead of adding it (where it fits) at the end of the first page.
                    splitPoint++;
                    forceNext = true;
                }

                _data.Copy(splitPoint, ref splitNode._data, 0, _count - splitPoint);
                _data.Clear(splitPoint, _count - splitPoint);

                splitNode._count = _count - splitPoint;
                _count = splitPoint;

                // insert the new element into the correct half
                if (!forceNext && index <= splitPoint)
                {
                    var (mustBeThis, mustBeNull) = Insert(false, index, item);
                    Debug.Assert(mustBeThis == this, $"Assertion failed: {nameof(mustBeThis)} == this");
                    Debug.Assert(mustBeNull == null, $"Assertion failed: {nameof(mustBeNull)} == null");
                }
                else
                {
                    var (mustBeSplitNode, mustBeNull) = splitNode.Insert(false, index - splitPoint, item);
                    Debug.Assert(mustBeSplitNode == splitNode, $"Assertion failed: {nameof(mustBeSplitNode)} == {nameof(splitNode)}");
                    Debug.Assert(mustBeNull == null, $"Assertion failed: {nameof(mustBeNull)} == null");
                }

                return (currentNode: this, splitNode);
            }

            internal override ImmutableTreeList<Node>.Node InsertRange(bool isAppend, int index, IEnumerable<T> collection)
            {
                Node insertionNode = AsMutable();
                int insertionNodeIndex = 0;
                ImmutableTreeList<Node>.Node lastLeaf = ImmutableTreeList<Node>.Node.Insert(ImmutableTreeList<Node>.Node.Empty, 0, insertionNode);
                foreach (T item in collection)
                {
                    Debug.Assert(index >= 0 && index <= ((LeafNode)insertionNode)._data.Length, "Assertion failed: index >= 0 && index <= ((LeafNode)insertionNode)._data.Length");

                    (_, Node newLastLeaf) = insertionNode.Insert(isAppend, index, item);
                    if (newLastLeaf != null)
                    {
                        // this insertion resulted in a split, so at minimum 'index' must be updated
                        if (insertionNodeIndex != lastLeaf.Count - 1)
                        {
                            lastLeaf = ImmutableTreeList<Node>.Node.Insert(lastLeaf, insertionNodeIndex + 1, newLastLeaf);

                            // We were not inserting into the last leaf (an earlier split in the InsertRange operation
                            // resulted in insertions prior to the last leaf)
                            if (index < insertionNode.Count)
                            {
                                // The split does not change the insertion node.
                                index++;
                            }
                            else
                            {
                                index = index + 1 - insertionNode.Count;
                                insertionNode = newLastLeaf;
                                insertionNodeIndex++;
                            }
                        }
                        else if (index < insertionNode.Count)
                        {
                            // The split resulted in a new last leaf, but no change in the insertion node.
                            index++;
                            lastLeaf = ImmutableTreeList<Node>.Node.Insert(lastLeaf, insertionNodeIndex + 1, newLastLeaf);
                        }
                        else
                        {
                            // The split resulted in a new last leaf which becomes the new insertion node.
                            index = index + 1 - insertionNode.Count;
                            lastLeaf = ImmutableTreeList<Node>.Node.Insert(lastLeaf, insertionNodeIndex + 1, newLastLeaf);
                            insertionNode = newLastLeaf;
                            insertionNodeIndex++;
                        }
                    }
                    else
                    {
                        index++;
                    }
                }

                return lastLeaf;
            }

            internal override Node RemoveLast()
            {
                Debug.Assert(_count > 0, $"Assertion failed: _count > 0");

                if (_count == 1)
                {
                    return null;
                }
                else
                {
                    Debug.Assert(_count > 1, $"Assertion failed: _count > 1");
                    LeafNode result = AsMutable();
                    result._count--;
                    result._data[result._count] = default;
                    return result;
                }
            }

            internal override (Node currentNode, Node nextNode) RemoveAt(int index, Node nextNode)
            {
                if (IsFrozen)
                    return AsMutable().RemoveAt(index, nextNode);

                for (int i = index; i < _count - 1; i++)
                {
                    _data[i] = _data[i + 1];
                }

                _data[_count - 1] = default;
                _count--;

                if (_count < _data.Length / 2 && nextNode != null)
                {
                    if (_count + nextNode.Count <= _data.Length)
                    {
                        // Merge nodes and remove the next node
                        ((LeafNode)nextNode)._data.Copy(0, ref _data, _count, nextNode.Count);
                        _count += nextNode.Count;
                        return (this, null);
                    }
                    else
                    {
                        // Rebalance nodes
                        nextNode = ((LeafNode)nextNode).AsMutable();
                        int minimumNodeCount = _data.Length / 2;
                        int transferCount = nextNode.Count - minimumNodeCount;
                        ((LeafNode)nextNode)._data.Copy(0, ref _data, _count, transferCount);
                        ((LeafNode)nextNode)._data.Copy(transferCount, ref ((LeafNode)nextNode)._data, 0, nextNode.Count - transferCount);
                        ((LeafNode)nextNode)._data.Clear(nextNode.Count - transferCount, transferCount);
                        _count += transferCount;
                        ((LeafNode)nextNode)._count -= transferCount;
                        return (this, nextNode);
                    }
                }

                return (this, nextNode);
            }

            internal override Node Sort(TreeSpan span, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                LeafNode mutableNode = AsMutable();
                mutableNode._data.Sort(span.Start, span.Count, comparer);
                return mutableNode;
            }

            internal override int FindIndex(TreeSpan span, Predicate<T> match)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                return _data.FindIndex(span.Start, span.Count, match);
            }

            internal override int FindLastIndex(TreeSpan span, Predicate<T> match)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                return _data.FindLastIndex(span.EndInclusive, span.Count, match);
            }

            internal override int BinarySearch(TreeSpan span, T item, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                return _data.BinarySearch(span.Start, span.Count, item, comparer);
            }

            internal override (Node currentNode, Node nextNode) TrimExcessImpl(Node nextNode)
            {
                if (_count == _data.Length || nextNode == null)
                    return (this, nextNode);

                if (IsFrozen)
                    return AsMutable().TrimExcessImpl(nextNode);

                LeafNode nextLeaf = ((LeafNode)nextNode).AsMutable();
                int elementsToMove = Math.Min(_data.Length - _count, nextNode.Count);
                nextLeaf._data.Copy(0, ref _data, _count, elementsToMove);
                _count += elementsToMove;

                if (elementsToMove == nextLeaf._count)
                {
                    nextLeaf = null;
                }
                else
                {
                    nextLeaf._data.Copy(elementsToMove, ref nextLeaf._data, 0, nextLeaf._count - elementsToMove);
                    nextLeaf._count -= elementsToMove;
                    nextLeaf._data.Clear(nextLeaf._count, elementsToMove);
                }

                return (this, nextLeaf);
            }

            internal override void Validate(ValidationRules rules, Node nextNode)
            {
                Debug.Assert(_data.Length >= 2, $"Assertion failed: {nameof(_data.Length)} >= 2");
                Debug.Assert(_count >= 0 && _count <= _data.Length, $"Assertion failed: {nameof(_count)} >= 0 && {nameof(_count)} <= {nameof(_data)}.Length");

                // Only the last node is allowed to have a reduced number of children
                Debug.Assert(_count >= (_data.Length + 1) / 2 || nextNode == null, $"Assertion failed: {nameof(_count)} >= ({nameof(_data)}.Length + 1) / 2 || {nameof(nextNode)} == null");

                if (default(T) == null)
                {
                    for (int i = _count; i < _data.Length; i++)
                        Debug.Assert(_data[i] == null, $"Assertion failed: {nameof(_data)}[i] == null");
                }

                if (rules.HasFlag(ValidationRules.RequirePacked))
                {
                    Debug.Assert(nextNode == null || _count == _data.Length, $"Assertion failed: {nameof(nextNode)} == null || {nameof(_count)} == {nameof(_data)}.Length");
                }
            }
        }
    }
}
