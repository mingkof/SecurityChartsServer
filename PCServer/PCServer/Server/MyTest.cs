using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKServerWeb.Server
{
    public class MyTest
    {
        public static void TestOracle(INodeServices nodeServices)
        {
            nodeServices.InvokeAsync<string>(
               "Node/test/oracleTest.js"
            );
        }

    }
}
