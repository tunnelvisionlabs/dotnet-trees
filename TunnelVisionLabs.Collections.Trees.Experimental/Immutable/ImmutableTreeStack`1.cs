// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableTreeStack<T> : IImmutableStack<T>
    {
        public static ImmutableTreeStack<T> Empty => throw null;

        public bool IsEmpty => throw null;

        public ImmutableTreeStack<T> Clear() => throw null;

        public Enumerator GetEnumerator() => throw null;

        public T Peek() => throw null;

        public ImmutableTreeStack<T> Pop() => throw null;

        public ImmutableTreeStack<T> Pop(out T value) => throw null;

        public ImmutableTreeStack<T> Push(T value) => throw null;

        IImmutableStack<T> IImmutableStack<T>.Clear() => throw null;

        IImmutableStack<T> IImmutableStack<T>.Pop() => throw null;

        IImmutableStack<T> IImmutableStack<T>.Push(T value) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;
    }
}
