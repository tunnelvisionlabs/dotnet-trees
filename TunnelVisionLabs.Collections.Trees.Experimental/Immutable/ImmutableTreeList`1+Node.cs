// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    public partial class ImmutableTreeList<T>
    {
        private abstract class Node
        {
            internal static readonly Node Empty = new EmptyNode();

            internal TreeSpan Span => new TreeSpan(0, Count);

            internal abstract int Count
            {
                get;
            }

            internal abstract int NodeCount
            {
                get;
            }

            internal abstract LeafNode FirstLeaf
            {
                get;
            }

            internal abstract Node FirstChild
            {
                get;
            }

            internal abstract bool IsFrozen
            {
                get;
            }

            internal abstract T this[int index]
            {
                get;
            }

            internal abstract void Freeze();

            internal abstract Node SetItem(int index, T value);

            internal static Node Insert(Node root, int index, T item)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index > root.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                (Node newRoot, Node splitNode) = root.Insert(index == root.Count, index, item);
                if (splitNode == null)
                    return newRoot;

                Debug.Assert(newRoot != null, $"Assertion failed: {nameof(newRoot)} != null");
                Debug.Assert(splitNode != null, $"Assertion failed: {nameof(splitNode)} != null");
                Debug.Assert(newRoot.Count + splitNode.Count > default(FixedArray8<int>).Length, $"Assertion failed: {nameof(newRoot)}.Count + {nameof(splitNode)}.Count > default(FixedArray8<int>).Length");
                return new IndexNode(newRoot, splitNode);
            }

            internal static Node InsertRange(Node root, int index, IEnumerable<T> collection)
            {
                if (collection == null)
                    throw new ArgumentNullException(nameof(collection));
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index > root.Count)
                    throw new ArgumentOutOfRangeException();

                // We can't insert a range into the empty node
                if (root == Empty)
                    root = new LeafNode();

                ImmutableTreeList<Node>.Node splitNode = root.InsertRange(index == root.Count, index, collection);
                while (splitNode != null)
                {
                    if (splitNode.Count == 1)
                    {
                        root = splitNode[0];
                        break;
                    }
                    else
                    {
                        // Make a new level, walking nodes on the previous root level from 'node' to 'splitNode'
                        IndexNode newRoot = new IndexNode(splitNode, out splitNode);
                        root = newRoot;
                    }
                }

                return root;
            }

            internal static Node RemoveAt(Node root, int index)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (index >= root.Count)
                    throw new ArgumentOutOfRangeException();

                if (index == root.Count - 1)
                {
                    if (index == 0)
                        return Empty;

                    root = root.RemoveLast();
                }
                else
                {
                    (root, _) = root.RemoveAt(index, null);
                }

                while (root.FirstChild != null && root.NodeCount == 1)
                {
                    root = root.FirstChild;
                }

                return root;
            }

            internal static Node RemoveRange(Node root, int index, int count)
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (count < 0)
                    throw new ArgumentOutOfRangeException(nameof(count));
                if (index > root.Count - count)
                    throw new ArgumentException();

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
                for (int i = 0; i < root.Count; i++)
                {
                    if (match(root[i]))
                    {
                        root = RemoveAt(root, i);
                        i--;
                    }
                }

                return root;
            }

            internal static Node TrimExcess(Node root)
            {
                (Node newRoot, Node mustBeNull) = root.TrimExcessImpl(null);
                Debug.Assert(mustBeNull == null, $"Assertion failed: {nameof(mustBeNull)} == null");
                while (newRoot.FirstChild != null && newRoot.NodeCount == 1)
                {
                    newRoot = newRoot.FirstChild;
                }

                return newRoot;
            }

            internal abstract (LeafNode leafNode, int offset) GetLeafNode(int index);

            internal Node Reverse(TreeSpan span)
            {
                Node newNode = this;
                int firstIndex = span.Start;
                int lastIndex = firstIndex + span.Count - 1;
                while (lastIndex > firstIndex)
                {
                    T temp = newNode[firstIndex];
                    newNode = newNode.SetItem(firstIndex, newNode[lastIndex]);
                    newNode = newNode.SetItem(lastIndex, temp);
                    firstIndex++;
                    lastIndex--;
                }

                return newNode;
            }

            internal abstract int IndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer);

            internal abstract int LastIndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer);

            internal abstract (Node currentNode, Node splitNode) Insert(bool isAppend, int index, T item);

            internal abstract ImmutableTreeList<Node>.Node InsertRange(bool isAppend, int index, IEnumerable<T> collection);

            internal abstract Node RemoveLast();

            internal abstract (Node currentNode, Node nextNode) RemoveAt(int index, Node nextNode);

            internal abstract Node Sort(TreeSpan span, IComparer<T> comparer);

            internal abstract int FindIndex(TreeSpan span, Predicate<T> match);

            internal abstract int FindLastIndex(TreeSpan span, Predicate<T> match);

            internal abstract int BinarySearch(TreeSpan span, T item, IComparer<T> comparer);

            internal ImmutableTreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter)
            {
                return ConvertAll(converter, null);
            }

            internal abstract ImmutableTreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, ImmutableTreeList<TOutput>.Node convertedNextNode);

            internal abstract (Node currentNode, Node nextNode) TrimExcessImpl(Node nextNode);

            internal abstract void Validate(ValidationRules rules, Node nextNode);

            private sealed class EmptyNode : Node
            {
                internal override int Count => 0;

                internal override int NodeCount => 0;

                internal override LeafNode FirstLeaf => null;

                internal override Node FirstChild => null;

                internal override bool IsFrozen => true;

                [ExcludeFromCodeCoverage]
                internal override T this[int index]
                {
                    get
                    {
                        throw ExceptionUtilities.Unreachable;
                    }
                }

                internal override void Freeze()
                {
                }

                [ExcludeFromCodeCoverage]
                internal override Node SetItem(int index, T value)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                [ExcludeFromCodeCoverage]
                internal override (LeafNode leafNode, int offset) GetLeafNode(int index)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                internal override int IndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer)
                {
                    Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");

                    return -1;
                }

                [ExcludeFromCodeCoverage]
                internal override int LastIndexOf(T item, TreeSpan span, IEqualityComparer<T> equalityComparer)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                internal override (Node currentNode, Node splitNode) Insert(bool isAppend, int index, T item)
                {
                    Debug.Assert(index == 0 && isAppend, "index == 0 && isAppend");
                    return new LeafNode().Insert(isAppend, index, item);
                }

                [ExcludeFromCodeCoverage]
                internal override ImmutableTreeList<Node>.Node InsertRange(bool isAppend, int index, IEnumerable<T> collection)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                [ExcludeFromCodeCoverage]
                internal override Node RemoveLast()
                {
                    throw ExceptionUtilities.Unreachable;
                }

                [ExcludeFromCodeCoverage]
                internal override (Node currentNode, Node nextNode) RemoveAt(int index, Node nextNode)
                {
                    throw ExceptionUtilities.Unreachable;
                }

                internal override Node Sort(TreeSpan span, IComparer<T> comparer)
                {
                    Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                    Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                    return this;
                }

                internal override int FindIndex(TreeSpan span, Predicate<T> match)
                {
                    Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                    Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                    return -1;
                }

                internal override int FindLastIndex(TreeSpan span, Predicate<T> match)
                {
                    Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                    Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

                    return -1;
                }

                internal override int BinarySearch(TreeSpan span, T item, IComparer<T> comparer)
                {
                    Debug.Assert(span.IsSubspanOf(Span), $"Assertion failed: {nameof(span)}.IsSubspanOf({nameof(Span)})");
                    Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

                    return ~0;
                }

                internal override ImmutableTreeList<TOutput>.Node ConvertAll<TOutput>(Func<T, TOutput> converter, ImmutableTreeList<TOutput>.Node convertedNextNode)
                {
                    return ImmutableTreeList<TOutput>.Node.Empty;
                }

                internal override (Node currentNode, Node nextNode) TrimExcessImpl(Node nextNode)
                {
                    Debug.Assert(nextNode == null, $"Assertion failed: {nameof(nextNode)} == null");

                    return (this, null);
                }

                internal override void Validate(ValidationRules rules, Node nextNode)
                {
                    Debug.Assert(this == Empty, $"Assertion failed: this == {nameof(Empty)}");
                    Debug.Assert(nextNode == null, $"Assertion failed: {nameof(nextNode)} == null");
                }
            }
        }
    }
}
