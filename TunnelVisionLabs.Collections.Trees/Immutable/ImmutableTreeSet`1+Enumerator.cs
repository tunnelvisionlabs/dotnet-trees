// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Immutable
{
    using System.Collections;
    using System.Collections.Generic;

    public partial class ImmutableTreeSet<T>
    {
        public struct Enumerator : IEnumerator<T>
        {
            private ImmutableSortedTreeList<(int hashCode, T value)>.Enumerator _enumerator;

            internal Enumerator(ImmutableSortedTreeList<(int hashCode, T value)>.Enumerator enumerator)
            {
                _enumerator = enumerator;
            }

            public T Current => _enumerator.Current.value;

            object IEnumerator.Current => Current;

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();
        }
    }
}
