using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PCServer.Protos.protos
{
    public class ProtoClassType
    {
        public  static Dictionary<string, string> protos = new Dictionary<string, string>();

        public static void Init()
        {
            protos.Add("0x0100", "Onlineproto.Cs0x0100GameLogin");

        }


        public static Type GetType (string classFullName)
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Type t = a.GetType("classFullName");
            return t;
        }


    }
}
