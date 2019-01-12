// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections.Generic;

    internal static class ImmutableSortedTreeList
    {
        public static ImmutableSortedTreeList<T> Create<T>()
            => ImmutableSortedTreeList<T>.Empty;

        public static ImmutableSortedTreeList<T> Create<T>(T item)
            => ImmutableSortedTreeList<T>.Empty.Add(item);

        public static ImmutableSortedTreeList<T> Create<T>(params T[] items)
            => ImmutableSortedTreeList<T>.Empty.AddRange(items);

        public static ImmutableSortedTreeList<T> Create<T>(IComparer<T> comparer)
            => ImmutableSortedTreeList<T>.Empty.WithComparer(comparer);

        public static ImmutableSortedTreeList<T> Create<T>(IComparer<T> comparer, T item)
            => ImmutableSortedTreeList<T>.Empty.WithComparer(comparer).Add(item);

        public static ImmutableSortedTreeList<T> Create<T>(IComparer<T> comparer, params T[] items)
            => ImmutableSortedTreeList<T>.Empty.WithComparer(comparer).AddRange(items);

        public static ImmutableSortedTreeList<T>.Builder CreateBuilder<T>()
            => Create<T>().ToBuilder();

        public static ImmutableSortedTreeList<T>.Builder CreateBuilder<T>(IComparer<T> comparer)
            => Create<T>(comparer).ToBuilder();

        public static ImmutableSortedTreeList<T> CreateRange<T>(IEnumerable<T> items)
            => ImmutableSortedTreeList<T>.Empty.AddRange(items);

        public static ImmutableSortedTreeList<T> CreateRange<T>(IComparer<T> comparer, IEnumerable<T> items)
            => ImmutableSortedTreeList<T>.Empty.WithComparer(comparer).AddRange(items);

        public static ImmutableSortedTreeList<T> ToImmutableSortedTreeList<T>(this IEnumerable<T> source)
            => ToImmutableSortedTreeList(source, comparer: null);

        public static ImmutableSortedTreeList<T> ToImmutableSortedTreeList<T>(this IEnumerable<T> source, IComparer<T> comparer)
            => ImmutableSortedTreeList<T>.Empty.WithComparer(comparer).AddRange(source);
    }
}
