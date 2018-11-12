// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableSortedTreeSet<T> : IImmutableSet<T>, ISet<T>, IList<T>, IReadOnlyList<T>, IList
    {
        public static readonly ImmutableSortedTreeSet<T> Empty;

        public IComparer<T> KeyComparer => throw null;

        public int Count => throw null;

        public bool IsEmpty => throw null;

        public T Max => throw null;

        public T Min => throw null;

        bool ICollection<T>.IsReadOnly => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

        bool IList.IsFixedSize => throw null;

        bool IList.IsReadOnly => throw null;

        public T this[int index] => throw null;

        T IList<T>.this[int index]
        {
            get => throw null;
            set => throw null;
        }

        object IList.this[int index]
        {
            get => throw null;
            set => throw null;
        }

        public ImmutableSortedTreeSet<T> Add(T value) => throw null;

        public ImmutableSortedTreeSet<T> Clear() => throw null;

        public bool Contains(T value) => throw null;

        public ImmutableSortedTreeSet<T> Except(IEnumerable<T> other) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public int IndexOf(T item) => throw null;

        public ImmutableSortedTreeSet<T> Intersect(IEnumerable<T> other) => throw null;

        public bool IsProperSubsetOf(IEnumerable<T> other) => throw null;

        public bool IsProperSupersetOf(IEnumerable<T> other) => throw null;

        public bool IsSubsetOf(IEnumerable<T> other) => throw null;

        public bool IsSupersetOf(IEnumerable<T> other) => throw null;

        public bool Overlaps(IEnumerable<T> other) => throw null;

        public ImmutableSortedTreeSet<T> Remove(T value) => throw null;

        public IEnumerable<T> Reverse() => throw null;

        public bool SetEquals(IEnumerable<T> other) => throw null;

        public ImmutableSortedTreeSet<T> SymmetricExcept(IEnumerable<T> other) => throw null;

        public bool TryGetValue(T equalValue, out T actualValue) => throw null;

        public ImmutableSortedTreeSet<T> Union(IEnumerable<T> other) => throw null;

        public Builder ToBuilder() => throw null;

        public ImmutableSortedTreeSet<T> WithComparer(IComparer<T> comparer) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Clear() => throw null;

        IImmutableSet<T> IImmutableSet<T>.Add(T value) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Remove(T value) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other) => throw null;

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other) => throw null;

        IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other) => throw null;

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;

        bool IList.Contains(object value) => throw null;

        int IList.IndexOf(object value) => throw null;

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

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();
    }
}
