// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class EverythingThrowsEnumerable<T> : IEnumerable<T>
    {
        public static readonly EverythingThrowsEnumerable<T> Instance = new EverythingThrowsEnumerable<T>();

        private EverythingThrowsEnumerable()
        {
        }

        public IEnumerator<T> GetEnumerator() => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotSupportedException();
    }
}
