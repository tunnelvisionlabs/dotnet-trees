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
            private int _version;

            private LeafNode _leafNode;
            private int _index;
            private T _current;

            public Enumerator(TreeList<T> list)
            {
                _list = list;
                _version = list._version;
                _leafNode = null;
                _index = -1;
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
                    // Need to get the first leaf node
                    _leafNode = _list._root.FirstLeaf;
                }
                else if (_index == _leafNode.Count - 1)
                {
                    // Need to move to the next leaf
                    _leafNode = _leafNode.Next;
                    _index = -1;
                }

                if (_leafNode == null)
                {
                    _index = int.MinValue;
                    return false;
                }

                _index++;
                _current = _leafNode[_index];
                return true;
            }

            public void Reset()
            {
                _version = _list._version;
                _leafNode = null;
                _index = -1;
            }
        }
    }
}
