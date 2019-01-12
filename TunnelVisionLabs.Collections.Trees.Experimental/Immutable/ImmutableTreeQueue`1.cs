// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableTreeQueue<T> : IImmutableQueue<T>
    {
        public static readonly ImmutableTreeQueue<T> Empty = new ImmutableTreeQueue<T>(ImmutableTreeList<T>.Empty);

        private readonly ImmutableTreeList<T> _treeList;

        private ImmutableTreeQueue(ImmutableTreeList<T> treeList)
        {
            _treeList = treeList;
        }

        public bool IsEmpty
            => _treeList.IsEmpty;

        public ImmutableTreeQueue<T> Clear()
            => Empty;

        public ImmutableTreeQueue<T> Dequeue()
            => Dequeue(out _);

        public ImmutableTreeQueue<T> Dequeue(out T value)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            value = _treeList[0];
            return new ImmutableTreeQueue<T>(_treeList.RemoveAt(0));
        }

        public ImmutableTreeQueue<T> Enqueue(T value)
            => new ImmutableTreeQueue<T>(_treeList.Add(value));

        public Enumerator GetEnumerator()
            => new Enumerator(_treeList.GetEnumerator());

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            return _treeList[0];
        }

        IImmutableQueue<T> IImmutableQueue<T>.Clear()
            => Clear();

        IImmutableQueue<T> IImmutableQueue<T>.Dequeue()
            => Dequeue();

        IImmutableQueue<T> IImmutableQueue<T>.Enqueue(T value)
            => Enqueue(value);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
