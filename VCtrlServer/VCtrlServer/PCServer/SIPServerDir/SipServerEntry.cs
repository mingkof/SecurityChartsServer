using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Timers;
using SHSecurityContext.IRepositorys;

namespace SIP_WS
{
    // ffmpeg.exe -i "udp://127.0.0.1:36030" -an -c:v libx264 -rtsp_transport tcp -f rtsp rtsp://15.160.16.120:554/4.sdp
    // ffmpeg.exe -i "udp://127.0.0.1:36000" -an -c:v copy -rtsp_transport tcp -f rtsp rtsp://15.160.16.120:554/push.sdp
   public class SipServerEntry
    {
        public static LocalServer SIPServer;
        public static List<StreamCopy> mAllStream = new List<StreamCopy>();

        public static ISipPortRepository _sipRepo;

        public static void Start(ISipPortRepository sipRepo)
        {
            //开启WS Server链接
            //StartWSServer();
            _sipRepo = sipRepo;

            //创建SIP Server实例
            SIPServer = new LocalServer();
            if (!SIPServer.StartServer())
                return;
            /*
             *   
             *   int转IntPtr

           int i = 12;
           IntPtr p = new IntPtr(i);

           IntPtr转int

           int myi = (int)p;
           MessageBox.Show(myi.ToString());

             * */

            //初始化AllStream,如果发现有session，则全部都关闭
            var list = _sipRepo.FindList(p => p.sipSession != 0, "", false).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                int session = list[i].sipSession;
                StopSession(session);
            }

            //命令行控制SIP链接
            //bool Finish = false;
            //while (!Finish)
            //{
            //    string cmd = Console.ReadLine();
            //    if (cmd == null)
            //        continue;
            //    string[] pars = cmd.Split(' ');

            //    if (pars.Length <= 0)
            //        continue;
            //    switch (pars[0].ToLower())
            //    {
            //        case "quit": Finish = true; break;
            //        case "updatecatalog": SIPServer.UpdateCatalog(); break;
            //        case "startsession": StartSession(pars); break;
            //        case "stopsession": StopSession(); break;
            //        case "queryrecord": QueryRecord(); break;
            //        default: Console.WriteLine("invalid"); break;
            //    }
            //}

            //SIPServer.StopServer();
        }

        public static void StopSession(int session)
        {
            IntPtr pSession = new IntPtr(session);
            SIPServer.StopSession(pSession);
        }
        /// <summary>
        /// Stop SIP Server 
        /// </summary>
        static void StopServer()
        {
            StopSession();
            SIPServer.StopServer();
        }


      public  static void QueryRecord()
        {
            SIPServer.QueryRecordInfo("31010821001180001015", (int)EGB28181RecordType.kAll, "2017-12-15T00:00:00", "2017-12-15T23:59:59");
        }

        /// <summary>
        /// 开始取流
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="TargetPort"></param>
       public static void StartSession(string DeviceID, string InIP, string InPort)
        {
            int port = 0;
            string deviceID = "";
            if (int.TryParse(InPort, out port))
            {
                deviceID = DeviceID;

                //检测原来该端口下是否已经在取流，已经在的停掉
                for (int i = 0; i < mAllStream.Count; ++i)
                {
                    if (mAllStream[i].Port == port)
                    {
                        mAllStream[i].Stop();
                        mAllStream.RemoveAt(i);
                        break;
                    }
                }
                mAllStream.Add(StartSession(deviceID, InIP, port));
            }
        }

        /// <summary>
        /// 命令行下---取流
        /// </summary>
        /// <param name="pars">对象类型</param>
        public static void StartSession(string[] pars)
        {
            if (pars.Length == 1)
            {
                StopSession();
                mAllStream.Add(StartSession("31010811001180007011", "15.160.16.90", 6040));
            }
            else if (pars.Length == 3)
            {
                StartSession(pars[2], "15.160.16.90", pars[1]);
            }
            else
            {
                Console.WriteLine("invalid params");
            }

            //mAllStream.Add(StartSession("31010821001320001062", 6000));
            //mAllStream.Add(StartSession("31010821001320001063", 6010));
            //mAllStream.Add(StartSession("31010821001320001064", 6020));
            //mAllStream.Add(StartSession("31010811001180007011", 6030));
        }
        public static StreamCopy StartSession(string InDeviceID, string InIP, int InPort)
        {
            StreamCopy stream = new StreamCopy();
            stream.Start(_sipRepo,InDeviceID, InIP, InPort);
            return stream;
        }
        public static void StopSession()
        {
            for (int i = 0; i < mAllStream.Count; ++ i)
            {
                mAllStream[i].Stop();
            }
            mAllStream.Clear();
        }


