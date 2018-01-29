using Microsoft.Extensions.Options;
using MKServerWeb.Model.RealData;
using MKServerWeb.Server;
using Newtonsoft.Json;
using SHSecurityContext.IRepositorys;
using SHSecurityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace PCServer.Server.GPSSocket
{
    public class GPSSocketClient
    {
        string tb_ServerIP = "10.17.32.54";
        string tb_ServerPort = "19133";
        IAsyncResult result;
        public Socket clientSocket;

        public AsyncCallback pfnCallBack;
        //private static PoliceGPSArray dataArray;

        IPoliceGpsRepository _repo;
        RealDataUrl _realData;


        public void Run(IPoliceGpsRepository repo, IOptions<RealDataUrl> config)
        {
            Logmng.Logger.Trace("GPSSocketClient is Run 001");
            _repo = repo;
            _realData = config.Value;

            tb_ServerIP = _realData.GPSSocketServerIP;
            tb_ServerPort = _realData.GPSSocketServerPort;

            ConnectServer();

            //var query = _repo.Find(p => p.PoliceID == "test");
            //if (query == null)
            //{
            //    _repo.Add(new PoliceGPS()
            //    {
            //        PoliceID = "test",
            //        GPS_X = "0",
            //        GPS_Y = "0"
            //    });
            //}
        }

       public void ConnectServer() {
            if (tb_ServerIP == "" || tb_ServerPort == "")
            {
                Logmng.Logger.Error("GPSSocketClient: ConnectServer Error! ServerIP Or Port is NULL!");
                return;
            }
            try
            {
                UpdateStatusMsg(false);

                // Create the socket instance
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(tb_ServerIP), System.Convert.ToInt32(tb_ServerPort));

                // Connect to the remote host
                clientSocket.Connect(ipEnd);
                if (clientSocket.Connected)
                {
                    UpdateStatusMsg(true);
                    //Wait for data asynchronously 
                    WaitForData();
                }
            }
            catch (SocketException se)
            {
                Logmng.Logger.Error("连接服务器失败，服务器正在运行么?\n" + se.Message);
                Logmng.Logger.Error("IP/port: " + tb_ServerIP + " " + tb_ServerPort);
                UpdateStatusMsg(false);

            }
        }


        private void UpdateStatusMsg(bool connected)
        {
            //log
            Logmng.Logger.Trace("GPSSocketClient: Connected is " + connected);
        }
        private void WaitForData()
        {
            try
            {
                if (pfnCallBack == null)
                    pfnCallBack = new AsyncCallback(OnDataReceived);

                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = clientSocket;

                result = clientSocket.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnCallBack, theSocPkt);
            }
            catch (SocketException se)
            {
                Logmng.Logger.Error("GPSSockcetClient:" + se.Message);
            }
        }

        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[2048];
        }


        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);

                AppendReceivedMsg(new System.String(chars));

                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                //System.Diagnostics.Debugger.Log(0, "1", Environment.NewLine + "数据接收时: Socket 已关闭");
            }
            catch (SocketException se)
            {
                Logmng.Logger.Error("GPSSockcetClient:" + se.Message);
            }
        }

      


        private void AppendReceivedMsg(string msg)
        {
            UpdateReceivedMsg(msg);
            //if (InvokeRequired)
            //    tb_ReceiveMsg.BeginInvoke(new UpdateReceiveMsgCallback(UpdateReceivedMsg), msg);
            //else
            //    UpdateReceivedMsg(msg);
        }


        private void UpdateReceivedMsg(string msg)
        {
            if (msg != "LINK ")
            {
            }

            if (msg.Contains("LINK") || msg.Trim() == "LINK")
            {
                byte[] byData = System.Text.Encoding.UTF8.GetBytes("LINK");
                if (clientSocket != null)
                    clientSocket.Send(byData);

                Logmng.Logger.Trace(DateTime.Now + "  Send： " + "LINK");

                //tb_ReceiveMsg.AppendText(Environment.NewLine + DateTime.Now + "  Send： " + "LINK");
            }
            else
            {
                HandleMsg(msg);
            }
            Logmng.Logger.Trace(DateTime.Now + "  接收到数据： " + msg);

            //tb_ReceiveMsg.AppendText(Environment.NewLine + DateTime.Now + "  接收到数据： " + msg);
        }


        public void HandleMsg(string msg)
        {
            int timeNow = KVDDDCore.Utils.TimeUtils.ConvertToTimeStampNow();
            var DayNow = DateTime.Now;
            string nowYear = DayNow.Year.ToString();
            string nowMonth = DayNow.Month.ToString("00");
            string nowDay = DayNow.Day.ToString("00");

            string nowHour= DayNow.Hour.ToString("00");
            string nowMinute = DayNow.Minute.ToString("00");
            string nowSecond = DayNow.Second.ToString("00");


            //?083068N,JR50300,13:50:38,1,121.459702,31.237600,0.0,202.5,0,2,0,0,0,0,00000000軺
            string[] dt = msg.Split(',');
            PoliceGPS tamp = new PoliceGPS();
            tamp.PoliceID = dt[1];
            tamp.GPS_X = dt[4];
            tamp.GPS_Y = dt[5];
            tamp.Timestamp = timeNow;
            tamp.Year = nowYear;
            tamp.Month = nowMonth;
            tamp.Day = nowDay;
            tamp.HH = nowHour;
            tamp.MM = nowMinute;
            tamp.SS = nowSecond;

            var query = _repo.Find(p => p.PoliceID == tamp.PoliceID);
            if(query == null)
            {
                _repo.Add(tamp);
            }
            else
            {
                query.GPS_X = tamp.GPS_X;
                query.GPS_Y = tamp.GPS_Y;
                query.Timestamp = tamp.Timestamp;
                query.Year = tamp.Year;
                query.Month = tamp.Month;
                query.Day = tamp.Day;
                query.HH = tamp.HH;
                query.MM = tamp.MM;
                query.SS = tamp.SS;

                _repo.Update(query);
            }


            //统计更新区域内警员人次的历史记录
            //PCServerMain.Instance.PoliceGpsStaticAreaManager.UpdatePoliceAreaStatic(tamp);



            //bool find = false;
            //foreach (var item in dataArray.PoliceArray)
            //{
            //    if (item.PoliceID == tamp.PoliceID)
            //    {
            //        find = true;
            //        item.GPS_X = tamp.GPS_X;
            //        item.GPS_Y = tamp.GPS_Y;
            //    }
            //}

            //if (!find)
            //    dataArray.PoliceArray.Add(tamp);

            //string jsonData = JsonConvert.SerializeObject(dataArray);

            //Logmng.Logger.Trace("GPSSocketClient Data：" + jsonData);
            //string str = @"..\policeGPS.json";

            //if (!File.Exists(str))
            //{
            //    FileStream fs1 = new FileStream(str, FileMode.Create, FileAccess.ReadWrite);
            //    fs1.Close();
            //}
            //File.WriteAllText(str, jsonData);


            //model = JObject.Parse(jsonWearther.ToString());
        }


    }

    //public class PoliceGPSArray
    //{
    //    public List<PoliceGPS> PoliceArray;
    //}
}
