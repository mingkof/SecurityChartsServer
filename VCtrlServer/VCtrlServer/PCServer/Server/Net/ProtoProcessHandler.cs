using Onlineproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    //协议业务处理
    public class ProtoProcessHandler
    {
        public async Task Cs0x0100GameLogin(Message msg)
        {
            var ProtoData = msg.ReadProto<Cs0x0100GameLogin>();

            //业务处理


            var data = new Onlineproto.Sc0x0100GameLogin();
            data.Diamonds = 1001;
            data.CheckCode = 1;
            data.Player = new GPlayer()
            {
                Id = 10001
            };
            data.Time = 123456;

            await MessageManager.Inst.SendMessage(msg.socketid, "0x0100", data);
        }




    }
}
