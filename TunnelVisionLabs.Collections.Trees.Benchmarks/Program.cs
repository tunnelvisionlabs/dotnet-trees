namespace TunnelVisionLabs.Collections.Trees.Benchmarks
{
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            if (true)
            {
                new BenchmarkSwitcher(typeof(Program).Assembly).Run(new[] { "*" });
            }
            else
            {
                BenchmarkRunner.Run<DummyBenchmark>();
            }
        }
    }
}
