using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    //Sequence消息序号 byte[2] +  
    //     协议号 byte[2] + 协议数据长度 byte[2] + 协议数据内容

    public class SCMessage<T>
    {
        public const int HEAD_SIZE = 6;

        int seq = 0;
        ushort protoIdInt = 0;
        int msgSize = 0;

        int index;
        byte[] buffer;

        public void Reset() {
            buffer = new byte[1024 * 64];
        }

        public byte[] GetProtoData(string protoid, T data)
        {
            //protoid  0x0100 解析成ushort

            ushort protoIdInt = 256;

            WriteHead(protoIdInt);
            WriteProto(data);

            return buffer;
        }


        public void WriteHead(ushort inMsgID)
        {
            protoIdInt = inMsgID;

            WriteUInt16(buffer, 0, protoIdInt);

            index = 2;
        }


        public int WriteProto(T t)
        {
            int len = 0;
            using (MemoryStream ms = new MemoryStream())
            {
                Serializer.Serialize<T>(ms, t);

                byte[] data = ms.ToArray();
                len = data.Length;

                WriteBytes(data);
            }
            return len;
        }


        void WriteBytes(byte[] bytes)
        {
            //Array.Copy(bytes, 0, buffer, index, bytes.Length);
            for (int i = 0; i < bytes.Length; i++)
                buffer[index + i] = bytes[i];

            index += bytes.Length;
        }

        public static void WriteInt32(byte[] buff, int index, int b)
        {
            /*buff[index] = (byte)(b << 24 >> 24);
            buff[index + 1] = (byte)(b << 16 >> 24);
            buff[index + 2] = (byte)(b << 8 >> 24);
            buff[index + 3] = (byte)(b >> 24);*/

            byte[] data = BitConverter.GetBytes(b);
            data.CopyTo(buff, index);
        }
        public static void WriteUInt16(byte[] buff, int index, ushort b)
        {
            //use Little Endian
            buff[index] = (byte)(b << 24 >> 24);
            buff[index + 1] = (byte)(b << 16 >> 24);

            //byte[] data = BitConverter.GetBytes(b);
            //data.CopyTo(buff, index);
        }


    }
}
