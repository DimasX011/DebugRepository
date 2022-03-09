using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MetricsAgent.AgentMetricRepo;
using MetricsAgent.AllRequestMetric;
using MetricsAgent.AllRequestMetric.Response;
using Microsoft.Extensions.Logging;
using System.Data.SQLite;
using AutoMapper;
using System;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentMetricsController : ControllerBase
    {
        private IAgentIterfaceRepository repository;
        private readonly ILogger<AgentMetricsController> _logger;
        private readonly IMapper mapper;
        public AgentMetricsController(IAgentIterfaceRepository repository, ILogger<AgentMetricsController> logger, IMapper mapper)
        {
            this.mapper = mapper;
            this.repository = repository;
            _logger = logger;

        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] CreateRequest request)
        {
            _logger.LogInformation("запрос",request);
            
            repository.Create(new AgentMetric
            {
                Time = request.Time,
                Value = request.Value
            });
            _logger.LogDebug("Регистрация пользователя:", request);
            return Ok();
        }

        [HttpGet("sql-read-write-test")]
        public IActionResult TryToInsertAndRead()
        {
            string connectionString = "Data Source=:memory:";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DROP TABLE IF EXISTS agentmetrics";
                    command.ExecuteNonQuery();
                    command.CommandText = @"CREATE TABLE agentmetrics(id INTEGER PRIMARY KEY,
                    value INT, time INT)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(10,1)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(50,2)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(75,4)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO agentmetrics(value, time) VALUES(90,5)";
                    command.ExecuteNonQuery();
                    string readQuery = "SELECT * FROM agentmetrics LIMIT 3";
                    var returnArray = new GenerateData[3];
                    command.CommandText = readQuery;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        var counter = 0;
                        while (reader.Read())
                        {
                            returnArray[counter] = new GenerateData
                            {
                                Id = reader.GetInt32(0),
                                Value = reader.GetInt32(1),
                                Time = reader.GetInt64(2)
                            };

                            counter++;
                        }
                    }
                    return Ok(returnArray);
                }
            }
        }

        [HttpGet("from/{fromTime}/to/{toTime}/")]
        public IActionResult GetMetrics(DateTime toTime)
        {
            DateTime fromTime = new DateTime(1970, 01, 01);
            string connectionString = "Data Source=:memory:";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var command = new SQLiteCommand(connection))
                {
                    string time = "SELECT id, value, time FROM agentmetrics WHERE time>@fromTime AND time<@toTime";
                    List<int> dateTimes = new List<int>();
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        var counter = 0;
                        while (reader.Read())
                        {
                           
                        }
                    }
                }
                
                return Ok()
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            IList<AgentMetric> metrics = repository.GetAll();

            var response = new AllAgentMetricResponse()
            {
                Metrics = new List<AgentMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<AgentMetricDto>(metric));
            }

            return Ok(response);
        }


        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogInformation("Регистрация пользователя:", agentInfo);
            return Ok();
        }

        [HttpGet("read")]
        public IActionResult ReadAllRegistred()
        {
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Регистрация пользователя:", agentId);
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("Регистрация пользователя:", agentId);
            return Ok();
        }
    }
}
