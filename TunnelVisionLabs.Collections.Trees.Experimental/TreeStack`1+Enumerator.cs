// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class TreeStack<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            public T Current => throw null;

            object IEnumerator.Current => throw null;

            public void Dispose() => throw null;

            public bool MoveNext() => throw null;

            void IEnumerator.Reset() => throw null;
        }
    }
}
