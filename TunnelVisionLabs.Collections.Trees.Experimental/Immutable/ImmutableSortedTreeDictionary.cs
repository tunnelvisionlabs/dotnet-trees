// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ImmutableSortedTreeDictionary
    {
        public static ImmutableSortedTreeDictionary<TKey, TValue> Create<TKey, TValue>()
            => ImmutableSortedTreeDictionary<TKey, TValue>.Empty;

        public static ImmutableSortedTreeDictionary<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> keyComparer)
            => ImmutableSortedTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer);

        public static ImmutableSortedTreeDictionary<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            => ImmutableSortedTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer);

        public static ImmutableSortedTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>()
            => Create<TKey, TValue>().ToBuilder();

        public static ImmutableSortedTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IComparer<TKey> keyComparer)
            => Create<TKey, TValue>(keyComparer).ToBuilder();

        public static ImmutableSortedTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            => Create<TKey, TValue>(keyComparer, valueComparer).ToBuilder();

        public static ImmutableSortedTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> items)
            => ImmutableSortedTreeDictionary<TKey, TValue>.Empty.AddRange(items);

        public static ImmutableSortedTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IComparer<TKey> keyComparer, IEnumerable<KeyValuePair<TKey, TValue>> items)
            => ImmutableSortedTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer).AddRange(items);

        public static ImmutableSortedTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, IEnumerable<KeyValuePair<TKey, TValue>> items)
            => ImmutableSortedTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer).AddRange(items);

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items)
            => ToImmutableSortedTreeDictionary(items, keyComparer: null, valueComparer: null);

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IComparer<TKey> keyComparer)
            => ToImmutableSortedTreeDictionary(items, keyComparer, valueComparer: null);

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (items is ImmutableSortedTreeDictionary<TKey, TValue> existingDictionary)
                return existingDictionary.WithComparers(keyComparer, valueComparer);

            return ImmutableSortedTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer).AddRange(items);
        }

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
            => ToImmutableSortedTreeDictionary(source, keySelector, elementSelector, keyComparer: null, valueComparer: null);

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IComparer<TKey> keyComparer)
            => ToImmutableSortedTreeDictionary(source, keySelector, elementSelector, keyComparer, valueComparer: null);

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));
            if (elementSelector is null)
                throw new ArgumentNullException(nameof(elementSelector));

            return ImmutableSortedTreeDictionary<TKey, TValue>.Empty.WithComparers(keyComparer, valueComparer)
                .AddRange(source.Select(element => new KeyValuePair<TKey, TValue>(keySelector(element), elementSelector(element))));
        }
    }
}
