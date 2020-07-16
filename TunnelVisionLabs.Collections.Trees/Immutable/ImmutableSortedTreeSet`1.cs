// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableSortedTreeSet<T> : IImmutableSet<T>, ISet<T>, IList<T>, IReadOnlyList<T>, IList
    {
        public static readonly ImmutableSortedTreeSet<T> Empty = new ImmutableSortedTreeSet<T>(ImmutableSortedTreeList.Create<T>(Comparer<T>.Default));

        private readonly ImmutableSortedTreeList<T> _sortedList;

        private ImmutableSortedTreeSet(ImmutableSortedTreeList<T> sortedList)
        {
            _sortedList = sortedList;
        }

        public IComparer<T> KeyComparer => _sortedList.Comparer;

        public int Count => _sortedList.Count;

        public bool IsEmpty => _sortedList.IsEmpty;

        public T Max => !IsEmpty ? this[Count - 1] : default;

        public T Min => !IsEmpty ? this[0] : default;

        bool ICollection<T>.IsReadOnly => true;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => this;

        bool IList.IsFixedSize => true;

        bool IList.IsReadOnly => true;

        public T this[int index] => _sortedList[index];

        T IList<T>.this[int index]
        {
            get => _sortedList[index];
            set => throw new NotSupportedException();
        }

        object IList.this[int index]
        {
            get => _sortedList[index];
            set => throw new NotSupportedException();
        }

        public ImmutableSortedTreeSet<T> Add(T value)
        {
            Builder builder = ToBuilder();
            builder.Add(value);
            return builder.ToImmutable();
        }

        public ImmutableSortedTreeSet<T> Clear()
        {
            if (IsEmpty)
                return this;

            return Empty.WithComparer(KeyComparer);
        }

        public bool Contains(T value)
            => ToBuilder().Contains(value);

        public ImmutableSortedTreeSet<T> Except(IEnumerable<T> other)
        {
            Builder builder = ToBuilder();
            builder.ExceptWith(other);
            return builder.ToImmutable();
        }

        public Enumerator GetEnumerator()
            => new Enumerator(_sortedList.GetEnumerator());

        public int IndexOf(T item)
            => _sortedList.IndexOf(item);

        public ImmutableSortedTreeSet<T> Intersect(IEnumerable<T> other)
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

        public ImmutableSortedTreeSet<T> Remove(T value)
        {
            Builder builder = ToBuilder();
            builder.Remove(value);
            return builder.ToImmutable();
        }

        public IEnumerable<T> Reverse()
            => _sortedList.Reverse();

        public bool SetEquals(IEnumerable<T> other)
            => ToBuilder().SetEquals(other);

        public ImmutableSortedTreeSet<T> SymmetricExcept(IEnumerable<T> other)
        {
            Builder builder = ToBuilder();
            builder.SymmetricExceptWith(other);
            return builder.ToImmutable();
        }

        public bool TryGetValue(T equalValue, out T actualValue)
            => ToBuilder().TryGetValue(equalValue, out actualValue);

        public ImmutableSortedTreeSet<T> Union(IEnumerable<T> other)
        {
            var builder = ToBuilder();
            builder.UnionWith(other);
            return builder.ToImmutable();
        }

        public Builder ToBuilder()
            => new Builder(this);

        public ImmutableSortedTreeSet<T> WithComparer(IComparer<T> comparer)
        {
            comparer = comparer ?? Comparer<T>.Default;
            if (comparer == _sortedList.Comparer)
                return this;

            if (IsEmpty)
            {
                if (comparer == Empty.KeyComparer)
                    return Empty;
                else
                    return new ImmutableSortedTreeSet<T>(Empty._sortedList.WithComparer(comparer));
            }

            return ImmutableSortedTreeSet.CreateRange(comparer, this);
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

        bool IList.Contains(object value)
        {
            IList sortedList = _sortedList;
            return sortedList.Contains(value);
        }

        int IList.IndexOf(object value)
        {
            IList sortedList = _sortedList;
            return sortedList.IndexOf(value);
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

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        internal void Validate(ValidationRules validationRules)
        {
            _sortedList.Validate(validationRules);
        }
    }
}
