using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.TaskNodeServer.Agent
{
    public enum AgentTypeEnum
    {
        kunKnown = 0,
        kBuildShader = 1,
        kTest = 2,
    }

    public enum AgentRunnerTypeEnum
    {
        kPowershell = 0,
    }

    //public class Cmd
    //{
    //    public string cmd { get; set; }
    //    public string args { get; set; }
    //}

    public class AgentRunContent
    {
        public AgentTypeEnum AgentType { get; set; }

        public AgentRunnerTypeEnum RunnerType { get; set; }

        public List<string> CmdList { get; set; }
    }



}
