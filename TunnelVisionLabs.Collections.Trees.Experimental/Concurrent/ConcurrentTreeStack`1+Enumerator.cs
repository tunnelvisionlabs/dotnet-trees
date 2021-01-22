﻿// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Concurrent
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class ConcurrentTreeStack<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            public T Current => throw null!;

            object? IEnumerator.Current => throw null!;

            public void Dispose() => throw null!;

            public bool MoveNext() => throw null!;

            public void Reset() => throw null!;
        }
    }
}
