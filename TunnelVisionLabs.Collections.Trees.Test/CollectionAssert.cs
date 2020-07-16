// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections.Generic;
    using Xunit;

    public static class CollectionAssert
    {
        public static void EnumeratorInvalidated<T>(IEnumerable<T> enumerable, Action action)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                action();
                Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
            }
        }

        public static void EnumeratorNotInvalidated<T>(IEnumerable<T> enumerable, Action action)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                action();
                enumerator.MoveNext();
            }
        }
    }
}
