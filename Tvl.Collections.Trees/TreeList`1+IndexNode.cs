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

            internal override int LastIndexOf(T item, int index, int count)
            {
                Debug.Assert(index >= 0 && index < Count, $"Assertion failed: {nameof(index)} >= 0 && {nameof(index)} < {nameof(Count)}");
                Debug.Assert(count >= 0 && count - 1 <= index, $"Assertion failed: {nameof(count)} >= 0 && {nameof(count)} - 1 <= {nameof(index)}");

                for (int i = FindLowerBound(_offsets, _nodeCount, index); i >= 0; i--)
                {
                    int offset = _offsets[i];

                    int adjustedCount = _nodes[i].Count;
                    if (index - count > offset + adjustedCount)
                        return -1;

                    int adjustedIndex = Math.Min(index - offset, _nodes[i].Count - 1);

                    if (adjustedIndex == index - offset)
                        adjustedCount -= adjustedCount - 1 - adjustedIndex;
                    if (index - count >= offset)
                        adjustedCount -= offset - (index - count);

                    int foundIndex = _nodes[i].LastIndexOf(item, adjustedIndex, adjustedCount);
                    if (foundIndex >= 0)
                        return offset + foundIndex;
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

            internal override bool RemoveAt(int index)
            {
                int pageIndex = FindLowerBound(_offsets, _nodeCount, index);
                bool removedChild = _nodes[pageIndex].RemoveAt(index - _offsets[pageIndex]);
                if (!removedChild)
                {
                    for (int i = pageIndex + 1; i < _nodeCount; i++)
                        _offsets[i]--;

                    _count--;
                    return false;
                }

                throw new NotImplementedException();
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
                    throw new NotImplementedException();
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

            private TreeSpan MapSpanDownToChild(TreeSpan span, int childIndex)
            {
                Debug.Assert(childIndex >= 0 && childIndex < _nodeCount, $"Assertion failed: {nameof(childIndex)} >= && {nameof(childIndex)} < {nameof(_nodeCount)}");

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