        #region WS Methods

        /// <summary>
        /// 保留全局引用
        /// </summary>
        //private static WebSocket wsInstance;
        private static bool isDebug = true;
        private static int MaxSDPRouter = 5;
        private static string[] UsedSdps = new string[MaxSDPRouter];


        /// <summary>
        /// 开启WS连接
        /// </summary>
        //private static void StartWSServer()
        //{
        //    using (var ws = new WebSocket("ws://15.160.16.120:9100"))
        //    {

        //        //接受消息
        //        ws.OnMessage += (sender, e) =>
        //        {
        //            if (isDebug)
        //                Console.WriteLine("Get Msg: " + e.Data);

        //            //处理收到的数据
        //            HandleRecvData(e.Data);
        //        };

        //        //打开WS
        //        ws.OnOpen += (sender, e) =>
        //        {
        //            if (isDebug)
        //                Console.WriteLine("WS Open! ");
        //        };

        //        //关闭WS
        //        ws.OnClose += (sender, e) =>
        //        {
        //            if (isDebug)
        //                Console.WriteLine("WS Close Say: " + e.Reason);
        //        };

        //        //报错提示
        //        ws.OnError += (sender, e) =>
        //        {
        //            if (isDebug)
        //                Console.WriteLine("WS Error: " + e.Message);
        //        };


        //        ws.Connect();
        //        wsInstance = ws;

        //        //定时发送Heart包
        //        Timer t = new Timer(1000);
        //        t.Elapsed += new ElapsedEventHandler(TimerToSendHeart);
        //        t.Enabled = true;
        //    }
        //}



        /// <summary>
        /// 处理收到的数据
        /// </summary>
        /// <param name="content"></param>
        private static void HandleRecvData(string content)
        {
            if (content == "heart")
            {
                //心跳包回复，不做处理
                if (isDebug)
                    Console.Write("handle: heart");
            }
            else
            {
                if (content.StartsWith("{"))
                {
                    JObject jo = (JObject)JsonConvert.DeserializeObject(content);
                    string requestID = jo["value"].ToString();      //请求播放的camera id
                    string sdp = jo["sdp"].ToString();      //请求的sdp router

                    //TODO: play && stop 
                    if (UsedSdps[int.Parse(sdp)-1] != string.Empty)
                    {
                        //原来播放的需要暂停：stop

                    }
                    //然后播放新的requestID Play
                    //StartSession(requestID, sdp);
                }

            }
        }



        ///// <summary>
        ///// 定时方法
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="e"></param>
        //private static void TimerToSendHeart(object source, ElapsedEventArgs e)
        //{
        //    SendHeartPackage(wsInstance);
        //}

        ///// <summary>
        ///// 发送心跳包
        ///// </summary>
        ///// <param name="ws"></param>
        //public static void SendHeartPackage(WebSocket ws)
        //{
        //    if (isDebug)
        //        Console.Write("Send: heart");

        //    if (ws != null && ws.IsAlive)
        //        ws.Send("sipheart");
        //}

        ///// <summary>
        ///// 发送数据包
        ///// </summary>
        ///// <param name="ws"></param>
        ///// <param name="content"></param>
        //public static void SendStringContent(WebSocket ws,string content)
        //{
        //    if (!isDebug)
        //        Console.Write("Send: " + content);

        //    if(ws!=null && ws.IsAlive)
        //        ws.Send(content);


        //}

        ///// <summary>
        ///// 发送协定的数据包请求数据
        ///// </summary>
        ///// <param name="ws"></param>
        ///// <param name="pkg"></param>
        //public static void SendConnectionPackage(WebSocket ws,ConnectionPackage pkg)
        //{
        //    string content = string.Empty;
        //    content = JsonConvert.SerializeObject(pkg);

        //    Console.WriteLine("Json send: " + content+"/n");
        //    SendStringContent(ws, content);
        //}

        //public class ConnectionPackage {
        //    public string type { get; set; }
        //    public string value { get; set; }
        //    public string sdp { get; set; }

        //}

        #endregion
    }
}
