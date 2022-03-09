using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.AllRequestMetric.Response
{
    public class AllDotnetMetricResponse
    {
        public List<DotnetMetricDto> Metrics { get; set; }
    }

    public class DotnetMetricDto
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }
}
