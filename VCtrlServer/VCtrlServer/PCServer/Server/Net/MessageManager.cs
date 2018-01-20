using Onlineproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    public class MessageManager
    {
        private static MessageManager _inst;

        public static MessageManager Inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = new MessageManager();
                }
                return _inst;
            }
        }

        public async Task<bool> SendMessage<T>(string socketid, string protoId, T data)
        {
            //var buffer = Encoding.UTF8.GetBytes(data);
            var socket = ChatWebSocketMiddleware.GetSocket(socketid);
            if (socket == null)
                return false;

            SCMessage<T> msg = new SCMessage<T>();
            msg.Reset();

            var buffer =  msg.GetProtoData(protoId, data);
            await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, true, CancellationToken.None);
            return true;
        }


    }
}
