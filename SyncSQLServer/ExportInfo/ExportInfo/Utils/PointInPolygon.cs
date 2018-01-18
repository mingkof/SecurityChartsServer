using System;
using System.Collections.Generic;
using System.Drawing;
//using System.Numerics;
using System.Text;


namespace KVDDDCore.Utils
{

    public class KPoint
    {
        public long X { get; set; }
        public long Y { get; set; }
        public KPoint(long _x,long _y)
        {
            X = _x;
            Y = _y;
        }
        public KPoint()
        {

        }
    }

   public class PointInPolygon
    {
       /// <summary>
       /// 点是否在 区域内
       /// poly点，从最左下角沿着顺时针设置数组
       /// 在线上返回false的
       /// </summary>
       /// <param name="p"></param>
       /// <param name="poly"></param>
       /// <returns></returns>
       public static bool CheckPointInPolygon(KPoint p, KPoint[] poly)
        {

            KPoint p1, p2;



            bool inside = false;



            if (poly.Length < 3)

            {

                return inside;

            }



            KPoint oldKPoint = new KPoint(

            poly[poly.Length - 1].X, poly[poly.Length - 1].Y);



            for (int i = 0; i < poly.Length; i++)

            {

                KPoint newKPoint = new KPoint(poly[i].X, poly[i].Y);



                if (newKPoint.X > oldKPoint.X)

                {

                    p1 = oldKPoint;

                    p2 = newKPoint;

                }

                else

                {

                    p1 = newKPoint;

                    p2 = oldKPoint;

                }



                if ((newKPoint.X < p.X) == (p.X <= oldKPoint.X)

                && ((long)p.Y - (long)p1.Y) * (long)(p2.X - p1.X)

                 < ((long)p2.Y - (long)p1.Y) * (long)(p.X - p1.X))

                {

                    inside = !inside;

                }



                oldKPoint = newKPoint;

            }



            return inside;

        }

    }
}
