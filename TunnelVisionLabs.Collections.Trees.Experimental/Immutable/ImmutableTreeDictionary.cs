// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;

    public static class ImmutableTreeDictionary
    {
        public static ImmutableTreeDictionary<TKey, TValue> Create<TKey, TValue>() => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> keyComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>() => throw null;

        public static ImmutableTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IEqualityComparer<TKey> keyComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue>.Builder CreateBuilder<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> CreateRange<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> keyComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey> keyComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TValue> ToImmutableTreeDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) => throw null;

        public static ImmutableTreeDictionary<TKey, TSource> ToImmutableTreeDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) => throw null;

        public static ImmutableTreeDictionary<TKey, TSource> ToImmutableTreeDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> keyComparer) => throw null;
    }
}
