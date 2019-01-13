// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    internal static class SetHelper
    {
        internal static (int uniqueCount, int unfoundCount) CheckUniqueAndUnfoundElements<T>(IList<(int hashCode, T value)> sortedList, IEqualityComparer<T> comparer, IEnumerable<T> other, bool returnIfUnfound)
        {
            if (sortedList.Count == 0)
            {
                if (other.Any())
                    return (uniqueCount: 0, unfoundCount: 1);

                return (uniqueCount: 0, unfoundCount: 0);
            }

            const int StackAllocThreshold = 100;
            int originalLastIndex = sortedList.Count;
            int intArrayLength = ((originalLastIndex - 1) / 32) + 1;
            Span<int> span = intArrayLength <= StackAllocThreshold
                ? stackalloc int[intArrayLength]
                : new int[intArrayLength];
            BitHelper bitHelper = new BitHelper(span);

            // count of items in other not found in this
            int unfoundCount = 0;

            // count of unique items in other found in this
            int uniqueCount = 0;

            foreach (T item in other)
            {
                int hashCode = comparer.GetHashCode(item);

                int index = sortedList.IndexOf((hashCode, item));
                if (index >= 0)
                {
                    // Find the duplicate value if it exists
                    for (int i = index; i < sortedList.Count; i++)
                    {
                        (int hashCode, T value) bucket = sortedList[i];
                        if (bucket.hashCode != hashCode)
                        {
                            index = -1;
                            break;
                        }

                        if (comparer.Equals(bucket.value, item))
                        {
                            index = i;
                            break;
                        }

                        // Fast path didn't match the item of interest
                        index = -1;
                    }
                }

                if (index >= 0)
                {
                    if (!bitHelper.IsMarked(index))
                    {
                        bitHelper.MarkBit(index);
                        uniqueCount++;
                    }
                }
                else
                {
                    unfoundCount++;
                    if (returnIfUnfound)
                        return (uniqueCount, unfoundCount);
                }
            }

            return (uniqueCount, unfoundCount);
        }

        internal static (int uniqueCount, int unfoundCount) CheckUniqueAndUnfoundElements<T>(IList<T> sortedList, IEnumerable<T> other, bool returnIfUnfound)
        {
            if (sortedList.Count == 0)
            {
                if (other.Any())
                    return (uniqueCount: 0, unfoundCount: 1);

                return (uniqueCount: 0, unfoundCount: 0);
            }

            const int StackAllocThreshold = 100;
            int originalLastIndex = sortedList.Count;
            int intArrayLength = ((originalLastIndex - 1) / 32) + 1;
            Span<int> span = intArrayLength <= StackAllocThreshold
                ? stackalloc int[intArrayLength]
                : new int[intArrayLength];
            BitHelper bitHelper = new BitHelper(span);

            // count of items in other not found in this
            int unfoundCount = 0;

            // count of unique items in other found in this
            int uniqueCount = 0;

            foreach (T item in other)
            {
                int index = sortedList.IndexOf(item);
                if (index >= 0)
                {
                    if (!bitHelper.IsMarked(index))
                    {
                        bitHelper.MarkBit(index);
                        uniqueCount++;
                    }
                }
                else
                {
                    unfoundCount++;
                    if (returnIfUnfound)
                        return (uniqueCount, unfoundCount);
                }
            }

            return (uniqueCount, unfoundCount);
        }

        private ref struct BitHelper
        {
            private readonly Span<int> _span;

            public BitHelper(Span<int> span)
            {
                _span = span;
            }

            internal void MarkBit(int bitPosition)
            {
                Debug.Assert(bitPosition >= 0, $"Assertion failed: {nameof(bitPosition)} >= 0");

                int bitArrayIndex = bitPosition / 32;
                Debug.Assert(bitArrayIndex < _span.Length, $"Assertion failed: {nameof(bitArrayIndex)} < {nameof(_span)}.Length");

                // Note: Using (bitPosition & 31) instead of (bitPosition % 32)
                _span[bitArrayIndex] |= 1 << (bitPosition & 31);
            }

            internal bool IsMarked(int bitPosition)
            {
                Debug.Assert(bitPosition >= 0, $"Assertion failed: {nameof(bitPosition)} >= 0");

                int bitArrayIndex = bitPosition / 32;
                Debug.Assert(bitArrayIndex < _span.Length, $"Assertion failed: {nameof(bitArrayIndex)} < {nameof(_span)}.Length");

                // Note: Using (bitPosition & 31) instead of (bitPosition % 32)
                return (_span[bitArrayIndex] & (1 << (bitPosition & 31))) != 0;
            }
        }

        internal sealed class WrapperComparer<T> : IComparer<(int hashCode, T value)>
        {
            internal static readonly WrapperComparer<T> Instance = new WrapperComparer<T>();

            public int Compare((int hashCode, T value) x, (int hashCode, T value) y)
            {
                return x.hashCode - y.hashCode;
            }
        }
    }
}
