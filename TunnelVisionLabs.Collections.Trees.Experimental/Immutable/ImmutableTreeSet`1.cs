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
        public static readonly ImmutableTreeSet<T> Empty = new ImmutableTreeSet<T>(ImmutableSortedTreeList.Create(comparer: SetHelper.WrapperComparer<T>.Instance), EqualityComparer<T>.Default);

        private readonly IEqualityComparer<T> _comparer;
        private readonly ImmutableSortedTreeList<(int hashCode, T value)> _sortedList;

        private ImmutableTreeSet(ImmutableSortedTreeList<(int hashCode, T value)> sortedList, IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
            _sortedList = sortedList;
        }

        public IEqualityComparer<T> KeyComparer => _comparer;

        public int Count => _sortedList.Count;

        public bool IsEmpty => _sortedList.IsEmpty;

        bool ICollection<T>.IsReadOnly => true;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => this;

        public ImmutableTreeSet<T> Add(T value)
        {
            Builder builder = ToBuilder();
            builder.Add(value);
            return builder.ToImmutable();
        }

        public ImmutableTreeSet<T> Clear()
            => Empty.WithComparer(KeyComparer);

        public bool Contains(T value)
            => ToBuilder().Contains(value);

        public ImmutableTreeSet<T> Except(IEnumerable<T> other)
        {
            Builder builder = ToBuilder();
            builder.ExceptWith(other);
            return builder.ToImmutable();
        }

        public Enumerator GetEnumerator()
            => new Enumerator(_sortedList.GetEnumerator());

        public ImmutableTreeSet<T> Intersect(IEnumerable<T> other)
        {
            Builder builder = ToBuilder();
            builder.IntersectWith(other);
            return builder.ToImmutable();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
            => ToBuilder().IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other)
            => ToBuilder().IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other)
            => ToBuilder().IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other)
            => ToBuilder().IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other)
            => ToBuilder().Overlaps(other);

        public ImmutableTreeSet<T> Remove(T value)
        {
            Builder builder = ToBuilder();
            builder.Remove(value);
            return builder.ToImmutable();
        }

        public bool SetEquals(IEnumerable<T> other)
            => ToBuilder().SetEquals(other);

        public ImmutableTreeSet<T> SymmetricExcept(IEnumerable<T> other)
        {
            Builder builder = ToBuilder();
            builder.SymmetricExceptWith(other);
            return builder.ToImmutable();
        }

        public bool TryGetValue(T equalValue, out T actualValue)
            => ToBuilder().TryGetValue(equalValue, out actualValue);

        public ImmutableTreeSet<T> Union(IEnumerable<T> other)
        {
            var builder = ToBuilder();
            builder.UnionWith(other);
            return builder.ToImmutable();
        }

        public Builder ToBuilder()
            => new Builder(this);

        public ImmutableTreeSet<T> WithComparer(IEqualityComparer<T> equalityComparer)
        {
            equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            if (equalityComparer == _comparer)
                return this;

            if (IsEmpty)
            {
                if (equalityComparer == Empty._comparer)
                    return Empty;
                else
                    return new ImmutableTreeSet<T>(Empty._sortedList, equalityComparer);
            }

            return ImmutableTreeSet.CreateRange(equalityComparer, this);
        }

        IImmutableSet<T> IImmutableSet<T>.Clear()
            => Clear();

        IImmutableSet<T> IImmutableSet<T>.Add(T value)
            => Add(value);

        IImmutableSet<T> IImmutableSet<T>.Remove(T value)
            => Remove(value);

        IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other)
            => Intersect(other);

        IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other)
            => Except(other);

        IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other)
            => SymmetricExcept(other);

        IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other)
            => Union(other);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            ICollection<T> collection = ToBuilder();
            collection.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection collection = ToBuilder();
            collection.CopyTo(array, index);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

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
