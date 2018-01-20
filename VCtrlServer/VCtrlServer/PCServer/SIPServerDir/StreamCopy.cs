using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SHSecurityContext.IRepositorys;

namespace SIP_WS
{
  public  class StreamCopy
    {
        //UdpClient RSocket;
        //UdpClient CSocket;

        //Thread mThread;
        //Thread mThreadRTCP;

        //IPEndPoint UEndPoint;
        //Socket USocket;

        public static IntPtr hSession;
        public int Port;
        public string DeviceID;

        public StreamCopy()
        { }

        public void Start(ISipPortRepository sipRepo, string InDeviceID, string InIP, int InPort)
        {
            Port = InPort;
            DeviceID = InDeviceID;

            GB28181.GB28181SessionParams sp = new GB28181.GB28181SessionParams();
            sp.mType = (uint)EGB28181SessionType.kRealTime;
            //sp.mSenderID = "31010821001320001062";
            sp.mSenderID = InDeviceID;
            sp.mReceiverID = "";
            sp.mStartTime = "";
            sp.mEndTime = "";
            sp.mReceiverIP = InIP;// "15.160.16.90";
            sp.mReceiverPort = InPort;
            sp.mSSRC = InPort;

            hSession = SipServerEntry.SIPServer.StartSession(sp);
            Console.WriteLine("Start Session : " + hSession);

            int myi = (int)hSession;
            var query = sipRepo.Find(p => p.pushToIp == InIP && p.pushToPort == InPort.ToString());
            if(query != null)
            {
                if(query.sipSession != 0)
                {
                    SipServerEntry.StopSession(query.sipSession);
                }

                query.sipSession = myi;
                sipRepo.Update(query);
            } else
            {
                sipRepo.Add(new SHSecurityModels.sys_sipport()
                {
                    Id = KVDDDCore.Utils.TimeUtils.ConvertToTimeStampNow().ToString(),
                    pushToIp = InIP,
                    pushToPort = InPort.ToString(),
                    sipSession = myi
                });
            }
            //UEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 30000 + port);
            //USocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);

            //// media receiver
            //IPEndPoint REndPoint = new IPEndPoint(IPAddress.Any, port);
            //RSocket = new UdpClient(REndPoint);

            //// rtcp receiver
            //IPEndPoint CEndPoint = new IPEndPoint(IPAddress.Any, port + 1);
            //CSocket = new UdpClient(CEndPoint);

            //mThread = new Thread(RTCPProc);
            //mThread.Start();

            //mThreadRTCP = new Thread(MediaReceiverProc);
            //mThreadRTCP.Start();
        }
        public void Stop()
        {
            //USocket.Close();
            //RSocket.Close();
            //CSocket.Close();

            //mThread.Abort();
            //mThreadRTCP.Abort();

            SipServerEntry.SIPServer.StopSession(hSession);
            hSession = (IntPtr)0;
        }
        //void MediaReceiverProc(object obj)
        //{
        //    IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //    byte[] sendBuf = new byte[64 * 4096];

        //    while (true)
        //    {
        //        byte[] buf = RSocket.Receive(ref remoteIPEndPoint);
        //        if (buf == null || buf.Length <= 0)
        //            break;

        //        if (buf.Length < 12)
        //            continue;

        //        Buffer.BlockCopy(buf, 12, sendBuf, 0, buf.Length - 12);
        //        USocket.SendTo(sendBuf, buf.Length - 12, SocketFlags.None, UEndPoint);
        //    }
        //}
        //void RTCPProc(object obj)
        //{
        //    //int len;
        //    byte[] send_data = new byte[1024];
        //    string CNAME = "(none)";

        //    IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //    while (true)
        //    {
        //        byte[] buf = CSocket.Receive(ref remoteIPEndPoint);
        //        if (buf == null || buf.Length <= 0)
        //        {
        //            break;
        //        }

        //        if (buf[1] >= 200)
        //        {
        //            int rtcp_type = buf[1];
        //            if (rtcp_type == 200 || rtcp_type == 202)
        //            {
        //                //uint mySSRC = 0x290F123E;

        //                send_data[0] = 0x80;
        //                send_data[1] = 201;
        //                send_data[2] = 0x00;
        //                send_data[3] = 0x01;
        //                send_data[4] = 0x29;
        //                send_data[5] = 0x0F;
        //                send_data[6] = 0x12;
        //                send_data[7] = 0x33;


                        
        //                int sdes_len = (CNAME.Length + 2 + 3) / 4 + 1;
        //                send_data[8] = 0x81;
        //                send_data[9] = 202;
        //                send_data[10] = 0x00;
        //                send_data[11] = (byte)sdes_len;


        //                send_data[12] = 0x29;
        //                send_data[13] = 0x0F;
        //                send_data[14] = 0x12;
        //                send_data[15] = 0x33;

        //                send_data[16] = 0x01;//CName type
        //                send_data[17] = (byte)CNAME.Length;
        //                send_data[18] = (byte)CNAME[0];
        //                send_data[19] = (byte)CNAME[1];
        //                send_data[20] = (byte)CNAME[2];
        //                send_data[21] = (byte)CNAME[3];
        //                send_data[22] = (byte)CNAME[4];
        //                send_data[23] = (byte)CNAME[5];
        //                RSocket.Send(send_data, 24, remoteIPEndPoint);
        //            }
        //            continue;
        //        }

        //    }
        //}
    }
}
