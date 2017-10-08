// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections.Generic;

    public static class ImmutableTreeList
    {
        public static ImmutableTreeList<T> Create<T>() => throw null;

        public static ImmutableTreeList<T> Create<T>(T item) => throw null;

        public static ImmutableTreeList<T> Create<T>(params T[] items) => throw null;

        public static ImmutableTreeList<T>.Builder CreateBuilder<T>() => throw null;

        public static ImmutableTreeList<T> CreateRange<T>(IEnumerable<T> items) => throw null;

        public static ImmutableTreeList<T> ToImmutableTreeList<T>(this IEnumerable<T> source) => throw null;
    }
}
