using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class AgentInfo
    {   
        public int AgentId { get;}

        public Uri AgentAddress { get; }

        public List<AgentInfo> agentInfos { get; set; }
        

        public AgentInfo(int agentid, Uri uri)
        {
            AgentId = agentid;
            AgentAddress = uri;
        }
        public void Add(AgentInfo agentInfo)
        {
            agentInfos.Add(agentInfo);
        }

        public List<AgentInfo> Get()
        {
            return agentInfos;
        }
    }
}
