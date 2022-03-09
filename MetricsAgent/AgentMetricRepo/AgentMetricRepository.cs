using Dapper;
using MetricsAgent.AgentmetricRepo;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.AgentMetricRepo
{
    public interface IAgentIterfaceRepository : IAgentInterface<AgentMetric>
    {

    }

    public class AgentMetricRepository : IAgentIterfaceRepository
    {
        private const string ConnectionString = @"Data Source=metrics.db; Version=3;Pooling=True;Max Pool Size=100;";
        public AgentMetricRepository()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public void Create(AgentMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO agentmetrics(value, time) VALUES(@value, @time)",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds
                    });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM agentmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }

        public void Update(AgentMetric item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE agentmetrics SET value = @value, time = @time WHERE id=@id",
                    new
                    {
                        value = item.Value,
                        time = item.Time.TotalSeconds,
                        id = item.Id
                    });
            }
        }

        public IList<AgentMetric> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<AgentMetric>("SELECT Id, Time, Value FROM agentmetrics").ToList();
            }
        }

        public AgentMetric GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<AgentMetric>("SELECT Id, Time, Value FROM agentmetrics WHERE id=@id",
                    new { id = id });
            }
        }


    }
}
