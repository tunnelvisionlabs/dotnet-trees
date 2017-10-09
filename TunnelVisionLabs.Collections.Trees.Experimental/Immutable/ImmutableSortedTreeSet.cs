// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections.Generic;

    public static class ImmutableSortedTreeSet
    {
        public static ImmutableSortedTreeSet<T> Create<T>() => throw null;

        public static ImmutableSortedTreeSet<T> Create<T>(T item) => throw null;

        public static ImmutableSortedTreeSet<T> Create<T>(params T[] items) => throw null;

        public static ImmutableSortedTreeSet<T> Create<T>(IComparer<T> comparer) => throw null;

        public static ImmutableSortedTreeSet<T> Create<T>(IComparer<T> comparer, T item) => throw null;

        public static ImmutableSortedTreeSet<T> Create<T>(IComparer<T> comparer, params T[] items) => throw null;

        public static ImmutableSortedTreeSet<T>.Builder CreateBuilder<T>() => throw null;

        public static ImmutableSortedTreeSet<T>.Builder CreateBuilder<T>(IComparer<T> comparer) => throw null;

        public static ImmutableSortedTreeSet<T> CreateRange<T>(IEnumerable<T> items) => throw null;

        public static ImmutableSortedTreeSet<T> CreateRange<T>(IComparer<T> comparer, IEnumerable<T> items) => throw null;

        public static ImmutableSortedTreeSet<TSource> ToImmutableSortedTreeSet<TSource>(this IEnumerable<TSource> source) => throw null;

        public static ImmutableSortedTreeSet<TSource> ToImmutableSortedTreeSet<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer) => throw null;
    }
}
