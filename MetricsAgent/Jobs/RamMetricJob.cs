using MetricsAgent.DAL;
using Quartz;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MetricsAgent.CpuMetricRepo;

namespace MetricsAgent.Jobs
{
    public class RamMetricJob : IJob
    {
        private ICpuInterfaceRepository _repository;
        private PerformanceCounter _ramCounter;

        public RamMetricJob(ICpuInterfaceRepository repository)
        {
            _repository = repository;
            _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Получаем значение занятости CPU
            var ramUsageInPercent = Convert.ToInt32(_ramCounter.NextValue());
            // Узнаем, когда мы сняли значение метрики
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            // Теперь можно записать что-то посредством репозитория
            _repository.Create(new CpuMetric
            {
                Time = time,
                Value = ramUsageInPercent
            });
            // Теперь можно записать что-то посредством репозитория

            return Task.CompletedTask;
        }
    }
}