// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeList<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private readonly Node _root;
            private readonly TreeSpan _span;

            private readonly Builder _builder;
            private readonly int _version;

            private int _index;
            private LeafNode _leafNode;
            private int _leafIndex;
            private T _current;

            internal Enumerator(ImmutableTreeList<T> list)
                : this(list, list._root.Span, builder: null)
            {
            }

            internal Enumerator(ImmutableTreeList<T> list, Builder builder)
                : this(list, list._root.Span, builder)
            {
            }

            internal Enumerator(ImmutableTreeList<T> list, TreeSpan span, Builder builder)
            {
                _root = list._root;
                _span = span;
                _builder = builder;
                _version = builder?.Version ?? 0;
                _index = -1;
                _leafNode = null;
                _leafIndex = -1;
                _current = default;
            }

            public T Current => _current;

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_builder != null && _builder.Version != _version)
                    throw new InvalidOperationException();

                if (_index < -1)
                {
                    // Past the end of the list.
                    return false;
                }

                if (_index == -1)
                {
                    if (_span.IsEmpty)
                    {
                        _index = int.MinValue;
                        return false;
                    }

                    // Need to get the first leaf node
                    (_leafNode, _leafIndex) = _root.GetLeafNode(_span.Start);

                    // The index and leaf index will be incremented below; want the correct final result
                    _index = _span.Start - 1;
                    _leafIndex--;
                }
                else if (_leafIndex == _leafNode.Count - 1)
                {
                    if (_index == _root.Count - 1)
                    {
                        _leafNode = null;
                        _leafIndex = -1;
                    }
                    else
                    {
                        // Need to move to the next leaf
                        (_leafNode, _leafIndex) = _root.GetLeafNode(_index + 1);

                        // The leaf index will be incremented below; want the correct final result
                        _leafIndex--;
                    }
                }

                _index++;
                if (_index == _span.EndExclusive)
                {
                    _index = int.MinValue;
                    return false;
                }

                _leafIndex++;
                _current = _leafNode[_leafIndex];
                return true;
            }

            public void Reset()
            {
                if (_builder != null && _builder.Version != _version)
                    throw new InvalidOperationException();

                _leafNode = null;
                _index = -1;
                _leafIndex = -1;
                _current = default;
            }
        }
    }
}
