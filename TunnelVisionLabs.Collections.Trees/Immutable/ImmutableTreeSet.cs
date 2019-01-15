// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;

    public static class ImmutableTreeSet
    {
        public static ImmutableTreeSet<T> Create<T>()
            => ImmutableTreeSet<T>.Empty;

        public static ImmutableTreeSet<T> Create<T>(T item)
            => ImmutableTreeSet<T>.Empty.Add(item);

        public static ImmutableTreeSet<T> Create<T>(params T[] items)
            => ImmutableTreeSet<T>.Empty.Union(items);

        public static ImmutableTreeSet<T> Create<T>(IEqualityComparer<T> equalityComparer)
            => ImmutableTreeSet<T>.Empty.WithComparer(equalityComparer);

        public static ImmutableTreeSet<T> Create<T>(IEqualityComparer<T> equalityComparer, T item)
            => ImmutableTreeSet<T>.Empty.WithComparer(equalityComparer).Add(item);

        public static ImmutableTreeSet<T> Create<T>(IEqualityComparer<T> equalityComparer, params T[] items)
            => ImmutableTreeSet<T>.Empty.WithComparer(equalityComparer).Union(items);

        public static ImmutableTreeSet<T>.Builder CreateBuilder<T>()
            => Create<T>().ToBuilder();

        public static ImmutableTreeSet<T>.Builder CreateBuilder<T>(IEqualityComparer<T> equalityComparer)
            => Create(equalityComparer).ToBuilder();

        public static ImmutableTreeSet<T> CreateRange<T>(IEnumerable<T> items)
            => ImmutableTreeSet<T>.Empty.Union(items);

        public static ImmutableTreeSet<T> CreateRange<T>(IEqualityComparer<T> equalityComparer, IEnumerable<T> items)
            => ImmutableTreeSet<T>.Empty.WithComparer(equalityComparer).Union(items);

        public static ImmutableTreeSet<TSource> ToImmutableTreeSet<TSource>(this IEnumerable<TSource> source)
            => ToImmutableTreeSet(source, equalityComparer: null);

        public static ImmutableTreeSet<TSource> ToImmutableTreeSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> equalityComparer)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return ImmutableTreeSet<TSource>.Empty.WithComparer(equalityComparer).Union(source);
        }
    }
}
