using ServerDBExt.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDBExt
{
    //class Test
    //{

    //    void TestOracle()
    //    {
    //        var connectionString = "user id=strike2014;password=strike@2014;data source=10.1.30.125:1521/powerdes";
    //        IDatabase db = new DatabaseSql(connectionString);

    //        if (db.DoEnsureOpen(null))
    //        {

    //            var res = db.Query("select * from T_2DTRA_1 where DATAITEM='廉洁风险'", null);

    //            foreach (var item in res)
    //            {
    //                var dic = item;

    //                StringBuilder sb = new StringBuilder();

    //                foreach (var dicitem in dic)
    //                {
    //                    sb.Append(dicitem.Key + ":" + dicitem.Value + "   ");
    //                }

    //                Console.WriteLine(sb.ToString());
    //            }

    //            db.DoEnsureClose();
    //        }
    //    }
    //}
}
