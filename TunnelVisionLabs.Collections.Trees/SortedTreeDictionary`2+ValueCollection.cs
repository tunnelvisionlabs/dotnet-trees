// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class SortedTreeDictionary<TKey, TValue>
    {
        public partial struct ValueCollection : ICollection<TValue>, IReadOnlyCollection<TValue>, ICollection
        {
            private readonly SortedTreeDictionary<TKey, TValue> _dictionary;

            internal ValueCollection(SortedTreeDictionary<TKey, TValue> dictionary)
            {
                Debug.Assert(dictionary != null, $"Assertion failed: {nameof(dictionary)} != null");
                _dictionary = dictionary;
            }

            public int Count => _dictionary.Count;

            bool ICollection<TValue>.IsReadOnly => false;

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();

            public void Clear() => _dictionary.Clear();

            public bool Contains(TValue item) => _dictionary.ContainsValue(item);

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0 || arrayIndex > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                if (array.Length - arrayIndex < _dictionary.Count)
                    throw new ArgumentException();

                int i = arrayIndex;
                foreach (TValue value in this)
                {
                    array[i] = value;
                    i++;
                }
            }

            public Enumerator GetEnumerator() => new Enumerator(_dictionary.GetEnumerator());

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();

            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (array.Rank != 1)
                    throw new ArgumentException();
                if (array.GetLowerBound(0) != 0)
                    throw new ArgumentException();
                if (index < 0 || index > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (array.Length - index < _dictionary.Count)
                    throw new ArgumentException();

                if (array is TValue[] values)
                {
                    CopyTo(values, index);
                }
                else if (array is object?[] objects)
                {
                    try
                    {
                        int i = index;
                        foreach (TValue value in this)
                        {
                            objects[i] = value;
                            i++;
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException();
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
