﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ImmutableTreeDictionary
    {
        public static ImmutableTreeDictionary<TKey, TValue> Create<TKey, TValue>()
            where TKey : notnull
            => ImmutableTreeDictionary<TKey, TValue>.Empty;

        public static ImmutableTreeDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey>? keyComparer)
            where TKey : notnull
            => ImmutableTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer);

        public static ImmutableTreeDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            where TKey : notnull
            => ImmutableTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer);

        public static ImmutableTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>()
            where TKey : notnull
            => Create<TKey, TValue>().ToBuilder();

        public static ImmutableTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IEqualityComparer<TKey>? keyComparer)
            where TKey : notnull
            => Create<TKey, TValue>(keyComparer).ToBuilder();

        public static ImmutableTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            where TKey : notnull
            => Create<TKey, TValue>(keyComparer, valueComparer).ToBuilder();

        public static ImmutableTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> items)
            where TKey : notnull
            => ImmutableTreeDictionary<TKey, TValue>.Empty.AddRange(items);

        public static ImmutableTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEnumerable<KeyValuePair<TKey, TValue>> items)
            where TKey : notnull
            => ImmutableTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer).AddRange(items);

        public static ImmutableTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer, IEnumerable<KeyValuePair<TKey, TValue>> items)
            where TKey : notnull
            => ImmutableTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer).AddRange(items);

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items)
            where TKey : notnull
            => ToImmutableTreeDictionary(items, keyComparer: null, valueComparer: null);

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey>? keyComparer)
            where TKey : notnull
            => ToImmutableTreeDictionary(items, keyComparer, valueComparer: null);

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            where TKey : notnull
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (items is ImmutableTreeDictionary<TKey, TValue> existingDictionary)
                return existingDictionary.WithComparers(keyComparer, valueComparer);

            return ImmutableTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer).AddRange(items);
        }

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
            where TKey : notnull
            => ToImmutableTreeDictionary(source, keySelector, elementSelector, keyComparer: null, valueComparer: null);

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey>? keyComparer)
            where TKey : notnull
            => ToImmutableTreeDictionary(source, keySelector, elementSelector, keyComparer, valueComparer: null);

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
            where TKey : notnull
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector is null)
                throw new ArgumentNullException(nameof(elementSelector));

            return ImmutableTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer)
                .AddRange(source.Select(element => new KeyValuePair<TKey, TValue>(keySelector(element), elementSelector(element))));
        }

        public static ImmutableTreeDictionary<TKey, TSource> ToImmutableTreeDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            where TKey : notnull
            => ToImmutableTreeDictionary(source, keySelector, elementSelector: x => x, keyComparer: null, valueComparer: null);

        public static ImmutableTreeDictionary<TKey, TSource> ToImmutableTreeDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? keyComparer)
            where TKey : notnull
            => ToImmutableTreeDictionary(source, keySelector, elementSelector: x => x, keyComparer, valueComparer: null);
    }
}
