// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Concurrent
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class ConcurrentTreeQueue<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
    {
        public ConcurrentTreeQueue() => throw null!;

        public ConcurrentTreeQueue(IEnumerable<T> collection) => throw null!;

        public int Count => throw null!;

        public bool IsEmpty => throw null!;

        bool ICollection.IsSynchronized => throw null!;

        object ICollection.SyncRoot => throw null!;

        public void Clear() => throw null!;

        public void CopyTo(T[] array, int index) => throw null!;

        public void Enqueue(T item) => throw null!;

        public Enumerator GetEnumerator() => throw null!;

        public T[] ToArray() => throw null!;

        public bool TryDequeue([MaybeNullWhen(false)] out T result) => throw null!;

        public bool TryPeek([MaybeNullWhen(false)] out T result) => throw null!;

        void ICollection.CopyTo(Array array, int index) => throw null!;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null!;

        IEnumerator IEnumerable.GetEnumerator() => throw null!;

        bool IProducerConsumerCollection<T>.TryAdd(T item) => throw null!;

        bool IProducerConsumerCollection<T>.TryTake([MaybeNullWhen(false)] out T item) => throw null!;
    }
}
