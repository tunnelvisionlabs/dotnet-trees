// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableTreeQueue<T> : IImmutableQueue<T>
    {
        public static ImmutableTreeQueue<T> Empty => throw null;

        public bool IsEmpty => throw null;

        public ImmutableTreeQueue<T> Clear() => throw null;

        public ImmutableTreeQueue<T> Dequeue() => throw null;

        public ImmutableTreeQueue<T> Dequeue(out T value) => throw null;

        public ImmutableTreeQueue<T> Enqueue(T value) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public T Peek() => throw null;

        IImmutableQueue<T> IImmutableQueue<T>.Clear() => throw null;

        IImmutableQueue<T> IImmutableQueue<T>.Dequeue() => throw null;

        IImmutableQueue<T> IImmutableQueue<T>.Enqueue(T value) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;
    }
}
