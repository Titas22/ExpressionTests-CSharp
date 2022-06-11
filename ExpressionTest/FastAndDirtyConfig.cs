using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Perfolizer.Horology;
using BenchmarkDotNet.Reports;

namespace ExpressionTest
{
    public class FastAndDirtyConfig : ManualConfig
    {
        public FastAndDirtyConfig()
        {
            Add(DefaultConfig.Instance); // *** add default loggers, reporters etc? ***
            AddJob(Job.Default
                .WithLaunchCount(1)     // benchmark process will be launched only once
                .WithIterationTime(new TimeInterval(50, TimeUnit.Millisecond)) // 100ms per iteration
                .WithWarmupCount(1)     // 3 warmup iteration
                .WithIterationCount(20)
            );
            /*
            SummaryStyle = SummaryStyle.Default
                .WithRatioStyle(BenchmarkDotNet.Columns.RatioStyle.Percentage);*/
        }
    }
}
