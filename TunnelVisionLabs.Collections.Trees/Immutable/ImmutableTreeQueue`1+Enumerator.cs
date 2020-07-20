// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeQueue<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private ImmutableTreeList<T>.Enumerator _enumerator;

            internal Enumerator(ImmutableTreeList<T>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public T Current => _enumerator.Current;

            object? IEnumerator.Current => _enumerator.Current;

            public bool MoveNext() => _enumerator.MoveNext();

            void IDisposable.Dispose() => _enumerator.Dispose();

            void IEnumerator.Reset() => InternalReset();

            internal void InternalReset() => _enumerator.Reset();
        }
    }
}
