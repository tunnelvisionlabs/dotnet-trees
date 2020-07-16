// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace TunnelVisionLabs.Collections.Trees.Test.Immutable
{
    using System.Collections.Generic;
    using TunnelVisionLabs.Collections.Trees.Immutable;

    public partial class ImmutableSortedTreeSetBuilderTest
    {
        public class ImmutableArguments : AbstractSetTest
        {
            protected override ISet<T> CreateSet<T>()
            {
                return ImmutableSortedTreeSet.CreateBuilder<T>();
            }

            protected override IEnumerable<T> TransformEnumerableForSetOperation<T>(IEnumerable<T> enumerable)
            {
                return ImmutableSortedTreeSet.CreateRange(enumerable);
            }
        }
    }
}
