using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    //     Sequence消息序号 byte[2] +  协议号 byte[2] + 协议数据长度 byte[2] + 协议数据内容
    public class Message
    {
        public const int HEAD_SIZE = 6;

        public int seq = 0;
        public int protoIdInt = 0;
        public int msgSize = 0;
        public byte[] msgBuffer;

        private byte[] rawBuffer;

        public string socketid;

        public Message (string _socketid, byte[] _rawBuffer)
        {
            socketid = _socketid;
            rawBuffer = _rawBuffer;

            //分析buffer得到head
            ReadHead();
        }

        public bool CheckAvalid()
        {
            if(seq == 0 || protoIdInt == 0 || msgSize == 0)
                return false;
            return true;
        }


        public void ReadHead()
        {
            int beginIndex = 0;
            seq = ReadInt16(rawBuffer, 0, 2, true);
            protoIdInt = ReadInt16(rawBuffer, beginIndex += 2, 2, true);  //协议号整形
            msgSize = ReadInt16(rawBuffer, beginIndex += 2, 2, true);
        }


        public T ReadProto<T>()
        {
            var msgBuffer = ReadBytes(rawBuffer, HEAD_SIZE, msgSize);
            return ReadProto<T>(msgBuffer);
        }

        public T ReadProto<T>(byte[] buffer)
        {
            T t = default(T);
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                t = ProtoBuf.Serializer.Deserialize<T>(ms);
            }
            return t;
        }

        public byte[] ReadBytes(byte[] buffer, int start, int len, bool reverse = false)
        {
            byte[] ret = new byte[len];

            for (int i = 0; i < ret.Length; i++)
                ret[i] = buffer[start + i];

            if (reverse)
            {
                Array.Reverse(ret);
            }

            return ret;
        }



        int ReadInt16(byte[] buffer, int start, int length, bool reverse = false)
        {
            //var bytes = new ArraySegment<byte>(buffer, start, length);
            //var buf = bytes.ToArray();

            var bytes = ReadBytes(buffer, start, length, reverse);
            int res = BitConverter.ToInt16(bytes, 0);
            return res;
        }


    }
}
