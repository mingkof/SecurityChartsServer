using Onlineproto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    delegate Task ProtoProcessDelegate(Message msg);

    public class ProtoProcess
    {
        public bool BeRegisted = false;
        private ProtoProcessHandler Processer = new ProtoProcessHandler();
        private Dictionary<string, ProtoProcessDelegate> processHandler = new Dictionary<string, ProtoProcessDelegate>();
   


        public void Register ()
        {
            //协议增加  这里需增加注册!
            processHandler.Add("0x0100", Processer.Cs0x0100GameLogin);


            BeRegisted = true;
        }



        public void Run(Message message)
        {
            if (!BeRegisted)
            {
                Logmng.Logger.Error("ProtoProcess Dont Register, Check appsetting.json UseProtobuf's Value is true");
                return;
            }


            int protoIdInt = message.protoIdInt;
            if (protoIdInt == 0)
            {
                Logmng.Logger.Error("ProtoProcess Run Error: ProtoIdInt is 0");
                return;
            }

            string protoId = String.Format("{0:X}", protoIdInt);
            protoId = "0x" + protoId.PadLeft(4, '0');


            if (!processHandler.ContainsKey(protoId))
            {
                Logmng.Logger.Error("ProtoProcess Run Error: has no register for protoId : " + protoIdInt + "/" + protoId);
                return;
            }


            ThreadPool.QueueUserWorkItem(async (a) =>
            {
                await processHandler[protoId].Invoke(message);
            });

        }



    }
}
