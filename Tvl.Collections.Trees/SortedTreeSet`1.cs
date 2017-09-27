// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class SortedTreeSet<T> : ISet<T>, IReadOnlyCollection<T>, ICollection
    {
        public SortedTreeSet() => throw null;

        public SortedTreeSet(IEnumerable<T> collection) => throw null;

        public SortedTreeSet(IComparer<T> comparer) => throw null;

        public SortedTreeSet(IEnumerable<T> collection, IComparer<T> comparer) => throw null;

        public SortedTreeSet(int branchingFactor) => throw null;

        public SortedTreeSet(int branchingFactor, IComparer<T> comparer) => throw null;

        public SortedTreeSet(int branchingFactor, IEnumerable<T> collection, IComparer<T> comparer) => throw null;

        public IComparer<T> Comparer => throw null;

        public int Count => throw null;

        public T Max => throw null;

        public T Min => throw null;

        bool ICollection<T>.IsReadOnly => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

        public static IEqualityComparer<SortedTreeSet<T>> CreateSetComparer() => throw null;

        public static IEqualityComparer<SortedTreeSet<T>> CreateSetComparer(IEqualityComparer<T> memberEqualityComparer) => throw null;

        public bool Add(T item) => throw null;

        public void Clear() => throw null;

        public bool Contains(T item) => throw null;

        public void CopyTo(T[] array) => throw null;

        public void CopyTo(T[] array, int arrayIndex) => throw null;

        public void CopyTo(T[] array, int arrayIndex, int count) => throw null;

        public void ExceptWith(IEnumerable<T> other) => throw null;

        public SortedTreeSet<T> GetViewBetween(T lowerValue, T upperValue) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public void IntersectWith(IEnumerable<T> other) => throw null;

        public bool IsProperSubsetOf(IEnumerable<T> other) => throw null;

        public bool IsProperSupersetOf(IEnumerable<T> other) => throw null;

        public bool IsSubsetOf(IEnumerable<T> other) => throw null;

        public bool IsSupersetOf(IEnumerable<T> other) => throw null;

        public bool Overlaps(IEnumerable<T> other) => throw null;

        public bool Remove(T item) => throw null;

        public int RemoveWhere(Predicate<T> match) => throw null;

        public bool SetEquals(IEnumerable<T> other) => throw null;

        public void SymmetricExceptWith(IEnumerable<T> other) => throw null;

        public void UnionWith(IEnumerable<T> other) => throw null;

        public void TrimExcess() => throw null;

        public bool TryGetValue(T equalValue, out T actualValue) => throw null;

        void ICollection<T>.Add(T item) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;
    }
}
