// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections.Generic;

    public static class ImmutableTreeSet
    {
        public static ImmutableTreeSet<T> Create<T>() => throw null;

        public static ImmutableTreeSet<T> Create<T>(T item) => throw null;

        public static ImmutableTreeSet<T> Create<T>(params T[] items) => throw null;

        public static ImmutableTreeSet<T> Create<T>(IEqualityComparer<T> equalityComparer) => throw null;

        public static ImmutableTreeSet<T> Create<T>(IEqualityComparer<T> equalityComparer, T item) => throw null;

        public static ImmutableTreeSet<T> Create<T>(IEqualityComparer<T> equalityComparer, params T[] items) => throw null;

        public static ImmutableTreeSet<T>.Builder CreateBuilder<T>() => throw null;

        public static ImmutableTreeSet<T>.Builder CreateBuilder<T>(IEqualityComparer<T> equalityComparer) => throw null;

        public static ImmutableTreeSet<T> CreateRange<T>(IEnumerable<T> items) => throw null;

        public static ImmutableTreeSet<T> CreateRange<T>(IEqualityComparer<T> equalityComparer, IEnumerable<T> items) => throw null;

        public static ImmutableTreeSet<TSource> ToImmutableTreeSet<TSource>(this IEnumerable<TSource> source) => throw null;

        public static ImmutableTreeSet<TSource> ToImmutableTreeSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> equalityComparer) => throw null;
    }
}
