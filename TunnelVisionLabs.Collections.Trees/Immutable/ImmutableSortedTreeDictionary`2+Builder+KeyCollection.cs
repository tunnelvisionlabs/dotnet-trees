// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class ImmutableSortedTreeDictionary<TKey, TValue>
    {
        public partial class Builder
        {
            public struct KeyCollection : IReadOnlyCollection<TKey>, ICollection<TKey>, ICollection
            {
                private readonly ImmutableSortedTreeDictionary<TKey, TValue>.Builder _dictionary;

                internal KeyCollection(ImmutableSortedTreeDictionary<TKey, TValue>.Builder dictionary)
                {
                    Debug.Assert(dictionary != null, $"Assertion failed: {nameof(dictionary)} != null");
                    _dictionary = dictionary;
                }

                public int Count => _dictionary.Count;

                bool ICollection<TKey>.IsReadOnly => false;

                bool ICollection.IsSynchronized => false;

                object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

                void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

                public void Clear() => _dictionary.Clear();

                public bool Contains(TKey item) => _dictionary.ContainsKey(item);

                public void CopyTo(TKey[] array, int arrayIndex)
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

                public ImmutableSortedTreeDictionary<TKey, TValue>.KeyCollection.Enumerator GetEnumerator()
                    => new ImmutableSortedTreeDictionary<TKey, TValue>.KeyCollection.Enumerator(_dictionary.GetEnumerator());

                public bool Remove(TKey item)
                    => _dictionary.Remove(item);

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
                        CopyTo(keys, index);
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

                IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => GetEnumerator();

                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            }
        }
    }
}
