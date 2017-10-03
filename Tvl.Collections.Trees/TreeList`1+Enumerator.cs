// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class TreeList<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private readonly TreeList<T> _list;
            private readonly TreeSpan _span;
            private readonly int _version;

            private int _index;
            private LeafNode _leafNode;
            private int _leafIndex;
            private T _current;

            internal Enumerator(TreeList<T> list)
                : this(list, list._root.Span)
            {
            }

            internal Enumerator(TreeList<T> list, TreeSpan span)
            {
                _list = list;
                _span = span;
                _version = list._version;
                _index = -1;
                _leafNode = null;
                _leafIndex = -1;
                _current = default;
            }

            public T Current
            {
                get
                {
                    if (_index < 0)
                        throw new InvalidOperationException();

                    return _current;
                }
            }

            object IEnumerator.Current => Current;

            void IDisposable.Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < -1)
                {
                    // Past the end of the list.
                    return false;
                }

                if (_list._version != _version)
                    throw new InvalidOperationException();

                if (_index == -1)
                {
                    if (_span.IsEmpty)
                    {
                        _index = int.MinValue;
                        return false;
                    }

                    // Need to get the first leaf node
                    (_leafNode, _leafIndex) = _list._root.GetLeafNode(_span.Start);

                    // The leaf index will be incremented below; want the correct final result
                    _leafIndex--;
                }
                else if (_leafIndex == _leafNode.Count - 1)
                {
                    // Need to move to the next leaf
                    _leafNode = _leafNode.Next;
                    _leafIndex = -1;
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
                _leafNode = null;
                _index = -1;
                _leafIndex = -1;
                _current = default;
            }
        }
    }
}
