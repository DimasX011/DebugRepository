using AutoMapper;
using MetricsAgent.AgentMetricRepo;
using MetricsAgent.AllRequestMetric.Response;
using MetricsAgent.CpuMetricRepo;
using MetricsAgent.HddMetricRepo;
using MetricsAgent.NetworkMetricRepo;
using MetricsAgent.RamMetricRepo;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CpuMetric, CpuMetricDto>();
            CreateMap<AgentMetric, AgentMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<NetworkMetric, NetworkMetricDto>();
            CreateMap<HddMetric, HddMetricDto>();
            CreateMap<RamMetric, RamMetricDto>();
        }
    }

}
