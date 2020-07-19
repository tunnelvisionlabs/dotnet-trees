// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;

    public static class ImmutableSortedTreeSet
    {
        public static ImmutableSortedTreeSet<T> Create<T>()
            => ImmutableSortedTreeSet<T>.Empty;

        public static ImmutableSortedTreeSet<T> Create<T>(T item)
            => ImmutableSortedTreeSet<T>.Empty.Add(item);

        public static ImmutableSortedTreeSet<T> Create<T>(params T[] items)
            => ImmutableSortedTreeSet<T>.Empty.Union(items);

        public static ImmutableSortedTreeSet<T> Create<T>(IComparer<T>? comparer)
            => ImmutableSortedTreeSet<T>.Empty.WithComparer(comparer);

        public static ImmutableSortedTreeSet<T> Create<T>(IComparer<T>? comparer, T item)
            => ImmutableSortedTreeSet<T>.Empty.WithComparer(comparer).Add(item);

        public static ImmutableSortedTreeSet<T> Create<T>(IComparer<T>? comparer, params T[] items)
            => ImmutableSortedTreeSet<T>.Empty.WithComparer(comparer).Union(items);

        public static ImmutableSortedTreeSet<T>.Builder CreateBuilder<T>()
            => Create<T>().ToBuilder();

        public static ImmutableSortedTreeSet<T>.Builder CreateBuilder<T>(IComparer<T>? comparer)
            => Create(comparer).ToBuilder();

        public static ImmutableSortedTreeSet<T> CreateRange<T>(IEnumerable<T> items)
            => ImmutableSortedTreeSet<T>.Empty.Union(items);

        public static ImmutableSortedTreeSet<T> CreateRange<T>(IComparer<T>? comparer, IEnumerable<T> items)
            => ImmutableSortedTreeSet<T>.Empty.WithComparer(comparer).Union(items);

        public static ImmutableSortedTreeSet<TSource> ToImmutableSortedTreeSet<TSource>(this IEnumerable<TSource> source)
            => ToImmutableSortedTreeSet(source, comparer: null);

        public static ImmutableSortedTreeSet<TSource> ToImmutableSortedTreeSet<TSource>(this IEnumerable<TSource> source, IComparer<TSource>? comparer)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return ImmutableSortedTreeSet<TSource>.Empty.WithComparer(comparer).Union(source);
        }
    }
}
