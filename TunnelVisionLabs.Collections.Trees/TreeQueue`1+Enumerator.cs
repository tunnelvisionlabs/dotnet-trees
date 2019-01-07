// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class TreeQueue<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private TreeList<T>.Enumerator _enumerator;

            internal Enumerator(TreeList<T>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public T Current => _enumerator.Current;

            object IEnumerator.Current => _enumerator.Current;

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext() => _enumerator.MoveNext();

            void IEnumerator.Reset() => InternalReset();

            internal void InternalReset() => _enumerator.InternalReset();
        }
    }
}
