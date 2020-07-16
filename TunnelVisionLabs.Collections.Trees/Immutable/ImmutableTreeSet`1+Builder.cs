// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public partial class ImmutableTreeSet<T>
    {
        public sealed class Builder : ISet<T>, IReadOnlyCollection<T>, ICollection
        {
            private ImmutableTreeSet<T> _immutableSet;
            private readonly ImmutableSortedTreeList<(int hashCode, T value)>.Builder _sortedList;

            internal Builder(ImmutableTreeSet<T> immutableSet)
            {
                _immutableSet = immutableSet;
                _sortedList = immutableSet._sortedList.ToBuilder();
            }

            public IEqualityComparer<T> KeyComparer => _immutableSet.KeyComparer;

            public int Count => _sortedList.Count;

            bool ICollection<T>.IsReadOnly => false;

            bool ICollection.IsSynchronized => false;

            object ICollection.SyncRoot => this;

            public bool Add(T item)
            {
                if (Contains(item))
                    return false;

                _sortedList.Add((KeyComparer.GetHashCode(item), item));
                return true;
            }

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

                if (other is ImmutableTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
                {
                    int i = 0;
                    int j = 0;
                    while (i < Count && j < set.Count)
                    {
                        (int hashCode, T value) left = _sortedList[i];
                        (int hashCode, T value) right = set._sortedList[j];

                        if (left.hashCode == right.hashCode)
                        {
                            if (KeyComparer.Equals(left.value, right.value))
                            {
                                i++;
                                j++;
                            }
                            else
                            {
                                // Need to compare all items with the same hash code
                                bool foundMatch = false;
                                for (int k = j + 1; k < set.Count; k++)
                                {
                                    (int hashCode, T value) altRight = set._sortedList[k];
                                    if (altRight.hashCode != left.hashCode)
                                        break;

                                    if (KeyComparer.Equals(left.value, altRight.value))
                                    {
                                        foundMatch = true;
                                        break;
                                    }
                                }

                                if (foundMatch)
                                {
                                    // Only increment i since set._sortedList[j] might match _sortedList[i+1]
                                    i++;
                                }
                                else
                                {
                                    _sortedList.RemoveAt(i);
                                }
                            }
                        }
                        else if (left.hashCode < right.hashCode)
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
                    var toSave = ImmutableTreeSet.Create(KeyComparer);
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

                if (other is ImmutableTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
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

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, KeyComparer, other, returnIfUnfound: false);
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

                if (other is ImmutableTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
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

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, KeyComparer, other, returnIfUnfound: true);
                return uniqueCount < Count && unfoundCount == 0;
            }

            public bool IsSubsetOf(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (Count == 0)
                    return true;

                if (other is ImmutableTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
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

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, KeyComparer, other, returnIfUnfound: false);
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

                if (other is ImmutableTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
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
            {
                int hashCode = KeyComparer.GetHashCode(item);

                // Find the index of the first item with this hash code
                int index = _sortedList.IndexOf((hashCode, item));
                if (index >= 0)
                {
                    // Find the item if it exists
                    for (int i = index; i < Count; i++)
                    {
                        (int hashCode, T value) bucket = _sortedList[i];
                        if (bucket.hashCode != hashCode)
                            return false;

                        if (KeyComparer.Equals(bucket.value, item))
                        {
                            _sortedList.RemoveAt(i);
                            return true;
                        }
                    }
                }

                return false;
            }

            public bool SetEquals(IEnumerable<T> other)
            {
                if (other == null)
                    throw new ArgumentNullException(nameof(other));

                if (this == other)
                    return true;

                (int uniqueCount, int unfoundCount) = SetHelper.CheckUniqueAndUnfoundElements(_sortedList, KeyComparer, other, returnIfUnfound: true);
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

                if (other is ImmutableTreeSet<T> set && KeyComparer.Equals(set.KeyComparer))
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

                set = ImmutableTreeSet.CreateRange(KeyComparer, other);
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

            internal bool TryGetValue(T equalValue, out T actualValue)
            {
                int hashCode = KeyComparer.GetHashCode(equalValue);

                // Find the index of the first item with this hash code
                int index = _sortedList.IndexOf((hashCode, equalValue));
                if (index >= 0)
                {
                    // Find the duplicate value if it exists
                    for (int i = index; i < Count; i++)
                    {
                        (int hashCode, T value) bucket = _sortedList[i];
                        if (bucket.hashCode != hashCode)
                            break;

                        if (KeyComparer.Equals(bucket.value, equalValue))
                        {
                            actualValue = bucket.value;
                            return true;
                        }
                    }
                }

                actualValue = default;
                return false;
            }

            public ImmutableTreeSet<T> ToImmutable()
            {
                ImmutableSortedTreeList<(int hashCode, T value)> sortedList = _sortedList.ToImmutable();
                if (_immutableSet._sortedList == sortedList)
                    return _immutableSet;

                _immutableSet = new ImmutableTreeSet<T>(sortedList, _immutableSet._comparer);
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

                for (int i = 0; i < Count; i++)
                {
                    array[arrayIndex + i] = _sortedList[i].value;
                }
            }

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

                try
                {
                    for (int i = 0; i < Count; i++)
                    {
                        array.SetValue(_sortedList[i].value, i + index);
                    }
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("Invalid array type");
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
                => GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            internal void Validate(ValidationRules validationRules)
            {
                _sortedList.Validate(validationRules);
            }
        }
    }
}
