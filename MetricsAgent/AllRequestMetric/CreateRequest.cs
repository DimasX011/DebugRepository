using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.AllRequestMetric
{
    public class CreateRequest
    {
        public TimeSpan Time { get; set; }
        public int Value { get; set; }

    }
}
