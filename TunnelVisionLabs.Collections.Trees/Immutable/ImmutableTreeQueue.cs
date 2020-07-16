// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections.Generic;

    public static class ImmutableTreeQueue
    {
        public static ImmutableTreeQueue<T> Create<T>()
            => ImmutableTreeQueue<T>.Empty;

        public static ImmutableTreeQueue<T> Create<T>(T item)
            => ImmutableTreeQueue<T>.Empty.Enqueue(item);

        public static ImmutableTreeQueue<T> Create<T>(params T[] items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            ImmutableTreeQueue<T> result = ImmutableTreeQueue<T>.Empty;
            foreach (T item in items)
                result = result.Enqueue(item);

            return result;
        }

        public static ImmutableTreeQueue<T> CreateRange<T>(IEnumerable<T> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            ImmutableTreeQueue<T> result = ImmutableTreeQueue<T>.Empty;
            foreach (T item in items)
                result = result.Enqueue(item);

            return result;
        }
    }
}
