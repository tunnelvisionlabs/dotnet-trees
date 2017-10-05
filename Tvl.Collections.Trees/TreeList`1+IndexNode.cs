// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class TreeList<T>
    {
        private sealed class IndexNode : Node
        {
            private readonly int[] _offsets;
            private readonly Node[] _nodes;
            private IndexNode _next;
            private int _nodeCount;
            private int _count;

            internal IndexNode(int branchingFactor)
            {
                _offsets = new int[branchingFactor];
                _nodes = new Node[branchingFactor];
            }

            internal IndexNode(int branchingFactor, Node child1, Node child2)
                : this(branchingFactor)
            {
                Debug.Assert(child1.NextNode == child2, "Assertion failed: child1.NextNode == child2");

                _nodes[0] = child1;
                _nodes[1] = child2;
                _offsets[1] = child1.Count;
                _nodeCount = 2;
                _count = child1.Count + child2.Count;
            }

            internal IndexNode(int branchingFactor, Node firstChild, Node lastChild, out IndexNode lastNode)
                : this(branchingFactor)
            {
                lastNode = null;
                for (Node current = firstChild; current != null; current = current == lastChild ? null : current.NextNode)
                {
                    if (_nodeCount == _nodes.Length)
                    {
                        // TODO: Avoid recursion here
                        _next = new IndexNode(branchingFactor, current, lastChild, out lastNode);
                        break;
                    }

                    _nodes[_nodeCount] = current;
                    _offsets[_nodeCount] = _count;
                    _count += current.Count;
                    _nodeCount++;
                }

                lastNode = lastNode ?? this;
            }

            internal override int Count => _count;

            internal override LeafNode FirstLeaf => _nodes[0].FirstLeaf;

            internal override Node NextNode => Next;

            internal override Node FirstChild => _nodes[0];

            internal IndexNode Next => _next;

            internal override T this[int index]
            {
                get
                {
                    int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                    return _nodes[pageIndex][index - _offsets[pageIndex]];
                }

                set
                {
                    int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                    _nodes[pageIndex][index - _offsets[pageIndex]] = value;
                }
            }

            private static int FindLowerBound(int[] data, int length, int value)
            {
                int index = Array.BinarySearch(data, 0, length, value);
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
                var (leafNode, indexWithinPage) = _nodes[pageIndex].GetLeafNode(index);
                return (leafNode, indexWithinPage + _offsets[pageIndex]);
            }

            internal override int IndexOf(T item, TreeSpan span)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                for (int i = FindLowerBound(_offsets, _nodeCount, span.Start); i < _nodeCount; i++)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        return -1;

                    int foundIndex = _nodes[i].IndexOf(item, mappedSpan);
                    if (foundIndex >= 0)
                        return _offsets[i] + foundIndex;
                }

                return -1;
            }

            internal override int LastIndexOf(T item, TreeSpan span)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                for (int i = FindLowerBound(_offsets, _nodeCount, span.EndInclusive); i >= 0; i--)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        return -1;

                    int foundIndex = _nodes[i].LastIndexOf(item, mappedSpan);
                    if (foundIndex >= 0)
                        return _offsets[i] + foundIndex;
                }

                return -1;
            }

            internal override Node Insert(int branchingFactor, bool isAppend, int index, T item)
            {
                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                Node splitChild = _nodes[pageIndex].Insert(branchingFactor, isAppend, index - _offsets[pageIndex], item);
                if (splitChild == null)
                {
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i]++;

                    _count++;
                    return null;
                }

                for (int i = pageIndex + 1; i < _nodeCount; i++)
                    _offsets[i] = _offsets[i - 1] + _nodes[i - 1].Count;

                _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;
                return InsertIndex(branchingFactor, isAppend, pageIndex + 1, splitChild);
            }

            internal override Node InsertRange(int branchingFactor, bool isAppend, int index, IEnumerable<T> collection)
            {
                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                int previousCount = _nodes[pageIndex].Count;
                Node splitChild = _nodes[pageIndex].InsertRange(branchingFactor, isAppend, index - _offsets[pageIndex], collection);
                if (splitChild == null)
                {
                    int insertionCount = _nodes[pageIndex].Count - previousCount;
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i] += insertionCount;

                    _count += insertionCount;
                    return null;
                }

                throw new NotImplementedException();
            }

            internal override bool RemoveLast()
            {
                if (_next != null)
                {
                    Debug.Assert(_next.Count == 1, $"Assertion failed: _next.Count == 1");
                    bool removedChild = _nodes[_nodeCount - 1].RemoveLast();
                    Debug.Assert(removedChild, $"Assertion failed: removedChild");
                    _next = null;
                    return true;
                }
                else
                {
                    Debug.Assert(_count > 1, $"Assertion failed: _count > 1");
                    if (_nodes[_nodeCount - 1].Count > 1)
                    {
                        bool removedChild = _nodes[_nodeCount - 1].RemoveLast();
                        Debug.Assert(!removedChild, $"Assertion failed: !removedChild");
                        _count--;
                        return false;
                    }
                    else
                    {
                        bool removedChild = _nodes[_nodeCount - 2].RemoveLast();
                        Debug.Assert(removedChild, $"Assertion failed: removedChild");
                        _nodeCount--;
                        _offsets[_nodeCount] = 0;
                        _nodes[_nodeCount] = null;
                        _count--;
                        return false;
                    }
                }
            }

            internal override bool RemoveAt(int index)
            {
                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                bool rebalancedChild = _nodes[pageIndex].RemoveAt(index - _offsets[pageIndex]);
                if (!rebalancedChild)
                {
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i]--;

                    _count--;
                    return false;
                }

                Node expectedNext = pageIndex == _nodeCount - 1 ? _next?._nodes[0] : _nodes[pageIndex + 1];
                Node nextChild = _nodes[pageIndex].NextNode;
                bool removedChild = nextChild != expectedNext;
                if (!removedChild)
                {
                    // This assertion can only fail if RemoveAt(int) is used when RemoveLast() is needed.
                    Debug.Assert(_nodes[pageIndex].Count > 0, $"Assertion failed: _nodes[pageIndex].Count > 0");

                    // The children were rebalanced, but that only affects node counts at this level
                    for (int i = pageIndex; i < _nodeCount; i++)
                    {
                        _offsets[i] = i == 0 ? 0 : _offsets[i - 1] + _nodes[i - 1].Count;
                    }

                    _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;

                    bool affectedNextPage = pageIndex == _nodeCount - 1 && _next != null;
                    if (affectedNextPage)
                    {
                        for (int i = 1; i < _next._nodeCount; i++)
                        {
                            _next._offsets[i] = _next._offsets[i - 1] + _next._nodes[i - 1].Count;
                        }

                        _next._count = _next._offsets[_next._nodeCount - 1] + _next._nodes[_next._nodeCount - 1].Count;
                    }

                    return affectedNextPage;
                }
                else
                {
                    bool removedFromNextPage = pageIndex == _nodeCount - 1;
                    if (removedFromNextPage)
                    {
                        if (_next._nodeCount == 1)
                        {
                            // Removed the only child of the next page
                            Debug.Assert(_next._next == null, $"Assertion failed: _next._next == null");
                            _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;
                            _next = null;
                            return true;
                        }

                        // The general strategy when the first node is removed from the next page is to move the last
                        // node of the current page to the first node of the next. This trivially works as long as it
                        // doesn't force a rebalancing of the current page
                        _next._nodes[0] = _nodes[_nodeCount - 1];
                        _count = _offsets[_nodeCount - 1];
                        _offsets[_nodeCount - 1] = 0;
                        _nodes[_nodeCount - 1] = null;
                        _nodeCount--;
                        for (int i = 1; i < _next._nodeCount; i++)
                        {
                            _next._offsets[i] = _next._offsets[i - 1] + _next._nodes[i - 1].Count;
                        }

                        _next._count = _next._offsets[_next._nodeCount - 1] + _next._nodes[_next._nodeCount - 1].Count;
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

                    if (_nodeCount < _nodes.Length / 2 && _next != null)
                    {
                        if (_nodeCount + _next._nodeCount <= _nodes.Length)
                        {
                            // Merge nodes and remove the next node
                            Array.Copy(_next._nodes, 0, _nodes, _nodeCount, _next._nodeCount);
                            for (int i = 0; i < _next._nodeCount; i++)
                                _offsets[_nodeCount + i] = _offsets[_nodeCount + i - 1] + _nodes[_nodeCount + i - 1].Count;

                            _nodeCount += _next._nodeCount;
                            _count += _next.Count;
                            _next = _next._next;
                            return true;
                        }
                        else
                        {
                            // Rebalance nodes
                            int minimumNodeCount = _nodes.Length / 2;
                            int transferCount = _next._nodeCount - minimumNodeCount;
                            Array.Copy(_next._nodes, 0, _nodes, _nodeCount, transferCount);
                            Array.Copy(_next._nodes, transferCount, _next._nodes, 0, _next._nodeCount - transferCount);
                            Array.Clear(_next._offsets, _next._nodeCount - transferCount, transferCount);
                            Array.Clear(_next._nodes, _next._nodeCount - transferCount, transferCount);
                            for (int i = 0; i < transferCount; i++)
                            {
                                _offsets[_nodeCount + i] = _offsets[_nodeCount + i - 1] + _nodes[_nodeCount + i - 1].Count;
                            }

                            _nodeCount += transferCount;
                            _next._nodeCount -= transferCount;
                            _count = _offsets[_nodeCount - 1] + _nodes[_nodeCount - 1].Count;

                            for (int i = 1; i < _next._nodeCount; i++)
                            {
                                _next._offsets[i] = _next._offsets[i - 1] + _next._nodes[i - 1].Count;
                            }

                            _next._count = _next._offsets[_next._nodeCount - 1] + _next._nodes[_next._nodeCount - 1].Count;
                            return true;
                        }
                    }

                    // No rebalancing was done, but if we removed the child node from the next page then it was still
                    // impacted due to moving a child from the current page to the next.
                    return removedFromNextPage;
                }
            }

            internal override void Sort(TreeSpan span, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                int firstPage = FindLowerBound(_offsets, _nodeCount, span.Start);
                int lastPage = firstPage;
                for (int i = firstPage; i < _nodeCount; i++)
                {
                    TreeSpan mappedSpan = MapSpanDownToChild(span, i);
                    if (mappedSpan.IsEmpty)
                        break;

                    lastPage = i;
                    _nodes[i].Sort(mappedSpan, comparer);
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
                            this[i] = this[j];
                            this[j] = temp;
                            i++;
                            while (i < first.EndExclusive && j < second.EndExclusive - 1 && comparer.Compare(temp, this[j + 1]) > 0)
                            {
                                j++;
                                T temp2 = this[i];
                                this[i] = this[j];
                                this[j] = temp2;
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

                for (int i = FindLowerBound(_offsets, _nodeCount, span.Start); i < Count; i++)
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

                    T value;
                    if (page == firstPage)
                    {
                        LeafNode firstLeaf = _nodes[page].FirstLeaf;
                        if (span.Start - _offsets[firstPage] < firstLeaf.Count)
                        {
                            value = firstLeaf[span.Start - _offsets[firstPage]];
                        }
                        else
                        {
                            value = _nodes[firstPage][span.Start - _offsets[firstPage]];
                        }
                    }
                    else
                    {
                        value = _nodes[page].FirstLeaf[0];
                    }

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

                if (lowPage < 0 || lowPage >= _nodeCount)
                    throw new NotImplementedException();

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

            internal override TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, TreeList<TOutput>.Node convertedNextNode)
            {
                var result = new TreeList<TOutput>.IndexNode(_nodes.Length);

                TreeList<TOutput>.Node convertedNextChild = convertedNextNode?.FirstChild;
                for (int i = _nodeCount - 1; i >= 0; i--)
                {
                    convertedNextChild = _nodes[i].ConvertAll(converter, convertedNextChild);
                    result._nodes[i] = convertedNextChild;
                }

                Array.Copy(_offsets, result._offsets, _nodeCount);
                result._next = (TreeList<TOutput>.IndexNode)convertedNextNode;
                result._count = _count;
                result._nodeCount = _nodeCount;
                return result;
            }

            internal override bool TrimExcess()
            {
                if (!FirstChild.TrimExcess())
                    return false;

                // Simply rebuild this level by walking child nodes
                IndexNode first = this;
                int firstOffset = 0;
                _nodeCount = 0;
                _count = 0;
                for (Node child = FirstChild; child != null; child = child.NextNode)
                {
                    if (firstOffset == first._nodes.Length)
                    {
                        first = first.Next;
                        firstOffset = 0;
                        first._nodeCount = 0;
                        first._count = 0;
                    }

                    first._offsets[firstOffset] = first._count;
                    first._nodes[firstOffset] = child;
                    first._nodeCount++;
                    first._count += child.Count;
                    firstOffset++;
                }

                Array.Clear(first._offsets, first._nodeCount, first._offsets.Length - first._nodeCount);
                Array.Clear(first._nodes, first._nodeCount, first._nodes.Length - first._nodeCount);
                first._next = null;
                return true;
            }

            private TreeSpan MapSpanDownToChild(TreeSpan span, int childIndex)
            {
                Debug.Assert(childIndex >= 0 && childIndex <= _nodeCount, $"Assertion failed: {nameof(childIndex)} >= 0 && {nameof(childIndex)} <= {nameof(_nodeCount)}");
                if (childIndex == _nodeCount)
                {
                    return new TreeSpan(Count, 0);
                }

                // Offset the input span
                TreeSpan mappedFullSpan = span.Offset(-_offsets[childIndex]);

                // Return the intersection
                return TreeSpan.Intersect(mappedFullSpan, _nodes[childIndex].Span);
            }

            private Node InsertIndex(int branchingFactor, bool isAppend, int index, Node node)
            {
                if (_nodeCount < _nodes.Length)
                {
                    if (index < _nodeCount)
                    {
                        Array.Copy(_nodes, index, _nodes, index + 1, _nodeCount - index);
                        Array.Copy(_offsets, index, _offsets, index + 1, _nodeCount - index);
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
                    return null;
                }

                if (isAppend)
                {
                    // optimize the case of adding at the end of the overall list
                    IndexNode result = new IndexNode(branchingFactor);
                    result._nodes[0] = node;
                    result._nodeCount = 1;
                    result._count = node.Count;
                    _next = result;
                    return result;
                }
                else
                {
                    // split the node
                    IndexNode splitNode = new IndexNode(branchingFactor);
                    int splitPoint = _nodeCount / 2;
                    Array.Copy(_nodes, splitPoint, splitNode._nodes, 0, _nodeCount - splitPoint);
                    Array.Copy(_offsets, splitPoint, splitNode._offsets, 0, _nodeCount - splitPoint);
                    Array.Clear(_nodes, splitPoint, _nodeCount - splitPoint);
                    Array.Clear(_offsets, splitPoint, _nodeCount - splitPoint);

                    splitNode._nodeCount = _nodeCount - splitPoint;
                    int adjustment = splitNode._offsets[0];
                    for (int i = 0; i < splitNode._nodeCount; i++)
                        splitNode._offsets[i] -= adjustment;

                    splitNode._count = _count - adjustment;

                    _nodeCount = splitPoint;
                    _count = adjustment;

                    // insert the new element into the correct half
                    if (index <= splitPoint)
                        InsertIndex(branchingFactor, false, index, node);
                    else
                        splitNode.InsertIndex(branchingFactor, false, index - splitPoint, node);

                    splitNode._next = _next;
                    _next = splitNode;
                    return splitNode;
                }
            }

            internal override void Validate(ValidationRules rules)
            {
                Debug.Assert(_nodes != null, $"Assertion failed: {nameof(_nodes)} != null");
                Debug.Assert(_nodes.Length >= 2, $"Assertion failed: {nameof(_nodes.Length)} >= 2");
                Debug.Assert(_offsets != null, $"Assertion failed: {nameof(_offsets)} != null");
                Debug.Assert(_offsets.Length == _nodes.Length, $"Assertion failed: {nameof(_offsets)}.Length == {nameof(_nodes)}.Length");
                Debug.Assert(_nodeCount >= 0 && _nodeCount <= _nodes.Length, $"Assertion failed: {nameof(_nodeCount)} >= 0 && {nameof(_nodeCount)} <= {nameof(_nodes)}.Length");

                // Only the last node is allowed to have a reduced number of children
                Debug.Assert(_nodeCount >= _nodes.Length / 2 || _next == null, $"Assertion failed: {nameof(_nodeCount)} >= {nameof(_nodes)}.Length / 2 || {nameof(_next)} == null");

                int sum = 0;
                for (int i = 0; i < _nodeCount; i++)
                {
                    _nodes[i].Validate(rules);

                    Debug.Assert(_offsets[i] == sum, $"Assertion failed: {nameof(_offsets)}[i] == {nameof(sum)}");
                    if (i < _nodeCount - 1)
                    {
                        Debug.Assert(_nodes[i + 1] == _nodes[i].NextNode, $"Assertion failed: {nameof(_nodes)}[i + 1] == {nameof(_nodes)}[i].NextNode");
                    }
                    else if (_next != null)
                    {
                        Debug.Assert(_next._nodes[0] == _nodes[i].NextNode, $"Assertion failed: {nameof(_next)}._nodes[0] == {nameof(_nodes)}[i].NextNode");
                    }
                    else
                    {
                        Debug.Assert(_nodes[i].NextNode == null, $"Assertion failed: {nameof(_nodes)}[i].NextNode == null");
                    }

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
                    Debug.Assert(_next == null || _nodeCount == _nodes.Length, $"Assertion failed: {nameof(_next)} == null || {nameof(_nodeCount)} == {nameof(_nodes)}.Length");
                }
            }
        }
    }
}
