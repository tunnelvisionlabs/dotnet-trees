// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableTreeSet<T> : IImmutableSet<T>, ISet<T>, ICollection
    {
        public static readonly ImmutableTreeSet<T> Empty;

        public IEqualityComparer<T> KeyComparer => throw null;

        public int Count => throw null;

        public bool IsEmpty => throw null;

        bool ICollection<T>.IsReadOnly => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

        public ImmutableTreeSet<T> Add(T value) => throw null;

        public ImmutableTreeSet<T> Clear() => throw null;

        public bool Contains(T value) => throw null;

        public ImmutableTreeSet<T> Except(IEnumerable<T> other) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public ImmutableTreeSet<T> Intersect(IEnumerable<T> other) => throw null;

        public bool IsProperSubsetOf(IEnumerable<T> other) => throw null;

        public bool IsProperSupersetOf(IEnumerable<T> other) => throw null;

        public bool IsSubsetOf(IEnumerable<T> other) => throw null;

        public bool IsSupersetOf(IEnumerable<T> other) => throw null;

        public bool Overlaps(IEnumerable<T> other) => throw null;

        public ImmutableTreeSet<T> Remove(T value) => throw null;

        public bool SetEquals(IEnumerable<T> other) => throw null;

        public ImmutableTreeSet<T> SymmetricExcept(IEnumerable<T> other) => throw null;

        public bool TryGetValue(T equalValue, out T actualValue) => throw null;

        public ImmutableTreeSet<T> Union(IEnumerable<T> other) => throw null;

        public Builder ToBuilder() => throw null;

        public ImmutableTreeSet<T> WithComparer(IEqualityComparer<T> equalityComparer) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Clear() => throw null;

        IImmutableSet<T> IImmutableSet<T>.Add(T value) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Remove(T value) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other) => throw null;

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other) => throw null;

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        bool ISet<T>.Add(T item) => throw new NotSupportedException();

        void ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ICollection<T>.Add(T item) => throw new NotSupportedException();

        void ICollection<T>.Clear() => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
    }
}
