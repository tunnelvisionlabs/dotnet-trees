// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ICollection = System.Collections.ICollection;

    public partial class TreeQueue<T> : IReadOnlyCollection<T>, ICollection
    {
        private readonly TreeList<T> _treeList;

        public TreeQueue()
        {
            _treeList = new TreeList<T>();
        }

        public TreeQueue(int branchingFactor)
        {
            _treeList = new TreeList<T>(branchingFactor);
        }

        public int Count => _treeList.Count;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => ((ICollection)_treeList).SyncRoot;

        public void Clear() => _treeList.Clear();

        public bool Contains(T item) => _treeList.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _treeList.CopyTo(array, arrayIndex);

        public Enumerator GetEnumerator() => new Enumerator(_treeList.GetEnumerator());

        public T Peek()
        {
            if (!TryPeek(out T result))
                throw new InvalidOperationException();

            return result;
        }

        public T Dequeue()
        {
            if (!TryDequeue(out T result))
                throw new InvalidOperationException();

            return result;
        }

        public void Enqueue(T item) => _treeList.Add(item);

        public T[] ToArray() => _treeList.ToArray();

        public void TrimExcess() => _treeList.TrimExcess();

        public bool TryPeek(out T result)
        {
            if (_treeList.Count == 0)
            {
                result = default;
                return false;
            }

            result = _treeList[0];
            return true;
        }

        public bool TryDequeue(out T result)
        {
            if (_treeList.Count == 0)
            {
                result = default;
                return false;
            }

            result = _treeList[0];
            _treeList.RemoveAt(0);
            return true;
        }

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_treeList).CopyTo(array, index);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
