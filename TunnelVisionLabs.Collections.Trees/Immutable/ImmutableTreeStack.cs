// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;

    public static class ImmutableTreeStack
    {
        public static ImmutableTreeStack<T> Create<T>()
            => ImmutableTreeStack<T>.Empty;

        public static ImmutableTreeStack<T> Create<T>(T item)
            => ImmutableTreeStack<T>.Empty.Push(item);

        public static ImmutableTreeStack<T> Create<T>(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            ImmutableTreeStack<T> result = ImmutableTreeStack<T>.Empty;
            foreach (T item in items)
                result = result.Push(item);

            return result;
        }

        public static ImmutableTreeStack<T> CreateRange<T>(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            ImmutableTreeStack<T> result = ImmutableTreeStack<T>.Empty;
            foreach (T item in items)
                result = result.Push(item);

            return result;
        }
    }
}
