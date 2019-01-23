// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class ImmutableTreeDictionary<TKey, TValue>
    {
        public readonly partial struct KeyCollection : IReadOnlyCollection<TKey>, ICollection<TKey>, ICollection
        {
            private readonly ImmutableTreeDictionary<TKey, TValue> _dictionary;

            internal KeyCollection(ImmutableTreeDictionary<TKey, TValue> dictionary)
            {
                Debug.Assert(dictionary != null, $"Assertion failed: {nameof(dictionary)} != null");
                _dictionary = dictionary;
            }

            public int Count => _dictionary.Count;

            bool ICollection<TKey>.IsReadOnly => true;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

            public Enumerator GetEnumerator() => new Enumerator(_dictionary.GetEnumerator());

            public bool Contains(TKey item) => _dictionary.ContainsKey(item);

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0 || arrayIndex > array.Length)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                if (array.Length - arrayIndex < _dictionary.Count)
                    throw new ArgumentException();

                int i = arrayIndex;
                foreach (TKey key in this)
                {
                    array[i] = key;
                    i++;
                }
            }

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

                if (array is TKey[] keys)
                {
                    ((ICollection<TKey>)this).CopyTo(keys, index);
                }
                else if (array is object[] objects)
                {
                    try
                    {
                        int i = index;
                        foreach (TKey key in this)
                        {
                            objects[i] = key;
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

            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

            void ICollection<TKey>.Clear() => throw new NotSupportedException();

            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException();
        }
    }
}
