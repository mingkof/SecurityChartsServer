using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using PCServer.Server.Net;
using PCServer.Server.Proto_Gongan;
using SHSecurityModels;
using SHSecurityContext.IRepositorys;
using Microsoft.Extensions.DependencyInjection;

namespace PCServer.TaskNodeServer
{

    public class AgentClient
    {
        public string socketId = "";
        //最大分配执行任务数
        public int maxTaskingCount = 5;

        //已分配任务
        public List<string> SendedTasks = new List<string>();

        //心跳上次时间
        public int HeartLastTimestamp = 0;
    }

    public delegate void AddAgentDelegate(string socketId);
    public delegate void RemoveAgentDelegate(string socketId);

    public class AgentsManager
    {
      public  SipCameraManager _SipManager = null;

        private static AgentsManager _inst;
        public static AgentsManager inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new AgentsManager();
                    _inst.Init();
                }
                return _inst;
            }
        }

        public event AddAgentDelegate AddAgentEvent;
        public event RemoveAgentDelegate RemoveAgentEvent;

        void Init()
        {
            _SipManager = new SipCameraManager();
            _SipManager.Init();
        }
        

        public AgentClient SipAgent = null;

        //socketid, agentClient
        Dictionary<string, AgentClient> AgentOnlineList = new Dictionary<string, AgentClient>();

        public AgentClient GetAgent()
        {
            if (AgentOnlineList.Count > 0)
            {
                return AgentOnlineList.First().Value;
            }

            return null;
        }

        public AgentClient GetAgent(string socketId) {
            if (AgentOnlineList.Count > 0)
            {
                if (AgentOnlineList.ContainsKey(socketId))
                    return AgentOnlineList[socketId];
            }

            return null;
        }



        public void CreateAgent(string socketId)
        {
            if (!AgentOnlineList.ContainsKey(socketId))
            {
                int nowSt = KVDDDCore.Utils.TimeUtils.ConvertToTimeStampNow();
                AgentOnlineList.Add(socketId, new AgentClient()
                {
                    socketId = socketId,
                    HeartLastTimestamp = nowSt
                });

                SendMessageToAgent(socketId, "Enter Server Succ;");
                //ThreadPool.QueueUserWorkItem(async (a) =>
                //{
                //});

                if (AddAgentEvent != null)
                    AddAgentEvent(socketId);
            }
        }

        //移除Agent
        //需要回收Agent未完成的任务
        public void RemoveAgent(string socketId)
        {
            if (AgentOnlineList.ContainsKey(socketId))
            {
                AgentOnlineList.Remove(socketId);

                //TODO: 需要回收Agent未完成的任务

                if (RemoveAgentEvent != null)
                    RemoveAgentEvent(socketId);
            }
        }

        public void SendMessageToAgent(string socketId, string message) {

            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                await ChatWebSocketMiddleware.SendString(socketId, message);

            });
            //    System.Threading.Tasks.Task.Run(async () =>
            //{
            //});
        }

        public void SendMessageToSipAgent(string message)
        {
            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                if(SipAgent != null)
                {
                    await ChatWebSocketMiddleware.SendString(SipAgent.socketId, message);
                }
            });
        }


        public void ReceiveMessageFromAgent(string socketId, string message) {

            //SendMessageToAgent(socketId, message);
            if (!AgentOnlineList.ContainsKey(socketId))
            {
            }
            else
            {
                ThreadPool.QueueUserWorkItem(async (a) =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(message))
                        {
                            VidioProtoCS csdata = Newtonsoft.Json.JsonConvert.DeserializeObject<VidioProtoCS>(message);
                            if (csdata == null)
                                return;

                            if (csdata.type == "close")
                            {
                                _SipManager.ClientToCloseRoad(csdata, socketId);
                            }
                            if (csdata.type == "cmd")
                            {
                                _SipManager.ClientToStartRoad(csdata, socketId);
                            }

                        }
                    }
                    catch
                    {
                    }
                });
            

            }


        }

        public bool CheckIsSipServer(string socketId)
        {
            if (SipAgent == null)
                return false;

            return SipAgent.socketId == socketId;
        }



    }





}
