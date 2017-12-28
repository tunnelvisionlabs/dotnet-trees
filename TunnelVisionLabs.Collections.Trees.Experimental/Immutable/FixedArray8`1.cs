// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal struct FixedArray8<T>
    {
        private T _item0;
        private T _item1;
        private T _item2;
        private T _item3;
        private T _item4;
        private T _item5;
        private T _item6;
        private T _item7;

        public int Length => 8;

        public T this[int index]
        {
            get
            {
                switch (index)
                {
                case 0:
                    return _item0;

                case 1:
                    return _item1;

                case 2:
                    return _item2;

                case 3:
                    return _item3;

                case 4:
                    return _item4;

                case 5:
                    return _item5;

                case 6:
                    return _item6;

                case 7:
                    return _item7;

                default:
                    throw new IndexOutOfRangeException();
                }
            }

            set
            {
                switch (index)
                {
                case 0:
                    _item0 = value;
                    break;

                case 1:
                    _item1 = value;
                    break;

                case 2:
                    _item2 = value;
                    break;

                case 3:
                    _item3 = value;
                    break;

                case 4:
                    _item4 = value;
                    break;

                case 5:
                    _item5 = value;
                    break;

                case 6:
                    _item6 = value;
                    break;

                case 7:
                    _item7 = value;
                    break;

                default:
                    throw new IndexOutOfRangeException();
                }
            }
        }

        internal int IndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer)
        {
            Debug.Assert(startIndex >= 0, $"Assertion failed: {nameof(startIndex)} >= 0");
            Debug.Assert(Length - startIndex >= count, $"Assertion failed: {nameof(Length)} - {nameof(startIndex)} >= {nameof(count)}");
            Debug.Assert(equalityComparer != null, $"Assertion failed: {nameof(equalityComparer)} != null");

            for (int i = 0; i < count; i++)
            {
                if (equalityComparer.Equals(item, this[i + startIndex]))
                    return i + startIndex;
            }

            return -1;
        }

        internal int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            Debug.Assert(startIndex >= 0, $"Assertion failed: {nameof(startIndex)} >= 0");
            Debug.Assert(Length - startIndex >= count, $"Assertion failed: {nameof(Length)} - {nameof(startIndex)} >= {nameof(count)}");
            Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

            for (int i = 0; i < count; i++)
            {
                if (match(this[i + startIndex]))
                    return i + startIndex;
            }

            return -1;
        }

        internal int LastIndexOf(T item, int startIndex, int count, IEqualityComparer<T> equalityComparer)
        {
            Debug.Assert(startIndex >= 0, $"Assertion failed: {nameof(startIndex)} >= 0");
            Debug.Assert(count >= 0, $"Assertion failed: {nameof(count)} >= 0");
            Debug.Assert((uint)startIndex < (uint)Length, $"Assertion failed: (uint){nameof(startIndex)} < (uint){nameof(Length)}");
            Debug.Assert(count >= 0 && startIndex - count + 1 >= 0, $"Assertion failed: {nameof(count)} >= 0 && {nameof(startIndex)} - {nameof(count)} + 1 >= 0");
            Debug.Assert(equalityComparer != null, $"Assertion failed: {nameof(equalityComparer)} != null");

            for (int i = 0; i < count; i++)
            {
                if (equalityComparer.Equals(item, this[startIndex - i]))
                    return startIndex - i;
            }

            return -1;
        }

        internal int FindLastIndex(int startIndex, int length, Predicate<T> match)
        {
            Debug.Assert(startIndex >= 0, $"Assertion failed: {nameof(startIndex)} >= 0");
            Debug.Assert(length >= 0, $"Assertion failed: {nameof(length)} >= 0");
            Debug.Assert((uint)startIndex < (uint)Length, $"Assertion failed: (uint){nameof(startIndex)} < (uint){nameof(Length)}");
            Debug.Assert(length >= 0 && startIndex - length + 1 >= 0, $"Assertion failed: {nameof(length)} >= 0 && {nameof(startIndex)} - {nameof(length)} + 1 >= 0");
            Debug.Assert(match != null, $"Assertion failed: {nameof(match)} != null");

            for (int i = 0; i < length; i++)
            {
                if (match(this[startIndex - i]))
                    return startIndex - i;
            }

            return -1;
        }

        internal void Copy(ref FixedArray8<T> destinationArray, int count)
        {
            Copy(0, ref destinationArray, 0, count);
        }

        internal void Copy(int sourceIndex, ref FixedArray8<T> destinationArray, int destinationIndex, int count)
        {
            if (destinationIndex > sourceIndex)
            {
                // Copy in reverse order just in case the source and destination arrays are the same instance
                for (int i = count - 1; i >= 0; i--)
                {
                    destinationArray[i + destinationIndex] = this[i + sourceIndex];
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    destinationArray[i + destinationIndex] = this[i + sourceIndex];
                }
            }
        }

        internal void Copy(int sourceIndex, Array destinationArray, int destinationIndex, int count)
        {
            if (destinationArray is T[] array)
            {
                for (int i = 0; i < count; i++)
                {
                    array[i + destinationIndex] = this[i + sourceIndex];
                }
            }
            else
            {
                try
                {
                    for (int i = 0; i < count; i++)
                    {
                        destinationArray.SetValue(this[i + sourceIndex], i + destinationIndex);
                    }
                }
                catch (InvalidCastException)
                {
                    throw new ArgumentException("Invalid array type");
                }
            }
        }

        internal void Clear(int index, int length)
        {
            for (int i = 0; i < length; i++)
            {
                this[i + index] = default;
            }
        }

        internal int BinarySearch(int index, int length, T value) => BinarySearch(index, length, value, comparer: Comparer<T>.Default);

        internal int BinarySearch(int index, int length, T value, IComparer<T> comparer)
        {
            Debug.Assert(index >= 0 && length >= 0 && (Length - index >= length), "Assertion failed: index >= 0 && length >= 0 && (Length - index >= length)");
            Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

            int lo = index;
            int hi = index + length - 1;
            while (lo <= hi)
            {
                int i = lo + ((hi - lo) >> 1);
                int order = comparer.Compare(this[i], value);

                if (order == 0)
                    return i;

                if (order < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return ~lo;
        }

        internal void Sort(int index, int length, IComparer<T> comparer)
        {
            Debug.Assert(index >= 0, $"Assertion failed: {nameof(index)} >= 0");
            Debug.Assert(Length - index >= length, $"Assertion failed: {nameof(Length)} - {nameof(index)} >= {nameof(length)}");
            Debug.Assert(comparer != null, $"Assertion failed: {nameof(comparer)} != null");

            if (length < 2)
            {
                // No work to do
                return;
            }
            else if (length == 2)
            {
                SwapIfGreater(ref this, index, index + 1);
            }
            else if (length == 3)
            {
                SwapIfGreater(ref this, index, index + 1);
                SwapIfGreater(ref this, index, index + 2);
                SwapIfGreater(ref this, index + 1, index + 2);
            }
            else
            {
                InsertionSort(ref this, index, index + length - 1);
            }

            void SwapIfGreater(ref FixedArray8<T> keys, int i, int j)
            {
                if (comparer.Compare(keys[i], keys[j]) > 0)
                {
                    T temp = keys[i];
                    keys[i] = keys[j];
                    keys[j] = temp;
                }
            }

            void InsertionSort(ref FixedArray8<T> keys, int lo, int hi)
            {
                int i, j;
                T t;
                for (i = lo; i < hi; i++)
                {
                    j = i;
                    t = keys[i + 1];
                    while (j >= lo && comparer.Compare(t, keys[j]) < 0)
                    {
                        keys[j + 1] = keys[j];
                        j--;
                    }

                    keys[j + 1] = t;
                }
            }
        }
    }
}
