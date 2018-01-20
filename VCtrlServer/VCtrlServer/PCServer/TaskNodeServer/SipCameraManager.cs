using Microsoft.Extensions.DependencyInjection;
using PCServer.Server;
using PCServer.Server.Proto_Gongan;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.TaskNodeServer
{
    public class SipCameraManager
    {
        string baseRtspUrl = "rtsp://38.104.104.5:554/";
        public List<sys_sipport> portsMngList = new List<sys_sipport>();

        ISipPortRepository sipRepo = null;

        public void Init()
        {
            var serviceScope = ServiceLocator.Instance.CreateScope();
            sipRepo = serviceScope.ServiceProvider.GetService<ISipPortRepository>();

       
            portsMngList = sipRepo.FindList(p => true, "",false).ToList();

            AgentsManager.inst.AddAgentEvent += Inst_AddAgentEvent;
            AgentsManager.inst.RemoveAgentEvent += Inst_RemoveAgentEvent;
        }

        public void InitConfig()
        {
            baseRtspUrl = PCServerMain.Instance.PCServerConfig.ext["RtspBaseUrl"];
            //int VedioCount = int.Parse(PCServerMain.Instance.PCServerConfig.ext["VedioCount"]);
        }

        private void Inst_RemoveAgentEvent(string socketId)
        {
            Logmng.Logger.Debug("RemoveAgentEvent: " + socketId);

            bool isSipServer = AgentsManager.inst.CheckIsSipServer(socketId);

            if(!isSipServer)
            {
                ClearAllCameraSession();
            } else
            {
                AgentsManager.inst.SipAgent = null;
            }
        }

        private void Inst_AddAgentEvent(string socketId)
        {

        }

        public void ClientToCloseRoad(VidioProtoCS csdata,string socketId)
        {
            string cameraId = csdata.value;
            string auth = csdata.autho;

            var query = FindHadRoad(cameraId);
            if (query != null)
            {
                query.CameraId = "";
                UpdatePortsSqlData(query);
            }
        }

        public void ClientToStartRoad(VidioProtoCS csdata, string socketId)
        {
            string cameraId = csdata.value;
            string auth = csdata.autho;

            sys_sipport portData = null;

            var queryHad = FindHadRoad(cameraId);
            if (queryHad == null)
            {
                int idleCount = FindIdleCount(out List<sys_sipport> idleList);

                if (idleCount <= 0 || idleList == null)
                {
                    SendToClient(socketId, "res", "noidle", csdata.autho);
                    return;
                }

                portData = idleList[0];
            }
            else
            {
                portData = queryHad;
            }

            if (AgentsManager.inst.SipAgent != null)
            {
                //发送给sip Server
                SendToSipServer(cameraId, portData.Id,portData.pushToIp, portData.pushToPort, "start");
                //等待2秒
                //Thread.Sleep(2000);

                //更新
                portData.CameraId = cameraId;
                UpdatePortsSqlData(portData);

                //再通知回VClient
                SendToClient(socketId, "res", baseRtspUrl + portData.Id + ".sdp" , csdata.autho);
            }
            else
            {
                //sip服务器没有
                SendToClient(socketId, "res", "server is null", csdata.autho);
            }
        }


       void UpdatePortsSqlData(sys_sipport data)
        {
            var query = sipRepo.Find(p => p.Id == data.Id);
            if(query == null)
            {
                sipRepo.Add(data);
            } else
            {
                query.CameraId = data.CameraId;
                query.pushToIp = data.pushToIp;
                query.pushToPort = data.pushToPort;
                query.sipSession = data.sipSession;

                sipRepo.Update(query);
            }
        }

        int FindIdleCount(out List<sys_sipport> idleList)
        {
            idleList = null;

            var List = portsMngList.Where(p => p.CameraId == null || p.CameraId == "");
            if (List == null)
                return 0;
            idleList = List.ToList();

            return idleList.Count();
        }

        sys_sipport FindHadRoad(string cameraId)
        {
            var query = portsMngList.Where(p => p.CameraId == cameraId).FirstOrDefault();
            return query;
        }

        void ClearAllCameraSession()
        {
            for (int i = 0; i < portsMngList.Count; i++)
            {
                portsMngList[i].CameraId = "";

                UpdatePortsSqlData(portsMngList[i]);
            }
        }

        void SendToClient(string socketId, string type, string value, string autho)
        {
            VidioProtoSC resdata = new VidioProtoSC()
            {
                type = type,
                value = value,
                autho = autho
            };
            AgentsManager.inst.SendMessageToAgent(socketId, Newtonsoft.Json.JsonConvert.SerializeObject(resdata));
        }

        void SendToSipServer(string cameraId, string sdpId, string  ip, string port, string type = "start")
        {
            //通知sip去切换视频源  1-5路
        
            SipProtoSC sipsc = new SipProtoSC()
            {
                type = type,
                value = cameraId,
                sdp = sdpId,
                ip = ip,
                port = port
            };
            AgentsManager.inst.SendMessageToSipAgent(Newtonsoft.Json.JsonConvert.SerializeObject(sipsc));
        }
    }
}
