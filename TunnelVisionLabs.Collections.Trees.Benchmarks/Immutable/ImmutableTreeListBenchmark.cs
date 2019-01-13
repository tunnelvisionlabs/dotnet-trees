// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Benchmarks.Immutable
{
    using System.Collections.Immutable;
    using System.Linq;
    using BenchmarkDotNet.Attributes;
    using TunnelVisionLabs.Collections.Trees.Immutable;

    public class ImmutableTreeListBenchmark
    {
        [ShortRunJob]
        public class RangeToList
        {
            [Params(10, 1000, 100000, 10000000)]
            public int Count
            {
                get;
                set;
            }

            [Benchmark(Baseline = true, Description = "ImmutableList<T>")]
            public ImmutableList<int> List()
            {
                return ImmutableList.CreateRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "ImmutableArray<T>")]
            public ImmutableArray<int> Array()
            {
                return ImmutableArray.CreateRange(Enumerable.Range(0, Count));
            }

            [Benchmark(Description = "ImmutableTreeList<T>")]
            public ImmutableTreeList<int> TreeList()
            {
                return ImmutableTreeList.CreateRange(Enumerable.Range(0, Count));
            }
        }
    }
}
