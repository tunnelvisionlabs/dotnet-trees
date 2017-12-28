// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#if !NETSTANDARD1_0

namespace TunnelVisionLabs.Collections.Trees
{
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal static class SyncRootFallback
    {
        private static readonly ConditionalWeakTable<IEnumerable, object> _syncRoots =
            new ConditionalWeakTable<IEnumerable, object>();

        public static object GetOrCreateSyncRoot<T>(T collection)
            where T : class, IEnumerable
        {
            Debug.Assert(collection != null, $"Assertion failed: {nameof(collection)} != null");

            return _syncRoots.GetOrCreateValue(collection);
        }
    }
}

#endif
