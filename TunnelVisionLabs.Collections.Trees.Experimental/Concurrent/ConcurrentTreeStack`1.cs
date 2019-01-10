// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Concurrent
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public partial class ConcurrentTreeStack<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
    {
        public ConcurrentTreeStack() => throw null!;

        public ConcurrentTreeStack(IEnumerable<T> collection) => throw null!;

        public int Count => throw null!;

        public bool IsEmpty => throw null!;

        bool ICollection.IsSynchronized => throw null!;

        object ICollection.SyncRoot => throw null!;

        public void Clear() => throw null!;

        public void CopyTo(T[] array, int index) => throw null!;

        public Enumerator GetEnumerator() => throw null!;

        public void Push(T item) => throw null!;

        public void PushRange(T[] items) => throw null!;

        public void PushRange(T[] items, int startIndex, int count) => throw null!;

        public T[] ToArray() => throw null!;

        public bool TryPeek([MaybeNullWhen(false)] out T result) => throw null!;

        public bool TryPop([MaybeNullWhen(false)] out T result) => throw null!;

        public int TryPopRange(T[] items) => throw null!;

        public int TryPopRange(T[] items, int startIndex, int count) => throw null!;

        void ICollection.CopyTo(Array array, int index) => throw null!;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null!;

        IEnumerator IEnumerable.GetEnumerator() => throw null!;

        bool IProducerConsumerCollection<T>.TryAdd(T item) => throw null!;

        bool IProducerConsumerCollection<T>.TryTake([MaybeNullWhen(false)] out T item) => throw null!;
    }
}
