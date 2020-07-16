// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class TreeSet<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private SortedTreeList<(int hashCode, T value)>.Enumerator _enumerator;

            internal Enumerator(SortedTreeList<(int hashCode, T value)>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public T Current => _enumerator.Current.value;

            object IEnumerator.Current => Current;

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext() => _enumerator.MoveNext();

            void IEnumerator.Reset() => InternalReset();

            internal void InternalReset() => _enumerator.InternalReset();
        }
    }
}
