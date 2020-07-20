// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections.Generic;

    public static class ImmutableTreeList
    {
        public static ImmutableTreeList<T> Create<T>() => ImmutableTreeList<T>.Empty;

        public static ImmutableTreeList<T> Create<T>(T item) => ImmutableTreeList<T>.Empty.Add(item);

        public static ImmutableTreeList<T> Create<T>(params T[] items) => ImmutableTreeList<T>.Empty.AddRange(items);

        public static ImmutableTreeList<T>.Builder CreateBuilder<T>() => Create<T>().ToBuilder();

        public static ImmutableTreeList<T> CreateRange<T>(IEnumerable<T> items) => ImmutableTreeList<T>.Empty.AddRange(items);

        public static ImmutableTreeList<T> ToImmutableTreeList<T>(this IEnumerable<T> source)
        {
            if (source is ImmutableTreeList<T> existingList)
                return existingList;

            return ImmutableTreeList<T>.Empty.AddRange(source);
        }
    }
}
