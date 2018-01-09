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
