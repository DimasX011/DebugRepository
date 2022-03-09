
using AutoMapper;
using Core;
using FluentMigrator.Runner;
using MetricsAgent.AgentMetricRepo;
using MetricsAgent.CpuMetricRepo;
using MetricsAgent.DotnetMetricRepo;
using MetricsAgent.HddMetricRepo;
using MetricsAgent.Jobs;
using MetricsAgent.NetworkMetricRepo;
using MetricsAgent.RamMetricRepo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System.Data.SQLite;

namespace MetricsAgent
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        private const string ConnectionString = @"Data Source=metrics.db; Version=3;";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<ICpuInterfaceRepository, CpuMetricRepository>();
            services.AddSingleton<IRamMetricRepository, RamMetricRepository>();
            services.AddSingleton<IDotnetInterfaceRepository, DotnetMetricRepository>();
            services.AddSingleton<IhddMetricInterface, HddMetricRepository>();
            services.AddSingleton<IAgentIterfaceRepository, AgentMetricRepository>();
            services.AddSingleton<INetworkMetricRepoitory, NetworkMetricRepository>();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();
            services.AddSingleton<CpuMetricJob>();
            services.AddSingleton(new JobSchedule(
            jobType: typeof(CpuMetricJob),
            cronExpression: "0/5 * * * * ?")); // ��������� ������ 5 ������
            services.AddSingleton(new JobSchedule(
            jobType: typeof(RamMetricJob),
            cronExpression: "0/5 * * * * ?"));

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // ��������� ��������� SQLite 
                    .AddSQLite()
                    // ������������� ������ �����������
                    .WithGlobalConnectionString(ConnectionString)
                    // ������������, ��� ������ ������ � ����������
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                ).AddLogging(lb => lb
                    .AddFluentMigratorConsole());
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // ��������� ��������
            migrationRunner.MigrateUp();
           }
   
        }
    }
