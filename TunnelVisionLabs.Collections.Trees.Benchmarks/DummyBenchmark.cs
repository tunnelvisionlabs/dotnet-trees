// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace TunnelVisionLabs.Collections.Trees.Benchmarks
{
    using BenchmarkDotNet.Attributes;

    public class DummyBenchmark
    {
        private static readonly int[] Ints = { 1, 2, 3 };

        [Benchmark(Baseline = true)]
        public object NewTreeList1()
        {
            return new TreeList<int>(Ints);
        }

        [Benchmark]
        public object NewTreeList2()
        {
            return new TreeList<int>(Ints);
        }
    }
}
