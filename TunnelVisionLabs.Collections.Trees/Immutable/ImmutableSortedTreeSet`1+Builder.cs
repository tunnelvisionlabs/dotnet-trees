// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public partial class ImmutableSortedTreeSet<T>
    {
        public sealed class Builder : ISet<T>, IReadOnlyCollection<T>, ICollection
        {
            private ImmutableSortedTreeSet<T> _immutableSet;
            private readonly ImmutableSortedTreeList<T>.Builder _sortedList;

            internal Builder(ImmutableSortedTreeSet<T> immutableSet)
            {
                _immutableSet = immutableSet;
                _sortedList = immutableSet._sortedList.ToBuilder();
            }

            public IComparer<T> KeyComparer => _immutableSet.KeyComparer;

            public int Count => _sortedList.Count;

            [MaybeNull]
            public T Max => Count > 0 ? this[Count - 1] : default;

            [MaybeNull]
            public T Min => Count > 0 ? this[0] : default;

            bool ICollection<T>.IsReadOnly => false;

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot => this;

            public T this[int index] => _sortedList[index];

            public bool Add(T item)
                => _sortedList.Add(item, addIfPresent: false);

            public void Clear()
                => _sortedList.Clear();

            public bool Contains(T item)
                => TryGetValue(item, out _);

            public void ExceptWith(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                {
                    return;
                }

                if (other == this)
                {
                    Clear();
                    return;
                }

                foreach (T item in other)
                {
                    Remove(item);
                }
            }

            public Enumerator GetEnumerator()
                => new Enumerator(_sortedList.GetEnumerator());

            public void IntersectWith(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                    return;

                if (this == other)
                    return;

                if (other is ImmutableSortedTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    int i = 0;
                    int j = 0;
                    while (i < Count && j < set.Count)
                    {
                        int comparison = KeyComparer.Compare(_sortedList[i], set._sortedList[j]);
                        if (comparison == 0)
                        {
                            // Keep the item
                            i++;
                            j++;
                        }
                        else if (comparison < 0)
                        {
                            _sortedList.RemoveAt(i);
                        }
                        else
                        {
                            // The item may be present, but it's not the current item
                            j++;
                        }
                    }

                    _sortedList.RemoveRange(i, Count - i);
                }
                else if (other is Builder builder && KeyComparer.Equals(builder.KeyComparer))
                {
                    IntersectWith(builder.ToImmutable());
                }
                else
                {
                    var toSave = ImmutableSortedTreeSet.Create(KeyComparer);
                    foreach (T item in other)
                    {
                        if (Contains(item))
                            toSave = toSave.Add(item);
                    }

                    IntersectWith(toSave);
                }
            }

            public bool IsProperSubsetOf(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                    return other.Any();

                if (other is ImmutableSortedTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    if (Count >= set.Count)
                        return false;

                    foreach (T item in this)
                    {
                        if (!set.Contains(item))
                            return false;
                    }

                    return true;
                }
                else if (other is Builder builder && KeyComparer.Equals(builder.KeyComparer))
                {
                    if (Count >= builder.Count)
                        return false;

                    foreach (T item in this)
                    {
                        if (!builder.Contains(item))
                            return false;
                    }

                    return true;
                }

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, other, returnIfUnfound: false);
                return uniqueCount == Count && unfoundCount > 0;
            }

            public bool IsProperSupersetOf(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                    return false;

                if (this == other)
                    return false;

                if (other is ICollection collection && collection.Count == 0)
                {
                    Debug.Assert(Count > 0, $"Assertion failed: {nameof(Count)} > 0");
                    return true;
                }

                if (other is ImmutableSortedTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    if (set.Count >= Count)
                        return false;

                    foreach (T item in set)
                    {
                        if (!Contains(item))
                            return false;
                    }

                    return true;
                }
                else if (other is Builder builder && KeyComparer.Equals(builder.KeyComparer))
                {
                    if (builder.Count >= Count)
                        return false;

                    foreach (T item in builder)
                    {
                        if (!Contains(item))
                            return false;
                    }

                    return true;
                }

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, other, returnIfUnfound: true);
                return uniqueCount < Count && unfoundCount == 0;
            }

            public bool IsSubsetOf(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                    return true;

                if (other is ImmutableSortedTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    if (Count > set.Count)
                        return false;

                    foreach (T item in this)
                    {
                        if (!set.Contains(item))
                            return false;
                    }

                    return true;
                }
                else if (other is Builder builder && KeyComparer.Equals(builder.KeyComparer))
                {
                    if (Count > builder.Count)
                        return false;

                    foreach (T item in this)
                    {
                        if (!builder.Contains(item))
                            return false;
                    }

                    return true;
                }

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, other, returnIfUnfound: false);
                return uniqueCount == Count && unfoundCount >= 0;
            }

            public bool IsSupersetOf(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (this == other)
                    return true;

                if (other is ICollection collection && collection.Count == 0)
                    return true;

                if (other is ImmutableSortedTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    if (Count < set.Count)
                        return false;

                    foreach (T item in set)
                    {
                        if (!Contains(item))
                            return false;
                    }

                    return true;
                }
                else if (other is Builder builder && KeyComparer.Equals(builder.KeyComparer))
                {
                    if (Count < builder.Count)
                        return false;

                    foreach (T item in builder)
                    {
                        if (!Contains(item))
                            return false;
                    }

                    return true;
                }

                foreach (T item in other)
                {
                    if (!Contains(item))
                        return false;
                }

                return true;
            }

            public bool Overlaps(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                {
                    return false;
                }

                foreach (T item in other)
                {
                    if (Contains(item))
                        return true;
                }

                return false;
            }

            public bool Remove(T item)
                => _sortedList.Remove(item);

            public IEnumerable<T> Reverse()
                => _sortedList.Reverse();

            public bool SetEquals(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (this == other)
                    return true;

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, other, returnIfUnfound: true);
                return uniqueCount == Count && unfoundCount == 0;
            }

            public void SymmetricExceptWith(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                {
                    UnionWith(other);
                    return;
                }

                if (other == this)
                {
                    Clear();
                    return;
                }

                if (other is ImmutableSortedTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    foreach (T item in other)
                    {
                        if (!Remove(item))
                            Add(item);
                    }

                    return;
                }
                else if (other is Builder builder && KeyComparer.Equals(builder.KeyComparer))
                {
                    foreach (T item in other)
                    {
                        if (!Remove(item))
                            Add(item);
                    }

                    return;
                }

                set = ImmutableSortedTreeSet.CreateRange(KeyComparer, other);
                SymmetricExceptWith(set);
            }

            public void UnionWith(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                foreach (T item in other)
                {
                    Add(item);
                }
            }

            internal void TrimExcess()
                => _sortedList.TrimExcess();

            internal bool TryGetValue(T equalValue, [MaybeNullWhen(false)] out T actualValue)
            {
                int index = _sortedList.BinarySearch(equalValue);
                if (index < 0)
                {
                    actualValue = default;
                    return false;
                }

                actualValue = _sortedList[index];
                return true;
            }

            public ImmutableSortedTreeSet<T> ToImmutable()
            {
                ImmutableSortedTreeList<T> sortedList = _sortedList.ToImmutable();
                if (_immutableSet._sortedList == sortedList)
                    return _immutableSet;

                _immutableSet = new ImmutableSortedTreeSet<T>(sortedList);
                return _immutableSet;
            }

            void ICollection<T>.Add(T item)
                => Add(item);

            void ICollection<T>.CopyTo(T[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                if (array.Length - arrayIndex < Count)
                    throw new ArgumentException("Not enough space is available in the destination array.", string.Empty);

                ICollection<T> collection = _sortedList;
                collection.CopyTo(array, arrayIndex);
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
                => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            void ICollection.CopyTo(Array array, int index)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (array.Rank != 1)
                    throw new ArgumentException("Only single dimensional arrays are supported for the requested action.", nameof(array));
                if (array.GetLowerBound(0) != 0)
                    throw new ArgumentException();
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                if (array.Length - index < Count)
                    throw new ArgumentException("Not enough space is available in the destination array.", nameof(index));

                ICollection collection = _sortedList;

                try
                {
                    collection.CopyTo(array, index);
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("Invalid array type");
                }
            }

            internal void Validate(ValidationRules validationRules)
            {
                _sortedList.Validate(validationRules);
            }
        }
    }
}
