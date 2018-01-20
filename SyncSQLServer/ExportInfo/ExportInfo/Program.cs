using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using KVDDDCore.Utils;
using System.Threading;
namespace ExportInfo
{
    class Program
    {

        static void Main(string[] args)
        {
            new SqlDataServer();

            Console.ReadLine();
        }
    }
}

