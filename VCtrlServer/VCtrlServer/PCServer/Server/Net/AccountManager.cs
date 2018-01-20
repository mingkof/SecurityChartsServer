using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server.Net
{
    public class AccountManager
    {
        Dictionary<int, string> uid2sid = new Dictionary<int, string>();
        Dictionary<string, int> sid2uid = new Dictionary<string, int>();

        public void AddUser(string socketid)
        {

        }

    }
}
