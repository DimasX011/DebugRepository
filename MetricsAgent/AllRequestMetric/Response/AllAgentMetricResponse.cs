using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.AllRequestMetric.Response
{
    public class AllAgentMetricResponse
    {
        public List<AgentMetricDto> Metrics { get; set; }
    }

    public class AgentMetricDto
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }
        public int Id { get; set; }
    }

}
