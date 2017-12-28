// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections.Generic;
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

            internal override Node NextNode => Next;

            internal override Node FirstChild => null;

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

            internal void CopyToArray(Array array, int index)
            {
                Array.Copy(_data, 0, array, index, _count);
            }

            internal override (LeafNode leafNode, int offset) GetLeafNode(int index)
            {
                Debug.Assert(index >= 0 && index < Count, $"Assertion failed: {nameof(index)} >= 0 && {nameof(index)} < {nameof(Count)}");

                return (this, index);
            }

            internal override int IndexOf(T item, TreeSpan span)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                return Array.IndexOf(_data, item, span.Start, span.Count);
            }

            internal override int LastIndexOf(T item, TreeSpan span)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                return Array.LastIndexOf(_data, item, span.EndInclusive, span.Count);
            }

            internal override TreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, TreeList<TOutput>.Node convertedNextNode)
            {
                var result = new TreeList<TOutput>.LeafNode(_data.Length);

                for (int i = _count - 1; i >= 0; i--)
                {
                    result._data[i] = converter(_data[i]);
                }

                result._next = (TreeList<TOutput>.LeafNode)convertedNextNode;
                result._count = _count;
                return result;
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
                    var result = (LeafNode)Empty.Insert(branchingFactor, isAppend, 0, item);
                    _next = result;
                    return result;
                }
                else
                {
                    // split the node
                    LeafNode splitNode = new LeafNode(branchingFactor);
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

                    Array.Copy(_data, splitPoint, splitNode._data, 0, _count - splitPoint);
                    Array.Clear(_data, splitPoint, _count - splitPoint);

                    splitNode._count = _count - splitPoint;
                    _count = splitPoint;

                    // insert the new element into the correct half
                    if (!forceNext && index <= splitPoint)
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

            internal override Node InsertRange(int branchingFactor, bool isAppend, int index, IEnumerable<T> collection)
            {
                Node insertionNode = this;
                Node lastLeaf = null;
                foreach (T item in collection)
                {
                    Debug.Assert(index >= 0 && index <= ((LeafNode)insertionNode)._data.Length, "Assertion failed: index >= 0 && index <= ((LeafNode)insertionNode)._data.Length");

                    Node newLastLeaf = insertionNode.Insert(branchingFactor, isAppend, index, item);
                    if (newLastLeaf != null)
                    {
                        // this insertion resulted in a split, so at minimum 'index' must be updated
                        if (lastLeaf != null && insertionNode != lastLeaf)
                        {
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
                            }
                        }
                        else if (index < insertionNode.Count)
                        {
                            // The split resulted in a new last leaf, but no change in the insertion node.
                            index++;
                            lastLeaf = newLastLeaf;
                        }
                        else
                        {
                            // The split resulted in a new last leaf which becomes the new insertion node.
                            index = index + 1 - insertionNode.Count;
                            lastLeaf = newLastLeaf;
                            insertionNode = newLastLeaf;
                        }
                    }
                    else
                    {
                        index++;
                    }
                }

                return lastLeaf;
            }

            internal override bool RemoveLast()
            {
                if (_next != null)
                {
                    Debug.Assert(_next.Count == 1, $"Assertion failed: _next.Count == 1");
                    _next = null;
                    return true;
                }
                else
                {
                    Debug.Assert(_count > 1, $"Assertion failed: _count > 1");
                    _count--;
                    _data[_count] = default;
                    return false;
                }
            }

            internal override bool RemoveAt(int index)
            {
                for (int i = index; i < _count - 1; i++)
                {
                    _data[i] = _data[i + 1];
                }

                _data[_count - 1] = default;
                _count--;

                if (_count < _data.Length / 2 && _next != null)
                {
                    if (_count + _next.Count <= _data.Length)
                    {
                        // Merge nodes and remove the next node
                        Array.Copy(_next._data, 0, _data, _count, _next.Count);
                        _count += _next._count;
                        _next = _next._next;
                        return true;
                    }
                    else
                    {
                        // Rebalance nodes
                        int minimumNodeCount = _data.Length / 2;
                        int transferCount = _next.Count - minimumNodeCount;
                        Array.Copy(_next._data, 0, _data, _count, transferCount);
                        Array.Copy(_next._data, transferCount, _next._data, 0, _next._count - transferCount);
                        Array.Clear(_next._data, _next._count - transferCount, transferCount);
                        _count += transferCount;
                        _next._count -= transferCount;
                        return true;
                    }
                }

                return _count == 0;
            }

            internal override void Sort(TreeSpan span, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                Array.Sort(_data, span.Start, span.Count, comparer);
            }

            internal override int FindIndex(TreeSpan span, Predicate<T> match)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                return Array.FindIndex(_data, span.Start, span.Count, match);
            }

            internal override int FindLastIndex(TreeSpan span, Predicate<T> match)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                return Array.FindLastIndex(_data, span.EndInclusive, span.Count, match);
            }

            internal override int BinarySearch(TreeSpan span, T item, IComparer<T> comparer)
            {
                Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                return Array.BinarySearch(_data, span.Start, span.Count, item, comparer);
            }

            internal override bool TrimExcess()
            {
                bool changedAnything = false;
                LeafNode first = this;
                int firstOffset = 0;
                LeafNode second = null;
                int secondOffset = 0;
                while (first != null)
                {
                    if (!changedAnything && first.Count == first._data.Length)
                    {
                        // Nothing moved yet
                        first = first.Next;
                        continue;
                    }

                    if (second == null && !changedAnything)
                    {
                        second = first.Next;
                        if (second == null)
                            break;

                        firstOffset = first._count;
                    }

                    if (second == null)
                    {
                        // No more items to copy
                        Debug.Assert(firstOffset > 0, $"Assertion failed: {nameof(firstOffset)} > 0");
                        first._count = firstOffset;
                        for (int i = firstOffset; i < first._data.Length; i++)
                        {
                            first._data[i] = default;
                        }

                        first._next = null;
                        break;
                    }

                    changedAnything = true;
                    int transferCount = Math.Min(second.Count - secondOffset, first._data.Length - firstOffset);
                    for (int i = 0; i < transferCount; i++)
                    {
                        first._data[firstOffset] = second._data[secondOffset];
                        firstOffset++;
                        secondOffset++;
                    }

                    // Move second before first so we can set first._next to null if we hit the end
                    if (secondOffset == second._count)
                    {
                        second = second.Next;
                        secondOffset = 0;
                        if (second == null)
                            continue;
                    }

                    if (firstOffset == first._data.Length)
                    {
                        first._count = firstOffset;
                        first = first.Next;
                        firstOffset = 0;
                    }
                }

                return changedAnything;
            }

            internal override void Validate(ValidationRules rules)
            {
                Debug.Assert(_data != null, $"Assertion failed: {nameof(_data)} != null");
                Debug.Assert(_data.Length >= 2, $"Assertion failed: {nameof(_data.Length)} >= 2");
                Debug.Assert(_count >= 0 && _count <= _data.Length, $"Assertion failed: {nameof(_count)} >= 0 && {nameof(_count)} <= {nameof(_data)}.Length");

                // Only the last node is allowed to have a reduced number of children
                Debug.Assert(_count >= (_data.Length + 1) / 2 || _next == null, $"Assertion failed: {nameof(_count)} >= ({nameof(_data)}.Length + 1) / 2 || {nameof(_next)} == null");

                if (default(T) == null)
                {
                    for (int i = _count; i < _data.Length; i++)
                        Debug.Assert(_data[i] == null, $"Assertion failed: {nameof(_data)}[i] == null");
                }

                if (rules.HasFlag(ValidationRules.RequirePacked))
                {
                    Debug.Assert(_next == null || _count == _data.Length, $"Assertion failed: {nameof(_next)} == null || {nameof(_count)} == {nameof(_data)}.Length");
                }
            }
        }
    }
}
