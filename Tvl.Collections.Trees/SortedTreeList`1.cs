// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace Tvl.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class SortedTreeList<T> : IList<T>, IReadOnlyList<T>, IList
    {
        public SortedTreeList() => throw null;

        public SortedTreeList(IEnumerable<T> collection) => throw null;

        public SortedTreeList(IComparer<T> comparer) => throw null;

        public SortedTreeList(IEnumerable<T> collection, IComparer<T> comparer) => throw null;

        public SortedTreeList(int branchingFactor) => throw null;

        public SortedTreeList(int branchingFactor, IComparer<T> comparer) => throw null;

        public SortedTreeList(int branchingFactor, IEnumerable<T> collection, IComparer<T> comparer) => throw null;

        public IComparer<T> Comparer => throw null;

        public int Count => throw null;

        bool ICollection<T>.IsReadOnly => throw null;

        bool IList.IsReadOnly => throw null;

        bool IList.IsFixedSize => throw null;

        bool ICollection.IsSynchronized => throw null;

        object ICollection.SyncRoot => throw null;

        public T this[int index]
        {
            get => throw null;
        }

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

        public int BinarySearch(T item) => throw null;

        public int BinarySearch(int index, int count, T item) => throw null;

        public int IndexOf(T item) => throw null;

        public int IndexOf(T item, int index) => throw null;

        public int IndexOf(T item, int index, int count) => throw null;

        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();

        public bool Remove(T item) => throw null;

        public void RemoveAt(int index) => throw null;

        public void RemoveRange(int index, int count) => throw null;

        public int RemoveAll(Predicate<T> match) => throw null;

        public void Add(T item) => throw null;

        public void AddRange(IEnumerable<T> collection) => throw null;

        public void Clear() => throw null;

        public bool Contains(T item) => throw null;

        public void CopyTo(T[] array) => throw null;

        public void CopyTo(T[] array, int arrayIndex) => throw null;

        public void CopyTo(int index, T[] array, int arrayIndex, int count) => throw null;

        public Enumerator GetEnumerator() => throw null;

        public bool Exists(Predicate<T> match) => throw null;

        public T Find(Predicate<T> match) => throw null;

        public SortedTreeList<T> FindAll(Predicate<T> match) => throw null;

        public int FindIndex(Predicate<T> match) => throw null;

        public int FindIndex(int startIndex, Predicate<T> match) => throw null;

        public int FindIndex(int startIndex, int count, Predicate<T> match) => throw null;

        public T FindLast(Predicate<T> match) => throw null;

        public int FindLastIndex(Predicate<T> match) => throw null;

        public int FindLastIndex(int startIndex, Predicate<T> match) => throw null;

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => throw null;

        public int LastIndexOf(T item) => throw null;

        public int LastIndexOf(T item, int index) => throw null;

        public int LastIndexOf(T item, int index, int count) => throw null;

        public void ForEach(Action<T> action) => throw null;

        public SortedTreeList<T> GetRange(int index, int count) => throw null;

        public T[] ToArray() => throw null;

        public void TrimExcess() => throw null;

        public bool TrueForAll(Predicate<T> match) => throw null;

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => throw null;

        IEnumerator IEnumerable.GetEnumerator() => throw null;

        int IList.Add(object value) => throw null;

        bool IList.Contains(object value) => throw null;

        int IList.IndexOf(object value) => throw null;

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw null;

        void ICollection.CopyTo(Array array, int index) => throw null;
    }
}
