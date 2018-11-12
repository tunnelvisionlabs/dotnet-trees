// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public sealed partial class ImmutableTreeList<T> : IImmutableList<T>, IReadOnlyList<T>, IList<T>, IList
    {
        public static readonly ImmutableTreeList<T> Empty;

        private ImmutableTreeList() => throw null;

        public int Count => throw null;

        public bool IsEmpty => throw null;

        bool ICollection<T>.IsReadOnly => throw null;

        bool IList.IsFixedSize => throw null;

        bool IList.IsReadOnly => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

        public T this[int index] => throw null;

        T IList<T>.this[int index]
        {
            get => throw null;
            set => throw new NotSupportedException();
        }

        object IList.this[int index]
        {
            get => throw null;
            set => throw new NotSupportedException();
        }

        public ImmutableTreeList<T> Add(T value) => throw null;

        public ImmutableTreeList<T> AddRange(IEnumerable<T> items) => throw null;

        public int BinarySearch(T item) => throw null;

        public int BinarySearch(T item, IComparer<T> comparer) => throw null;

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer) => throw null;

        public ImmutableTreeList<T> Clear() => throw null;

        public bool Contains(T value) => throw null;

        public ImmutableTreeList<TOutput> ConvertAll<TOutput>(Func<T, TOutput> converter) => throw null;

        public void CopyTo(T[] array) => throw null;

        public void CopyTo(T[] array, int arrayIndex) => throw null;

        public void CopyTo(int index, T[] array, int arrayIndex, int count) => throw null;

        public bool Exists(Predicate<T> match) => throw null;

        public T Find(Predicate<T> match) => throw null;

        public ImmutableTreeList<T> FindAll(Predicate<T> match) => throw null;

        public int FindIndex(Predicate<T> match) => throw null;

        public int FindIndex(int startIndex, Predicate<T> match) => throw null;

        public int FindIndex(int startIndex, int count, Predicate<T> match) => throw null;

        public T FindLast(Predicate<T> match) => throw null;

        public int FindLastIndex(Predicate<T> match) => throw null;

        public int FindLastIndex(int startIndex, Predicate<T> match) => throw null;

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => throw null;

        public void ForEach(Action<T> action) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public ImmutableTreeList<T> GetRange(int index, int count) => throw null;

        public int IndexOf(T value) => throw null;

        public int IndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> Insert(int index, T item) => throw null;

        public ImmutableTreeList<T> InsertRange(int index, IEnumerable<T> items) => throw null;

        public int LastIndexOf(T item, int index, int count, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> Remove(T value) => throw null;

        public ImmutableTreeList<T> Remove(T value, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> RemoveAll(Predicate<T> match) => throw null;

        public ImmutableTreeList<T> RemoveAt(int index) => throw null;

        public ImmutableTreeList<T> RemoveRange(IEnumerable<T> items) => throw null;

        public ImmutableTreeList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> RemoveRange(int index, int count) => throw null;

        public ImmutableTreeList<T> Replace(T oldValue, T newValue) => throw null;

        public ImmutableTreeList<T> Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) => throw null;

        public ImmutableTreeList<T> Reverse() => throw null;

        public ImmutableTreeList<T> Reverse(int index, int count) => throw null;

        public ImmutableTreeList<T> SetItem(int index, T value) => throw null;

        public ImmutableTreeList<T> Sort() => throw null;

        public ImmutableTreeList<T> Sort(IComparer<T> comparer) => throw null;

        public ImmutableTreeList<T> Sort(Comparison<T> comparison) => throw null;

        public ImmutableTreeList<T> Sort(int index, int count, IComparer<T> comparer) => throw null;

        public Builder ToBuilder() => throw null;

        public bool TrueForAll(Predicate<T> match) => throw null;

        IImmutableList<T> IImmutableList<T>.Clear() => throw null;

        IImmutableList<T> IImmutableList<T>.Add(T value) => throw null;

        IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => throw null;

        IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => throw null;

        IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => throw null;

        IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T> equalityComparer) => throw null;

        IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => throw null;

        IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T> equalityComparer) => throw null;

        IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => throw null;

        IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => throw null;

        IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => throw null;

        IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T> equalityComparer) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection<T>.Add(T item) => throw new NotSupportedException();

        void ICollection<T>.Clear() => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value) => throw null;

        int IList.IndexOf(object value) => throw null;

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => throw null;
    }
}
