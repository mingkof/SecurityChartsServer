using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PCServer.Model;
using PCServer.Protos.protos;
using PCServer.TaskNodeServer;
using SHSecurityContext.IRepositorys;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    public class ChatWebSocketMiddleware
    {
        private static ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _sockets = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();

        private readonly RequestDelegate _next;

        public ChatWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public static System.Net.WebSockets.WebSocket GetSocket(string socketid)
        {
            if(_sockets.ContainsKey(socketid))
            {
                var socket = _sockets[socketid];

                if (socket.State != WebSocketState.Open)
                {
                    return null;
                }
                return socket;
            }
            return null;
        }


        private static string AddSocket(System.Net.WebSockets.WebSocket currentSocket)
        {
            string socketId = Guid.NewGuid().ToString();
            if (!_sockets.ContainsKey(socketId))
            {
                _sockets.TryAdd(socketId, currentSocket);
                AgentsManager.inst.CreateAgent(socketId);

                RunHeartbeat(socketId, currentSocket);
            }
            return socketId;
        }

        private static void RemoveSocket(string socketId)
        {
            if (_sockets.ContainsKey(socketId))
            {
                _sockets.TryRemove(socketId, out var socket);
                AgentsManager.inst.RemoveAgent(socketId);
            }
        }



        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            //System.Net.WebSockets.WebSocket dummy;

            CancellationToken ct = context.RequestAborted;
            var currentSocket = await context.WebSockets.AcceptWebSocketAsync();

            string socketId = AddSocket(currentSocket);

            //string socketId = context.Request.Query["sid"].ToString();

            //_sockets.TryRemove(socketId, out dummy);
            //_sockets.TryAdd(socketId, currentSocket);

            //后期增加socketid和userid的绑定关系


            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                //读取PCServerConfig配置
                var sipRepo = serviceScope.ServiceProvider.GetService<ISipPortRepository>();


                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    string recMsg = await ReceiveMessageAsync(socketId, currentSocket, ct);

                    Logmng.Logger.Debug("SocketID:" + socketId + "  Client: " + recMsg);

                    CheckHeart(socketId, currentSocket, recMsg);

                    AgentsManager.inst.ReceiveMessageFromAgent(socketId, recMsg);
                }
            }
        

            //_sockets.TryRemove(socketId, out dummy);

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }


  

        //https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Server/Server.cs
        private static async Task<string> ReceiveMessageAsync(string socketid, System.Net.WebSockets.WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            byte[] receiveBuffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = await socket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

            string s = System.Text.Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

            return s;
        }

        //https://www.cnblogs.com/webabcd/archive/2013/10/10/3360490.html
        private static async Task SendStringAsync(string msg , string socketid, System.Net.WebSockets.WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var bytes = Encoding.UTF8.GetBytes(msg) ;

            await socket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }


        public static async Task SendString(string socketId, string msg)
        {
            if (!_sockets.ContainsKey(socketId))
                return;

            var socket = _sockets[socketId];

            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            //socket.Key == msg.ReceiverID ||
            await SendStringAsync(msg, socketId, socket, default(CancellationToken));
        }

        public static void CheckHeart(string socketId, System.Net.WebSockets.WebSocket socket, string msg)
        {

            if (msg == "heart" || msg == "sipheart")
            {
                var agent = AgentsManager.inst.GetAgent(socketId);
                if(agent !=  null)
                {
                    int nowSt = KVDDDCore.Utils.TimeUtils.ConvertToTimeStampNow();
                    agent.HeartLastTimestamp = nowSt;

                    AgentsManager.inst.SendMessageToAgent(socketId, "heart");

                    if(msg == "sipheart")
                    {
                        AgentsManager.inst.SipAgent = agent;
                    }
                }
            }
        }

        public static void RunHeartbeat(string socketId, System.Net.WebSockets.WebSocket socket)
        {
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    var agent = AgentsManager.inst.GetAgent(socketId);
                    if (agent != null)
                    {
                        int nowSt = KVDDDCore.Utils.TimeUtils.ConvertToTimeStampNow();

                        //检测心跳时间大于的秒数
                        int heartTimeout = 30;

                        if (nowSt - agent.HeartLastTimestamp > heartTimeout)
                        {
                            RemoveSocket(socketId);
                            break;
                        }
                    } else
                    {
                        RemoveSocket(socketId);
                        break;
                    }

                    //每隔20秒检测  (5000=5秒)
                    Thread.Sleep(20000);
                }
            });
        }

    }


}
