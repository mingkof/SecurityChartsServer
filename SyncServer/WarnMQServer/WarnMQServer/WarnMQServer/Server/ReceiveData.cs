using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.Threading;
using KVDDDCore.Utils;
using System.Text;

namespace WarnMQServer
{
    public class ReceiveData
    {
        public static Queue<string> queue1 = new Queue<string>();
        public static Queue<string> queue2 = new Queue<string>();
        public static Queue<string> queue3 = new Queue<string>();
        public static Queue<string> queue4 = new Queue<string>();
        public static Queue<string> queue5 = new Queue<string>();

        public ReceiveData()
        {
            Console.WriteLine("已启用读取ActiveMQ数据服务 !");

            Console.WriteLine(TimeUtils.ConvertToTimeStamps("2018-01-24 13:05:00"));
            InitConsumer();
            ProcessData();

            Console.ReadLine();
        }

        public static void InitConsumer()
        {
            //创建连接工厂
            IConnectionFactory factory = new ConnectionFactory("tcp://115.159.28.159:61616");
            //通过工厂构建连接
            IConnection connection = factory.CreateConnection("baiyulan", "baiyulan123");
            //这个是连接的客户端名称标识
            connection.ClientId = "MokaiTest2";
            //启动连接，监听的话要主动启动连接
            connection.Start();
            //通过连接创建一个会话
            ISession session = connection.CreateSession();
            //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
            //IMessageConsumer consumer = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQQueue("firstQueue"), "filter='demo'");
            IMessageConsumer consumer_fire_report = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("fire.report.topic"));
            IMessageConsumer consumer_fire_clear = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("fire.clear.topic"));
            IMessageConsumer consumer_fault_report = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("fault.report.topic"));
            IMessageConsumer consumer_fault_clear = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("fault.clear.topic"));
            IMessageConsumer consumer_unusual_report = session.CreateConsumer(new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("unusual.report.topic"));
            //注册监听事件
            consumer_fire_report.Listener += new MessageListener(consumer_Listener1);
            consumer_fire_clear.Listener += new MessageListener(consumer_Listener2);
            consumer_fault_report.Listener += new MessageListener(consumer_Listener3);
            consumer_fault_clear.Listener += new MessageListener(consumer_Listener4);
            consumer_unusual_report.Listener += new MessageListener(consumer_Listener5);
            Console.WriteLine("启动服务");
        }

        static void consumer_Listener1(IMessage message)
        {
            string msg = ((ITextMessage)message).Text.ToString();
            msg = ConvertJsonData(msg, "1");
            queue1.Enqueue(msg);

            Console.WriteLine("接受到消息:" + msg);
        }
        static void consumer_Listener2(IMessage message)
        {
            string msg = ((ITextMessage)message).Text.ToString();
            msg = ConvertJsonData(msg, "2");
            queue2.Enqueue(msg);

            Console.WriteLine("接受到消息:" + msg);
        }
        static void consumer_Listener3(IMessage message)
        {
            string msg = ((ITextMessage)message).Text.ToString();
            msg = ConvertJsonData(msg, "3");
            queue3.Enqueue(msg);

            Console.WriteLine("接受到消息:" + msg);
        }
        static void consumer_Listener4(IMessage message)
        {
            string msg = ((ITextMessage)message).Text.ToString();
            msg = ConvertJsonData(msg, "4");
            queue4.Enqueue(msg);

            Console.WriteLine("接受到消息:" + msg);
        }
        static void consumer_Listener5(IMessage message)
        {
            string msg = ((ITextMessage)message).Text.ToString();
            msg = ConvertJsonData(msg, "5");
            queue5.Enqueue(msg);

            Console.WriteLine("接受到消息:" + msg);
        }

        static void ProcessData()
        {
            ThreadPool.QueueUserWorkItem((a) => 
            {
                var pathBase = System.IO.Path.Combine(Environment.CurrentDirectory, "MQData/");
                if (!System.IO.Directory.Exists(pathBase))
                    System.IO.Directory.CreateDirectory(pathBase);

                while (true)
                {
                    var YEAR = System.DateTime.Now.Year.ToString();
                    var MONTH = System.DateTime.Now.Month.ToString("00");
                    var DAY = System.DateTime.Now.Day.ToString("00");
                    var HH = System.DateTime.Now.Hour.ToString("00");
                    string fileName = YEAR + MONTH + DAY + HH;


                    string path = pathBase + fileName + ".txt";

                    //string path = @"E:\MKProjects\MKSecurityCharts\SecurityChartsServer\SyncServer\WarnMQServer\MQData\" + fileName + ".txt";

                    List<string> stringList = FileUtils.ReadFileToList(path);
                    if (stringList == null)
                        stringList = new List<string>();
                    for (int i = 0; i < queue1.Count; i++)
                    {
                        string oriDataStr = queue1.Dequeue();
                        stringList.Add(oriDataStr);
                    }
                    for (int i = 0; i < queue2.Count; i++)
                    {
                        string oriDataStr = queue2.Dequeue();
                        stringList.Add(oriDataStr);
                    }
                    for (int i = 0; i < queue3.Count; i++)
                    {
                        string oriDataStr = queue3.Dequeue();
                        stringList.Add(oriDataStr);
                    }
                    for (int i = 0; i < queue4.Count; i++)
                    {
                        string oriDataStr = queue4.Dequeue();
                        stringList.Add(oriDataStr);
                    }
                    for (int i = 0; i < queue5.Count; i++)
                    {
                        string oriDataStr = queue5.Dequeue();
                        stringList.Add(oriDataStr);
                    }
                    Console.WriteLine(DateTime.Now.ToString());
                    Console.WriteLine("检测新消息写入文件");
                    FileUtils.WriteFile(path, stringList, true, Encoding.UTF8);
                    Thread.Sleep(1000*5);
                }
            });
        }

        /// <summary>
        /// 添加topicType timeStamp
        /// </summary>
        /// <param name="oriDataStr"></param>
        /// <param name="topicType"></param>
        /// <param name="stringList"></param>
        static string ConvertJsonData(string oriDataStr,string topicType)
        {
            JsonMQServerDataContruct res = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonMQServerDataContruct>(oriDataStr);
            res.topicType = topicType;

            res.timeStamp = TimeUtils.ConvertToTimeStamps(res.time);
            string processedData = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            return processedData;
        }
    }
}
