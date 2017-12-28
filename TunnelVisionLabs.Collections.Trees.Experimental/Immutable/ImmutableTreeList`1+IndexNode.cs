// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class ImmutableTreeList<T>
    {
        private sealed class IndexNode : Node
        {
            private FixedArray8<int> _offsets;
            private FixedArray8<Node> _nodes;
            private int _nodeCount;
            private int _count;
            private bool _frozen;

            private IndexNode()
            {
            }

            private IndexNode(IndexNode node)
            {
                node._offsets.Copy(ref _offsets, node.NodeCount);
                node._nodes.Copy(ref _nodes, node.NodeCount);
                _nodeCount = node._nodeCount;
                _count = node._count;
            }

            internal IndexNode(Node child1, Node child2)
            {
                _nodes[0] = child1;
                _nodes[1] = child2;
                _offsets[1] = child1.Count;
                _nodeCount = 2;
                _count = child1.Count + child2.Count;
            }

            internal IndexNode(ImmutableTreeList<Node>.Node children, out ImmutableTreeList<Node>.Node lastNode)
            {
                if (children.Count > _offsets.Length)
                {
                    lastNode = ImmutableTreeList<Node>.Node.Empty;
                }
                else
                {
                    lastNode = null;
                }

                for (int pageIndex = 0; pageIndex < children.Count; pageIndex += _offsets.Length)
                {
                    IndexNode current = pageIndex == 0 ? this : new IndexNode();
                    if (lastNode != null)
                        lastNode = ImmutableTreeList<Node>.Node.Insert(lastNode, lastNode.Count, current);

                    int pageSize = Math.Min(children.Count - pageIndex, _offsets.Length);
                    for (int i = 0; i < pageSize; i++)
                    {
                        current._offsets[i] = current._count;
                        current._nodes[i] = children[pageIndex + i];
                        current._count += current._nodes[i].Count;
                    }

                    current._nodeCount = pageSize;
                }
            }

            internal override int Count => _count;

            internal override int NodeCount => _nodeCount;

            internal override LeafNode FirstLeaf => _nodes[0].FirstLeaf;

            internal override Node FirstChild => _nodes[0];

            internal override bool IsFrozen => _frozen;

            internal override T this[int index]
            {
                get
                {
                    int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                    return _nodes[pageIndex][index - _offsets[pageIndex]];
                }
            }

            internal override void Freeze()
            {
                if (!_frozen)
                {
                    for (int i = 0; i < NodeCount; i++)
                    {
                        _nodes[i].Freeze();
                    }

                    _frozen = true;
                }
            }

            private IndexNode AsMutable()
            {
                return IsFrozen ? new IndexNode(this) : this;
            }

            internal override Node SetItem(int index, T value)
            {
                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                Node node = _nodes[pageIndex].SetItem(index - _offsets[pageIndex], value);
                if (node == _nodes[pageIndex])
                    return this;

                IndexNode mutableNode = AsMutable();
                mutableNode._nodes[pageIndex] = node;
                return mutableNode;
            }

            private static int FindLowerBound(in FixedArray8<int> data, int length, int value)
            {
                int index = data.BinarySearch(0, length, value);
                if (index < 0)
                    return -index - 2;

                return index;
            }

            internal override (LeafNode leafNode, int offset) GetLeafNode(int index)
            {
                Debug.Assert(index >= 0 && index < Count, $"Assertion failed: {nameof(index)} >= 0 && {nameof(index)} < {nameof(Count)}");

                if (index == 0)
                    return (FirstLeaf, 0);

                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                return _nodes[pageIndex].GetLeafNode(index - _offsets[pageIndex]);
            }

            internal override int IndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                for (int i = FindLowerBound(_offsets, _nodeCount, span.Start); i < _nodeCount; i++)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        return -1;

                    int foundIndex = _nodes[i].IndexOf(item, mappedSpan, equalityComparer);
                    if (foundIndex >= 0)
                        return _offsets[i] + foundIndex;
                }

                return -1;
            }

            internal override int LastIndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                for (int i = FindLowerBound(_offsets, _nodeCount, span.EndInclusive); i >= 0; i--)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        return -1;

                    int foundIndex = _nodes[i].LastIndexOf(item, mappedSpan, equalityComparer);
                    if (foundIndex >= 0)
                        return _offsets[i] + foundIndex;
                }

                return -1;
            }

            internal override (Node currentNode, Node splitNode) Insert(bool isAppend, int index, T item)
            {
                if (IsFrozen)
                    return AsMutable().Insert(isAppend, index, item);

                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                (Node currentChild, Node splitChild) = _nodes[pageIndex].Insert(isAppend, index - _offsets[pageIndex], item);
                _nodes[pageIndex] = currentChild;
                if (splitChild == null)
                {
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i]++;

                    _count++;
                    return (currentNode: this, null);
                }

                for (int i = pageIndex + 1; i < _nodeCount; i++)
                    _offsets[i] = _offsets[i - 1] + _nodes[i - 1].Count;

                _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;
                return InsertIndex(isAppend, pageIndex + 1, splitChild);
            }

            internal override ImmutableTreeList<Node>.Node InsertRange(bool isAppend, int index, IEnumerable<T> collection)
            {
                if (IsFrozen)
                    return AsMutable().InsertRange(isAppend, index, collection);

                ImmutableTreeList<Node>.Node result = ImmutableTreeList<Node>.Node.Insert(ImmutableTreeList<Node>.Node.Empty, 0, this);
                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                int previousCount = _nodes[pageIndex].Count;
                ImmutableTreeList<Node>.Node impactedChildren = _nodes[pageIndex].InsertRange(isAppend, index - _offsets[pageIndex], collection);
                _nodes[pageIndex] = impactedChildren[0];
                if (impactedChildren.Count == 1)
                {
                    int insertionCount = _nodes[pageIndex].Count - previousCount;
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i] += insertionCount;

                    _count += insertionCount;
                    return result;
                }

                for (int i = pageIndex + 1; i < _nodeCount; i++)
                    _offsets[i] = _offsets[i - 1] + _nodes[i - 1].Count;

                _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;
                pageIndex++;

                IndexNode insertionNode = this;
                int insertionNodeIndex = 0;
                for (int i = 1; i < impactedChildren.Count; i++)
                {
                    Node item = impactedChildren[i];
                    Debug.Assert(item != null, "Assertion failed: item != null");
                    Debug.Assert(pageIndex >= 0 && pageIndex <= insertionNode._nodes.Length, "Assertion failed: pageIndex >= 0 && pageIndex <= insertionNode._nodes.Length");

                    Debug.Assert(!insertionNode.IsFrozen, $"Assertion failed: !{nameof(insertionNode)}.IsFrozen");
                    (_, Node newLastIndex) = insertionNode.InsertIndex(isAppend, pageIndex, item);
                    if (newLastIndex != null)
                    {
                        // this insertion resulted in a split, so at minimum 'pageIndex' must be updated
                        if (insertionNodeIndex != result.Count - 1)
                        {
                            result = ImmutableTreeList<Node>.Node.Insert(result, insertionNodeIndex + 1, newLastIndex);

                            // We were not inserting into the last node (an earlier split in the InsertRange operation
                            // resulted in insertions prior to the last node)
                            //
                            // When we reach this point, a previous index insertion caused a split which did not change
                            // the insertion node. Afterwards, we continued inserting and have now reached a point where
                            // the page is splitting a second time. It is impossible for this split to be caused by an
                            // insertion in the first half of the list. The primary difference between this situation
                            // and similar code in LeafNode is the first insertion index - for LeafNode it is possible
                            // to start inserting at index 0, but for IndexNode the first possible insertion is index 1.
                            Debug.Assert(pageIndex >= insertionNode._nodeCount, $"Assertion failed: {nameof(pageIndex)} >= {nameof(insertionNode)}._nodeCount");
                            pageIndex = pageIndex + 1 - insertionNode._nodeCount;
                            insertionNode = (IndexNode)newLastIndex;
                            insertionNodeIndex++;
                        }
                        else if (pageIndex < insertionNode._nodeCount)
                        {
                            // The split resulted in a new last node, but no change in the insertion node.
                            pageIndex++;
                            result = ImmutableTreeList<Node>.Node.Insert(result, insertionNodeIndex + 1, newLastIndex);
                        }
                        else
                        {
                            // The split resulted in a new last node which becomes the new insertion node.
                            pageIndex = pageIndex + 1 - insertionNode._nodeCount;
                            result = ImmutableTreeList<Node>.Node.Insert(result, insertionNodeIndex + 1, newLastIndex);
                            insertionNode = (IndexNode)newLastIndex;
                            insertionNodeIndex++;
                        }
                    }
                    else
                    {
                        pageIndex++;
                    }
                }

                return result;
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
                    IndexNode result = AsMutable();
                    Node lastNode = result._nodes[result._nodeCount - 1];
                    if (lastNode.Count == 1)
                    {
                        result._nodeCount--;
                        result._offsets[result._nodeCount] = default;
                        result._nodes[result._nodeCount] = default;
                    }
                    else
                    {
                        result._nodes[result._nodeCount - 1] = lastNode.RemoveLast();
                    }

                    result._count--;
                    return result;
                }
            }

            internal override (Node currentNode, Node nextNode) RemoveAt(int index, Node nextNode)
            {
                if (IsFrozen)
                    return AsMutable().RemoveAt(index, nextNode);

                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                Node originalNextChild = pageIndex < _nodeCount - 1 ? _nodes[pageIndex + 1] : nextNode?.FirstChild;
                (Node modifiedChild, Node modifiedNextChild) = _nodes[pageIndex].RemoveAt(index - _offsets[pageIndex], originalNextChild);
                _nodes[pageIndex] = modifiedChild;
                if (modifiedNextChild == originalNextChild)
                {
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i]--;

                    _count--;
                    return (this, nextNode);
                }

                IndexNode nextIndex = (IndexNode)nextNode;

                bool removedChild = modifiedNextChild == null;
                if (!removedChild)
                {
                    bool affectedNextPage;
                    if (pageIndex < _nodeCount - 1)
                    {
                        affectedNextPage = false;
                        _nodes[pageIndex + 1] = modifiedNextChild;
                    }
                    else
                    {
                        affectedNextPage = true;
                        nextIndex = nextIndex.AsMutable();
                        nextIndex._nodes[0] = modifiedNextChild;
                    }

                    // This assertion can only fail if RemoveAt(int) is used when RemoveLast() is needed.
                    Debug.Assert(_nodes[pageIndex].Count > 0, $"Assertion failed: _nodes[pageIndex].Count > 0");

                    // The children were rebalanced, but that only affects node counts at this level
                    for (int i = pageIndex; i < _nodeCount; i++)
                    {
                        _offsets[i] = i == 0 ? 0 : _offsets[i - 1] + _nodes[i - 1].Count;
                    }

                    _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;

                    if (affectedNextPage)
                    {
                        for (int i = 1; i < nextIndex._nodeCount; i++)
                        {
                            nextIndex._offsets[i] = nextIndex._offsets[i - 1] + nextIndex._nodes[i - 1].Count;
                        }

                        nextIndex._count = nextIndex._offsets[nextIndex._nodeCount - 1] + nextIndex._nodes[nextIndex._nodeCount - 1].Count;
                    }

                    return (this, nextIndex);
                }
                else
                {
                    bool removedFromNextPage = pageIndex == _nodeCount - 1;
                    if (removedFromNextPage)
                    {
                        if (nextIndex._nodeCount == 1)
                        {
                            // Removed the only child of the next page
                            _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;
                            return (this, null);
                        }

                        nextIndex = nextIndex.AsMutable();

                        // The general strategy when the first node is removed from the next page is to move the last
                        // node of the current page to the first node of the next. This trivially works as long as it
                        // doesn't force a rebalancing of the current page
                        nextIndex._nodes[0] = _nodes[_nodeCount - 1];
                        _count = _offsets[_nodeCount - 1];
                        _offsets[_nodeCount - 1] = 0;
                        _nodes[_nodeCount - 1] = null;
                        _nodeCount--;
                        for (int i = 1; i < nextIndex._nodeCount; i++)
                        {
                            nextIndex._offsets[i] = nextIndex._offsets[i - 1] + nextIndex._nodes[i - 1].Count;
                        }

                        nextIndex._count = nextIndex._offsets[nextIndex._nodeCount - 1] + nextIndex._nodes[nextIndex._nodeCount - 1].Count;
                    }
                    else
                    {
                        for (int i = pageIndex + 1; i < _nodeCount - 1; i++)
                        {
                            _offsets[i] = _offsets[i - 1] + _nodes[i - 1].Count;
                            _nodes[i] = _nodes[i + 1];
                        }

                        _offsets[_nodeCount - 1] = 0;
                        _nodes[_nodeCount - 1] = default;
                        _nodeCount--;
                        _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;
                    }

                    if (_nodeCount < _nodes.Length / 2 && nextIndex != null)
                    {
                        if (_nodeCount + nextIndex._nodeCount <= _nodes.Length)
                        {
                            // Merge nodes and remove the next node
                            nextIndex._nodes.Copy(0, ref _nodes, _nodeCount, nextIndex._nodeCount);
                            for (int i = 0; i < nextIndex._nodeCount; i++)
                                _offsets[_nodeCount + i] = _offsets[_nodeCount + i - 1] + _nodes[_nodeCount + i - 1].Count;

                            _nodeCount += nextIndex._nodeCount;
                            _count += nextIndex.Count;
                            return (this, null);
                        }
                        else
                        {
                            // Rebalance nodes
                            nextIndex = nextIndex.AsMutable();
                            int minimumNodeCount = _nodes.Length / 2;
                            int transferCount = nextIndex._nodeCount - minimumNodeCount;
                            nextIndex._nodes.Copy(0, ref _nodes, _nodeCount, transferCount);
                            nextIndex._nodes.Copy(transferCount, ref nextIndex._nodes, 0, nextIndex._nodeCount - transferCount);
                            nextIndex._offsets.Clear(nextIndex._nodeCount - transferCount, transferCount);
                            nextIndex._nodes.Clear(nextIndex._nodeCount - transferCount, transferCount);
                            for (int i = 0; i < transferCount; i++)
                            {
                                _offsets[_nodeCount + i] = _offsets[_nodeCount + i - 1] + _nodes[_nodeCount + i - 1].Count;
                            }

                            _nodeCount += transferCount;
                            nextIndex._nodeCount -= transferCount;
                            _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;

                            for (int i = 1; i < nextIndex._nodeCount; i++)
                            {
                                nextIndex._offsets[i] = nextIndex._offsets[i - 1] + nextIndex._nodes[i - 1].Count;
                            }

                            nextIndex._count = nextIndex._offsets[nextIndex._nodeCount - 1] + nextIndex._nodes[nextIndex._nodeCount - 1].Count;
                            return (this, nextIndex);
                        }
                    }

                    // No rebalancing was done, but if we removed the child node from the next page then it was still
                    // impacted due to moving a child from the current page to the next.
                    return (this, nextIndex);
                }
            }

            internal override Node Sort(TreeSpan span, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                if (IsFrozen)
                    return AsMutable().Sort(span, comparer);

                int firstPage = FindLowerBound(_offsets, _nodeCount, span.Start);
                int lastPage = firstPage;
                for (int i = firstPage; i < _nodeCount; i++)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        break;

                    lastPage = i;
                    _nodes[i] = _nodes[i].Sort(mappedSpan, comparer);
                }

                if (firstPage != lastPage)
                {
                    // Need to merge the results
                    int pageCount = lastPage - firstPage + 1;
                    for (int mergeSegmentSize = 1; mergeSegmentSize < pageCount; mergeSegmentSize *= 2)
                    {
                        for (int firstSegment = firstPage; firstSegment < lastPage; firstSegment += mergeSegmentSize * 2)
                        {
                            int secondSegment = firstSegment + mergeSegmentSize;
                            if (secondSegment > lastPage)
                                break;

                            TreeSpan firstSpan = GetSegmentSpan(span, firstSegment, mergeSegmentSize);
                            TreeSpan secondSpan = GetSegmentSpan(span, secondSegment, mergeSegmentSize);
                            MergeSegments(firstSpan, secondSpan);
                        }
                    }
                }

                return this;

                // Local functions
                TreeSpan GetSegmentSpan(TreeSpan bounds, int firstPageOfSegment, int segmentPageCount)
                {
                    int lastPageOfSegment = Math.Min(_nodeCount - 1, firstPageOfSegment + segmentPageCount - 1);
                    int startIndex = _offsets[firstPageOfSegment];
                    int endIndexExclusive = _offsets[lastPageOfSegment] + _nodes[lastPageOfSegment].Count;
                    return TreeSpan.Intersect(bounds, TreeSpan.FromBounds(startIndex, endIndexExclusive));
                }

                void MergeSegments(TreeSpan first, TreeSpan second)
                {
                    Debug.Assert(first.IsSubspanOf(Span), $"Assertion failed: {nameof(first)}.IsSubspanOf({nameof(Span)})");
                    Debug.Assert(second.IsSubspanOf(Span), $"Assertion failed: {nameof(second)}.IsSubspanOf({nameof(Span)})");
                    Debug.Assert(first.EndExclusive == second.Start, $"Assertion failed: first.EndExclusive == second.Start");

                    // Stop immediately if already ordered
                    if (comparer.Compare(this[first.EndInclusive], this[second.Start]) <= 0)
                        return;

                    int i = first.Start;
                    int j = second.Start;
                    while (true)
                    {
                        if (i == first.EndExclusive)
                        {
                            break;
                        }

                        Debug.Assert(j < second.EndExclusive, $"Assertion failed: j < second.EndExclusive");

                        int c = comparer.Compare(this[i], this[j]);
                        if (c == 0)
                        {
                            i++;
                        }
                        else if (c < 0)
                        {
                            i++;
                        }
                        else
                        {
                            T temp = this[i];
                            Node mustBeThis = SetItem(i, this[j]);
                            Debug.Assert(mustBeThis == this, $"Assertion failed: mustBeThis == this");
                            mustBeThis = SetItem(j, temp);
                            Debug.Assert(mustBeThis == this, $"Assertion failed: mustBeThis == this");
                            i++;
                            while (i < first.EndExclusive && j < second.EndExclusive - 1 && comparer.Compare(temp, this[j + 1]) > 0)
                            {
                                j++;
                                T temp2 = this[i];
                                mustBeThis = SetItem(i, this[j]);
                                Debug.Assert(mustBeThis == this, $"Assertion failed: mustBeThis == this");
                                mustBeThis = SetItem(j, temp2);
                                Debug.Assert(mustBeThis == this, $"Assertion failed: mustBeThis == this");
                                i++;
                            }

                            if (j > 1 && j + 1 < second.EndExclusive)
                                MergeSegments(TreeSpan.FromBounds(second.Start, j + 1), TreeSpan.FromBounds(j + 1, second.EndExclusive));

                            j = second.Start;
                        }
                    }
                }
            }

            internal override int FindIndex(TreeSpan span, Predicate<T> match)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                for (int i = FindLowerBound(_offsets, _nodeCount, span.Start); i < _nodeCount; i++)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        return -1;

                    int foundIndex = _nodes[i].FindIndex(mappedSpan, match);
                    if (foundIndex >= 0)
                        return _offsets[i] + foundIndex;
                }

                return -1;
            }

            internal override int FindLastIndex(TreeSpan span, Predicate<T> match)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                for (int i = FindLowerBound(_offsets, _nodeCount, span.EndInclusive); i >= 0; i--)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        return -1;

                    int foundIndex = _nodes[i].FindLastIndex(mappedSpan, match);
                    if (foundIndex >= 0)
                        return _offsets[i] + foundIndex;
                }

                return -1;
            }

            internal override int BinarySearch(TreeSpan span, T item, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                // Since accessing FirstLeaf is O(1), at each index level we are only looking for the correct child page
                int firstPage = FindLowerBound(_offsets, _nodeCount, span.Start);
                int lastPage = FindLowerBound(_offsets, _nodeCount, span.EndInclusive);

                int lowPage = firstPage;
                int highPage = lastPage;
                while (lowPage < highPage)
                {
                    // Avoid choosing page == lowPage because we won't have enough information to make progress
                    int page = lowPage + ((highPage - lowPage + 1) >> 1);
                    Debug.Assert(page > firstPage, $"Assertion failed: {nameof(page)} > {nameof(firstPage)}");

                    T value = _nodes[page].FirstLeaf[0];

                    int c;
                    try
                    {
                        c = comparer.Compare(value, item);
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException("Failed to compare two elements in the list.", e);
                    }

                    if (c == 0)
                    {
                        // Binary search allows any index once found
                        return _offsets[page];
                    }

                    if (c < 0)
                    {
                        lowPage = page;
                    }
                    else
                    {
                        highPage = page - 1;
                    }
                }

                Debug.Assert(lowPage >= 0 && lowPage < _nodeCount, $"Assertion failed: {nameof(lowPage)} >= 0 && {nameof(lowPage)} < {nameof(_nodeCount)}");

                int result = _nodes[lowPage].BinarySearch(MapSpanDownToChild(span, lowPage), item, comparer);
                if (result < 0)
                {
                    return ~(~result + _offsets[lowPage]);
                }
                else
                {
                    return result + _offsets[lowPage];
                }
            }

            internal override ImmutableTreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, ImmutableTreeList<TOutput>.Node convertedNextNode)
            {
                var result = new ImmutableTreeList<TOutput>.IndexNode();

                ImmutableTreeList<TOutput>.Node convertedNextChild = convertedNextNode?.FirstChild;
                for (int i = _nodeCount - 1; i >= 0; i--)
                {
                    convertedNextChild = _nodes[i].ConvertAll(converter, convertedNextChild);
                    result._nodes[i] = convertedNextChild;
                }

                _offsets.Copy(ref result._offsets, _nodeCount);
                result._count = _count;
                result._nodeCount = _nodeCount;
                return result;
            }

            private static (Node currentNode, Node nextNode) TrimExcessImpl(IndexNode currentNode, IndexNode nextNode)
            {
                for (int i = 0; i < currentNode._nodeCount; i++)
                {
                    Node originalChild = currentNode._nodes[i];
                    int originalChildCount = originalChild.Count;
                    Node originalNextChild = i < currentNode._nodeCount - 1 ? currentNode._nodes[i + 1] : nextNode?.FirstChild;
                    (Node currentChild, Node nextChild) = originalChild.TrimExcessImpl(originalNextChild);
                    if (originalChildCount == currentChild.Count)
                        continue;

                    currentNode = currentNode.AsMutable();
                    currentNode._nodes[i] = currentChild;
                    if (nextChild == null)
                    {
                        if (i < currentNode._nodeCount - 1)
                        {
                            // nextChild belonged to currentNode
                            for (int j = i + 1; j < currentNode._nodeCount - 1; j++)
                            {
                                currentNode._offsets[j] = currentNode._offsets[j + 1];
                                currentNode._nodes[j] = currentNode._nodes[j + 1];
                            }

                            currentNode._offsets[currentNode._nodeCount - 1] = default;
                            currentNode._nodes[currentNode._nodeCount - 1] = default;
                            currentNode._nodeCount--;
                        }
                        else if (nextNode._nodeCount == 1)
                        {
                            int movedCount = currentChild.Count - originalChildCount;
                            currentNode._count += movedCount;
                            nextNode = null;
                        }
                        else
                        {
                            // nextChild belonged to nextNode
                            int movedCount = currentChild.Count - originalChildCount;
                            nextNode = nextNode.AsMutable();
                            for (int j = 0; j < nextNode._nodeCount - 1; j++)
                            {
                                nextNode._offsets[j] = nextNode._offsets[j + 1] - movedCount;
                                nextNode._nodes[j] = nextNode._nodes[j + 1];
                            }

                            nextNode._offsets[nextNode._nodeCount - 1] = default;
                            nextNode._nodes[nextNode._nodeCount - 1] = default;
                            nextNode._nodeCount--;
                            currentNode._count += movedCount;
                            nextNode._count -= movedCount;
                        }
                    }
                    else
                    {
                        Debug.Assert(nextChild.Count > 0, $"Assertion failed: nextChild.Count > 0");

                        if (i < currentNode._nodeCount - 1)
                        {
                            // nextChild belongs to currentNode, and the number of nodes in currentChild and nextChild
                            // didn't change.
                            currentNode._nodes[i + 1] = nextChild;
                            currentNode._offsets[i + 1] = currentNode._offsets[i] + currentChild.Count;
                        }
                        else
                        {
                            // nextChild belongs to nextNode
                            nextNode = nextNode.AsMutable();
                            int movedCount = currentChild.Count - originalChildCount;
                            nextNode._nodes[0] = nextChild;
                            for (int j = 1; j < nextNode._nodeCount; j++)
                            {
                                nextNode._offsets[j] -= movedCount;
                            }

                            currentNode._count += movedCount;
                            nextNode._count -= movedCount;
                        }
                    }

                    // Process the current node again since changes were made
                    i--;
                }

                if (currentNode._nodeCount < currentNode._nodes.Length && nextNode != null)
                {
                    currentNode = currentNode.AsMutable();
                    nextNode = nextNode.AsMutable();
                    int elementsToMove = Math.Min(currentNode._nodes.Length - currentNode._nodeCount, nextNode.NodeCount);
                    nextNode._nodes.Copy(0, ref currentNode._nodes, currentNode._nodeCount, elementsToMove);
                    for (int i = currentNode._nodeCount; i < currentNode._nodeCount + elementsToMove; i++)
                    {
                        currentNode._offsets[i] = currentNode._offsets[i - 1] + currentNode._nodes[i - 1].Count;
                    }

                    currentNode._nodeCount += elementsToMove;
                    currentNode._count = currentNode._offsets[currentNode._nodeCount - 1] + currentNode._nodes[currentNode._nodeCount - 1].Count;

                    nextNode._nodes.Copy(elementsToMove, ref nextNode._nodes, 0, nextNode._nodeCount - elementsToMove);
                    nextNode._nodeCount -= elementsToMove;
                    nextNode._offsets.Clear(nextNode._nodeCount, elementsToMove);
                    nextNode._nodes.Clear(nextNode._nodeCount, elementsToMove);
                    for (int i = 1; i < nextNode._nodeCount; i++)
                    {
                        nextNode._offsets[i] = nextNode._offsets[i - 1] + nextNode._nodes[i - 1].Count;
                    }

                    if (nextNode._nodeCount > 0)
                        nextNode._count = nextNode._offsets[nextNode._nodeCount - 1] + nextNode._nodes[nextNode._nodeCount - 1].Count;
                    else
                        nextNode = null;
                }

                return (currentNode, nextNode);
            }

            internal override (Node currentNode, Node nextNode) TrimExcessImpl(Node nextNode)
            {
                return TrimExcessImpl(this, (IndexNode)nextNode);
            }

            private TreeSpan MapSpanDownToChild(TreeSpan span, int childIndex)
            {
                Debug.Assert(childIndex >= 0 && childIndex < _nodeCount, $"Assertion failed: {nameof(childIndex)} >= 0 && {nameof(childIndex)} < {nameof(_nodeCount)}");

                // Offset the input span
                TreeSpan mappedFullSpan = span.Offset(-_offsets[childIndex]);

                // Return the intersection
                return TreeSpan.Intersect(mappedFullSpan, _nodes[childIndex].Span);
            }

            private (Node currentNode, Node splitNode) InsertIndex(bool isAppend, int index, Node node)
            {
                Debug.Assert(!IsFrozen, $"Assertion failed: !{nameof(IsFrozen)}");

                if (_nodeCount < _nodes.Length)
                {
                    if (index < _nodeCount)
                    {
                        _nodes.Copy(index, ref _nodes, index + 1, _nodeCount - index);
                        _offsets.Copy(index, ref _offsets, index + 1, _nodeCount - index);
                    }
                    else
                    {
                        _offsets[_nodeCount] = Count;
                    }

                    _nodes[index] = node;
                    _nodeCount++;

                    int delta = node.Count;
                    for (int i = index + 1; i < _nodeCount; i++)
                        _offsets[i] += delta;

                    _count += delta;
                    return (this, null);
                }

                if (isAppend)
                {
                    // optimize the case of adding at the end of the overall list
                    IndexNode result = new IndexNode();
                    result._nodes[0] = node;
                    result._nodeCount = 1;
                    result._count = node.Count;
                    return (this, result);
                }
                else
                {
                    // split the node
                    IndexNode splitNode = new IndexNode();
                    int splitPoint = _nodeCount / 2;

                    bool forceNext = false;
                    if ((_nodeCount + 1) / 2 > splitPoint && index > splitPoint)
                    {
                        // When splitting a node with an odd branching factor, prior to insertion one split node will
                        // have (b-1)/2 nodes and the other will have (b+1)/2 nodes. Since the minimum number of nodes
                        // after insertion is (b+1)/2, the split point uniquely determines the insertion point. This
                        // block handles the case where the insertion point is index (b+1)/2 by forcing it to the first
                        // node of the next page instead of adding it (where it fits) at the end of the first page.
                        splitPoint++;
                        forceNext = true;
                    }

                    _nodes.Copy(splitPoint, ref splitNode._nodes, 0, _nodeCount - splitPoint);
                    _offsets.Copy(splitPoint, ref splitNode._offsets, 0, _nodeCount - splitPoint);
                    _nodes.Clear(splitPoint, _nodeCount - splitPoint);
                    _offsets.Clear(splitPoint, _nodeCount - splitPoint);

                    splitNode._nodeCount = _nodeCount - splitPoint;
                    int adjustment = splitNode._offsets[0];
                    for (int i = 0; i < splitNode._nodeCount; i++)
                        splitNode._offsets[i] -= adjustment;

                    splitNode._count = _count - adjustment;

                    _nodeCount = splitPoint;
                    _count = adjustment;

                    // insert the new element into the correct half
                    if (!forceNext && index <= splitPoint)
                        InsertIndex(false, index, node);
                    else
                        splitNode.InsertIndex(false, index - splitPoint, node);

                    return (this, splitNode);
                }
            }

            internal override void Validate(ValidationRules rules, Node nextNode)
            {
                Debug.Assert(_nodes.Length >= 2, $"Assertion failed: {nameof(_nodes.Length)} >= 2");
                Debug.Assert(_offsets.Length == _nodes.Length, $"Assertion failed: {nameof(_offsets)}.Length == {nameof(_nodes)}.Length");
                Debug.Assert(_nodeCount >= 0 && _nodeCount <= _nodes.Length, $"Assertion failed: {nameof(_nodeCount)} >= 0 && {nameof(_nodeCount)} <= {nameof(_nodes)}.Length");

                // Only the last node is allowed to have a reduced number of children
                Debug.Assert(_nodeCount >= (_nodes.Length + 1) / 2 || nextNode == null, $"Assertion failed: {nameof(_nodeCount)} >= ({nameof(_nodes)}.Length + 1) / 2 || {nameof(nextNode)} == null");

                int sum = 0;
                for (int i = 0; i < _nodeCount; i++)
                {
                    _nodes[i].Validate(rules, i < _nodeCount - 1 ? _nodes[i + 1] : nextNode?.FirstChild);

                    Debug.Assert(_offsets[i] == sum, $"Assertion failed: {nameof(_offsets)}[i] == {nameof(sum)}");

                    sum += _nodes[i].Count;
                }

                Debug.Assert(_count == sum, $"Assertion failed: {nameof(_count)} == {nameof(sum)}");

                for (int i = _nodeCount; i < _nodes.Length; i++)
                {
                    Debug.Assert(_offsets[i] == 0, $"Assertion failed: {nameof(_offsets)}[i] == 0");
                    Debug.Assert(_nodes[i] == null, $"Assertion failed: {nameof(_nodes)}[i] == null");
                }

                if (rules.HasFlag(ValidationRules.RequirePacked))
                {
                    Debug.Assert(nextNode == null || _nodeCount == _nodes.Length, $"Assertion failed: {nameof(nextNode)} == null || {nameof(_nodeCount)} == {nameof(_nodes)}.Length");
                }
            }
        }
    }
}
