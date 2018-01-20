using PCServer.TaskNodeServer.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.TaskNodeServer.Task
{
    //TargetFiles, ShellType, ShellContent
    //OutDir

    public class NodeTask
    {
        //任务ID
        public string TaskGuid { get; set; }
        //任务发布者
        public string AuthorID { get; set; }
        //任务名称
        public string TaskName { get; set; }
        //任务描述
        public string TaskDesc { get; set; }
        //任务内容
        public AgentRunContent AgentRunContent { get; set; }
        //附件 上传至该任务主目录的 files/ 目录下
        //public List<string> Files { get; set; }

        public NodeTask ()
        {
            TaskGuid = Guid.NewGuid().ToString();
        }
    }
}
