using PCServer.TaskNodeServer.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.TaskNodeServer
{
    public class TaskManager
    {
        //上传的任务池
        public Queue<NodeTask> TaskPools = new Queue<NodeTask>();

        //分发过的任务<TaskGUID, Task>
        public Dictionary<string, NodeTask> TaskSended = new Dictionary<string, NodeTask>();

        private static TaskManager _inst;
        public static TaskManager inst
        {
            get
            {
                if (_inst == null)
                    _inst = new TaskManager();
                return _inst;
            }
        }

        public void AddTask(NodeTask task)
        {
            TaskPools.Enqueue(task);
        }

        public NodeTask SendTask(string socketId)
        {
            if(TaskPools.Count <= 0)
            {
                return null;
            }

            var task = TaskPools.Dequeue();
            //var task = TaskPools.Peek();

            if(!TaskSended.ContainsKey(task.TaskGuid))
            {
                TaskSended.Add(task.TaskGuid, task);
            } else
            {
                TaskSended[task.TaskGuid] = task;
            }

            if (task != null)
            {
                var taskjson = Newtonsoft.Json.JsonConvert.SerializeObject(task);
                AgentsManager.inst.SendMessageToAgent(socketId, taskjson);
            }

            return task;
        }
        
        //每隔一段时间, 检测是否有在线Online的Agent和未分配的任务, 将任务分发
        public void TaskDispatcher()
        {
            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                while (true)
                {
                    //之后这里需要: 智能分发, 判断在线用户和最大承受任务数, 优先没有任务的Agent和性能高的Agent

                    var AgentClient = AgentsManager.inst.GetAgent();
                    if(AgentClient != null)
                    {
                        TaskManager.inst.SendTask(AgentClient.socketId);
                    }

                    Thread.Sleep(5000);
                }
            });
        }

    }
}
