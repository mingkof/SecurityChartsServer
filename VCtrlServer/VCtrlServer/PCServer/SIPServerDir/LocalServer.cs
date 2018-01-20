using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using GB28181;
using System.Xml;

namespace SIP_WS
{
    public enum EGB28181_CMD
    {
        //设备目录命令
        kCATALOG = 1,
        //设备信息命令
        kDEVICE_INFO = 2,
        //设备状态命令
        kDEVICE_STATUS = 3,
        //设备记录文件列表
        kRECORD_INFO = 4,
        //云台控制命令
        kDEVICE_CONTROL_PTZ = 5,
        //启动，停止手动录像命令
        kDEVICE_CONTROL_RECORD = 6,
        //布放车坊命令
        kDEVICE_CONTROL_GUARD = 7,
        //报警命令 
        kDEVICE_CONTROL_ALARM = 8,
        //复位报警命令
        kDEVICE_CONTROL_RESET_ALARM = 9,
        //重启前端设备命令
        kDEVICE_CONTROL_TELEBOOT = 10,
    };

    public enum EGB28181SessionType
    {
        kRealTime = 0,
        kPlayback = 1,
        kDownload = 2,
    }
    public enum EGB28181RecordType
    {
        kAll        = 0, // 查询所有的文件
        kTime       = 1, // 查询定时录像文件
        kManual     = 2, // 查询手动录像文件
        kAlarm      = 3, // 查询报警录像文件
    }
  public  class LocalServer : GB28181LocalServer
    {
        FileStream mFile;
        StreamWriter Writer;
        bool mConnected = false;
        string mConnectID;

        void SendBaseCMD(int InCmd, int InSubCmd, string InStrID)
        {
            GB28181CMDParams p = new GB28181CMDParams()
            {
                mCMD = InCmd,
                mSubCMD = InSubCmd,
                mID = InStrID
            };
            SendCMD(p);
        }

        public LocalServer()
        {
            mFile = File.OpenWrite("test.xml");
            Writer = new StreamWriter(mFile);
        }
        public bool StartServer()
        {
            GB28181ServerInfo localServerInfo = new GB28181ServerInfo();
            localServerInfo.mID = "31010601002000000011";
            localServerInfo.mRealm = "31010601";
            localServerInfo.mIP = "15.160.16.90";
            localServerInfo.mPort = 6050;
            localServerInfo.mPassword = "123456";

            return Start(localServerInfo);
        }
        public void StopServer()
        {
            Stop();
        }

        public void UpdateCatalog()
        {
            if (!mConnected)
                return;

            SendBaseCMD((int)EGB28181_CMD.kCATALOG, 0, mConnectID);
        }

        #region SIP系统回调函数

        // 注销服务器，注册服务器
        public override void OnServerRegistration(bool InUnregister, bool InSuccess)
        {
            base.OnServerRegistration(InUnregister, InSuccess);
        }
        // 当收到文件播放或者下载结束时会调用该函数
        public override void OnSessionEnd(IntPtr InSession)
        {
            Console.WriteLine("Session End." + InSession);
            base.OnSessionEnd(InSession);
        }
        // 报警
        public override void OnUserAgentAlarm(string InStrID, string InStrXML)
        {
            base.OnUserAgentAlarm(InStrID, InStrXML);
        }
        // 心跳包
        public override void OnUserAgentKeepalive(string InStrID, string InStrXML, string InStrStatus)
        {
            //Console.WriteLine("心跳：" + InStrID);
            //Console.WriteLine(InStrXML);
            //Console.WriteLine();
        }
        // 注销，注册
        public override void OnUserAgentRegistration(string InStrID, bool InUnregister)
        {
            if (InUnregister)
            {
                Console.WriteLine("注销：" + InStrID);
                mConnected = false;
            }
            else
            {
                Console.WriteLine("注册：" + InStrID);
                mConnected = true;
                mConnectID = InStrID;
                
                //SendBaseCMD((int)EGB28181_CMD.kDEVICE_INFO, 0, InStrID);
                //SendBaseCMD((int)EGB28181_CMD.kDEVICE_STATUS, 0, InStrID);
            }
            Console.WriteLine();
        }
        // 设备目录
        public override void OnUserAgentResponseCatalog(string InStrID, string InStrXML, bool InLast)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(InStrXML);
            XmlNode nItem = doc.SelectSingleNode("/Response/DeviceList/Item");
            string t = "";
            foreach (XmlNode node in nItem.ChildNodes)
            {
                t += node.InnerText + ",";
            }

            Writer.WriteLine(t);
            Writer.Flush();
            if (InLast)
            {
                mFile.Close();
                Console.WriteLine("finish");
            }
            else
            {
                Console.Write(".");
            }

            //Console.WriteLine("设备目录：" + InStrID);
            //Console.WriteLine(InStrXML);
            //Console.WriteLine();
        }
        // 当服务器向前端设备发送有应答命令时，28181服务器收到响应后 会调用该函数
        public override void OnUserAgentResponseDeviceControl(string InStrID, string InStrXML)
        {
            base.OnUserAgentResponseDeviceControl(InStrID, InStrXML);
        }
        // 设备信息
        public override void OnUserAgentResponseDeviceInfo(string InStrID, string InStrXML, bool InLast)
        {
            Console.WriteLine("设备信息：" + InStrID);
            Console.WriteLine(InStrXML);
            Console.WriteLine();
        }
        // 设备状态
        public override void OnUserAgentResponseDeviceStatus(string InStrID, string InStrXML, bool InLast)
        {
            Console.WriteLine("设备状态：" + InStrID);
            Console.WriteLine(InStrXML);
            Console.WriteLine();
        }
        // 当服务器向前端设备查询录像文件时，28181服务器收到响应后 会调用该函数
        public override void OnUserAgentResponseRecordInfo(string InStrID, string InStrXML, bool InLast)
        {
            Console.WriteLine("查询录像文件：" + InStrID);
            Console.WriteLine(InStrXML);
            Console.WriteLine();
        }

        #endregion
    }
}
