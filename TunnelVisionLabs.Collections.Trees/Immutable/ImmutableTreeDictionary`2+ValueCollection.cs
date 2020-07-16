// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public partial class ImmutableTreeDictionary<TKey, TValue>
    {
        public readonly partial struct ValueCollection : IReadOnlyCollection<TValue>, ICollection<TValue>, ICollection
        {
            private readonly ImmutableTreeDictionary<TKey, TValue> _dictionary;

            internal ValueCollection(ImmutableTreeDictionary<TKey, TValue> dictionary)
            {
                Debug.Assert(dictionary != null, $"Assertion failed: {nameof(dictionary)} != null");
                _dictionary = dictionary;
            }

            public int Count => _dictionary.Count;

            bool ICollection<TValue>.IsReadOnly => true;

            bool ICollection.IsSynchronized => true;

            object ICollection.SyncRoot => ((ICollection)_dictionary).SyncRoot;

            public Enumerator GetEnumerator() => new Enumerator(_dictionary.GetEnumerator());

            public bool Contains(TValue item) => _dictionary.ContainsValue(item);

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
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
                    ((ICollection<TValue>)this).CopyTo(values, index);
                }
                else if (array is object[] objects)
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

            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();

            void ICollection<TValue>.Clear() => throw new NotSupportedException();

            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();
        }
    }
}
