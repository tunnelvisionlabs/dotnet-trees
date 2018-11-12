// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;

    public static class ImmutableSortedTreeDictionary
    {
        public static ImmutableSortedTreeDictionary<TKey, TValue> Create<TKey, TValue>() => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> keyComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> Create<TKey, TValue>(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>() => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IComparer<TKey> keyComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IComparer<TKey> keyComparer, IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IComparer<TKey> keyComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IComparer<TKey> keyComparer) => throw null;

        public static ImmutableSortedTreeDictionary<TKey, TValue> ToImmutableSortedTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;
    }
}
