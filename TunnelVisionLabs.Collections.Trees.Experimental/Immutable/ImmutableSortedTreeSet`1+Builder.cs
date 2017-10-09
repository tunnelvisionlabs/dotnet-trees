// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableSortedTreeSet<T>
    {
        public sealed class Builder : ISet<T>, IReadOnlyCollection<T>, ICollection
        {
            public IComparer<T> KeyComparer => throw null;

            public int Count => throw null;

            public T Min => throw null;

            public T Max => throw null;

            bool ICollection<T>.IsReadOnly => throw null;

            bool ICollection.IsSynchronized => throw null;

            object ICollection.SyncRoot => throw null;

            public T this[int index] => throw null;

            public bool Add(T item) => throw null;

            public void Clear() => throw null;

            public bool Contains(T item) => throw null;

            public void ExceptWith(IEnumerable<T> other) => throw null;

            public Enumerator GetEnumerator() => throw null;

            public void IntersectWith(IEnumerable<T> other) => throw null;

            public bool IsProperSubsetOf(IEnumerable<T> other) => throw null;

            public bool IsProperSupersetOf(IEnumerable<T> other) => throw null;

            public bool IsSubsetOf(IEnumerable<T> other) => throw null;

            public bool IsSupersetOf(IEnumerable<T> other) => throw null;

            public bool Overlaps(IEnumerable<T> other) => throw null;

            public bool Remove(T item) => throw null;

            public IEnumerable<T> Reverse() => throw null;

            public bool SetEquals(IEnumerable<T> other) => throw null;

            public void SymmetricExceptWith(IEnumerable<T> other) => throw null;

            public void UnionWith(IEnumerable<T> other) => throw null;

            public ImmutableSortedTreeSet<T> ToImmutable() => throw null;

            void ICollection<T>.Add(T item) => throw null;

            void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw null;

            IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

            IEnumerator IEnumerable.GetEnumerator() => throw null;

            void ICollection.CopyTo(Array array, int index) => throw null;
        }
    }
}
