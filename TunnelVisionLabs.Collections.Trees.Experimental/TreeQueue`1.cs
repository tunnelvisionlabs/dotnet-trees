﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ICollection = System.Collections.ICollection;

    public partial class TreeQueue<T> : IReadOnlyCollection<T>, ICollection
    {
        public TreeQueue() => throw null;

        public TreeQueue(int branchingFactor) => throw null;

        public int Count => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

        public void Clear() => throw null;

        public bool Contains(T item) => throw null;

        public void CopyTo(T[] array, int arrayIndex) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public T Peek() => throw null;

        public T Dequeue() => throw null;

        public void Enqueue(T item) => throw null;

        public T[] ToArray() => throw null;

        public void TrimExcess() => throw null;

        public bool TryPeek(out T result) => throw null;

        public bool TryDequeue(out T result) => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;
    }
}