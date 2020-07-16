// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableTreeStack<T> : IImmutableStack<T>
    {
        public static readonly ImmutableTreeStack<T> Empty = new ImmutableTreeStack<T>(ImmutableTreeList<T>.Empty);

        private readonly ImmutableTreeList<T> _treeList;

        private ImmutableTreeStack(ImmutableTreeList<T> treeList)
        {
            _treeList = treeList;
        }

        public bool IsEmpty
            => _treeList.IsEmpty;

        public ImmutableTreeStack<T> Clear()
            => Empty;

        public Enumerator GetEnumerator()
            => new Enumerator(_treeList.GetEnumerator());

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            return _treeList[0];
        }

        public ImmutableTreeStack<T> Pop()
            => Pop(out _);

        public ImmutableTreeStack<T> Pop(out T value)
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            value = _treeList[0];
            return new ImmutableTreeStack<T>(_treeList.RemoveAt(0));
        }

        public ImmutableTreeStack<T> Push(T value)
            => new ImmutableTreeStack<T>(_treeList.Insert(0, value));

        IImmutableStack<T> IImmutableStack<T>.Clear()
            => Clear();

        IImmutableStack<T> IImmutableStack<T>.Pop()
            => Pop();

        IImmutableStack<T> IImmutableStack<T>.Push(T value)
            => Push(value);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        internal void Validate(ValidationRules validationRules)
        {
            _treeList.Validate(validationRules);
        }
    }
}
