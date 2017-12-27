using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCServer.Server.GPS
{

    public class Vector3
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }

        public Vector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    class BMapPixel
    {
        public int X, Y;

        public BMapPixel()
        {
            X = Y = 0;
        }
        public BMapPixel(int x, int y)
        {
            X = x;
            Y = y;
        }
    };
    class BMapPoint
    {
        public double longitude;
        public double latitude;

        public BMapPoint()
        {
            longitude = latitude = 0;
        }
        public BMapPoint(double x, double y)
        {
            longitude = x;
            latitude = y;
        }
    };


    public class GPSUtils
    {
        public const int X = 105629 / 4;
        public const int Y = 28484 / 4;

        public const float MapUnit = 100;//1 meter | 100centimeter
        public const int MipLevel = 14;
        public const int MaxMipLevel = 17;
        public const int MipLevelDelta = 19 - MipLevel;
        public const int MaxMipLevelDelta = 19 - MaxMipLevel;
        public const float BaseTileSize = 128 * MapUnit;
        public const float TileSize = BaseTileSize * (1 << MipLevelDelta);
        public const float MaxTileSize = BaseTileSize * (1 << MaxMipLevelDelta);

        //static List<string> Logs = new List<string>();

        //public static string GetLogs()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    foreach (var item in Logs)
        //    {
        //        sb.AppendLine(item);
        //    }

        //    return sb.ToString();
        //}


       public static Vector3 ComputeLocalPositionGCJ(string longitude, string latitude)
        {
            longitude = "121.453523";
            latitude = "31.245909";

            //Logs.Clear();

            double.TryParse(longitude, out double lng);
            double.TryParse(latitude, out double lat);

            //Logs.Add("1: [ComputeLocalPositionGCJ] lng: " + lng + " / lat:" + lat);

            BMapPoint pt = gcj02_To_Bd09(lng, lat);

           // Logs.Add("2: [ComputeLocalPositionGCJ] pt.longitude: " + pt.longitude + " /  pt.latitude:" + pt.latitude);


            return ComputeLocalPositionImpl(pt.longitude, pt.latitude);
        }

        static Vector3 ComputeLocalPositionImpl(double longitude, double latitude)
        {
            BMapPixel pixel = lngLatToPoint(longitude, latitude);
            double tileX = PixelToTile(pixel.X, MaxMipLevel) - X;
            double tileY = Y - PixelToTile(pixel.Y, MaxMipLevel);

           // Logs.Add("3: [ComputeLocalPositionImpl] tileX: " + tileX + " /  tileY:" + tileY + "  / MaxTileSize :" + MaxTileSize);

            return new Vector3((float)tileX * MaxTileSize, (float)tileY * MaxTileSize, 0);
        }

        static BMapPixel lngLatToPoint(double x, double y)
        {
           // Logs.Add("4: [lngLatToPoint] x: " + x + " /  y:" + y );
            BMapPoint point = zb(new BMapPoint(x, y));

            //Logs.Add("5: [lngLatToPoint] (int)point.longitude: " + (int)point.longitude + " /  (int)point.latitude:" + (int)point.latitude);

            return new BMapPixel((int)point.longitude, (int)point.latitude);
        }

        static double PixelToTile(int pixel, int Level)
        {
            double n = Math.Pow(2, Level - 18);
            return pixel * n / 256;
        }

        /*
        ** 火星坐标系 (GCJ-02) 与百度坐标系 (BD-09) 的转换算法 将 GCJ-02 坐标转换成 BD-09 坐标
        */
        static BMapPoint gcj02_To_Bd09(double gg_lon, double gg_lat)
        {
            double x = gg_lon, y = gg_lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * 3.1415926535897932384626433832795 * 3000.0 / 180.0);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * 3.1415926535897932384626433832795 * 3000.0 / 180.0);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new BMapPoint(bd_lon, bd_lat);
        }



        static double MD(double a, double b, double c)
        {
            for (; a > c;) a -= c - b;
            for (; a < b;) a += c - b;
            return a;
        }

        static double QD(double a, double b, double c)
        {
            if (b != 0)
                a = a > b ? a : b;
            if (c != 0)
                a = a < c ? a : c;
            return a;
        }


        static BMapPoint oK(BMapPoint a, double[] b, int length)
        {
            if (length > 0)
            {
                double c = b[0] + b[1] * (a.longitude < 0 ? -a.longitude : a.longitude);
                double d = (a.latitude < 0 ? -a.latitude : a.latitude) / b[9];
                d = b[2] + b[3] * d + b[4] * d * d + b[5] * d * d * d + b[6] * d * d * d * d + b[7] * d * d * d * d * d + b[8] * d * d * d * d * d * d;
                c = c * (0 > a.longitude ? -1 : 1);
                d = d * (0 > a.latitude ? -1 : 1);
                return new BMapPoint(c, d);
            }
            return new BMapPoint(0, 0);
        }


        static BMapPoint zb(BMapPoint point)
        {
            double[] c = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            point.longitude = MD(point.longitude, -180, 180);
            point.latitude = QD(point.latitude, -74, 74);

            BMapPoint b = new BMapPoint(point.longitude, point.latitude);

            int[] Bu = { 75, 60, 45, 30, 15, 0 };

            double[,] oG = new double[6, 10]
        { { -0.0015702102444, 111320.7020616939, 1704480524535203, -10338987376042340, 26112667856603880, -35149669176653700, 26595700718403920, -10725012454188240, 1800819912950474, 82.5 }
            ,{ 8.277824516172526E-4, 111320.7020463578, 6.477955746671607E8, -4.082003173641316E9, 1.077490566351142E10, -1.517187553151559E10, 1.205306533862167E10, -5.124939663577472E9, 9.133119359512032E8, 67.5 }
            ,{ 0.00337398766765, 111320.7020202162, 4481351.045890365, -2.339375119931662E7, 7.968221547186455E7, -1.159649932797253E8, 9.723671115602145E7, -4.366194633752821E7, 8477230.501135234, 52.5 }
            ,{ 0.00220636496208, 111320.7020209128, 51751.86112841131, 3796837.749470245, 992013.7397791013, -1221952.21711287, 1340652.697009075, -620943.6990984312, 144416.9293806241, 37.5 }
            ,{ -3.441963504368392E-4, 111320.7020576856, 278.2353980772752, 2485758.690035394, 6070.750963243378, 54821.18345352118, 9540.606633304236, -2710.55326746645, 1405.483844121726, 22.5 }
        ,{ -3.218135878613132E-4, 111320.7020701615, 0.00369383431289, 823725.6402795718, 0.46104986909093, 2351.343141331292, 1.58060784298199, 8.77738589078284, 0.37238884252424, 7.45 } };

            bool processed = false;
            for (int d = 0; d < 6; d++)
            {
                if (b.latitude >= Bu[d])
                {
                    for (int m = 0; m < 10; m++)
                        c[m] = oG[d,m];
                    processed = true;
                    break;
                }
            }
            if (!processed)
            {
                for (int d = 0; d < 6; d++)
                    if (b.latitude <= -Bu[d])
                    {
                        for (int m = 0; m < 10; m++)
                            c[m] = oG[d,m];
                        //processed;
                        break;
                    }
            }
            point = oK(point, c, processed ? 10 : 0);
            return point;
        }


    }
}
