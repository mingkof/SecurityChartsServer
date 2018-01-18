using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MQClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void InitConsumer()
        {
            //创建连接工厂
            IConnectionFactory factory = new ConnectionFactory("tcp://115.159.28.159:61616");
            //通过工厂构建连接
            IConnection connection = factory.CreateConnection("baiyulan", "baiyulan123");
            //这个是连接的客户端名称标识
            connection.ClientId = "MokaiTest1";
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

            //connection.Stop();
            //connection.Close();  

        }

        void consumer_Listener1(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            ShowLable(tbReceiveMessage1, msg);
        }
        void consumer_Listener2(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            ShowLable(tbReceiveMessage2, msg);
        }
        void consumer_Listener3(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            ShowLable(tbReceiveMessage3, msg);
        }
        void consumer_Listener4(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            ShowLable(tbReceiveMessage4, msg);
        }
        void consumer_Listener5(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            //tbReceiveMessage1.Invoke(new DelegateRevMessage(RevMessage), msg);
            ShowLable(tbReceiveMessage5, msg);
        }

        public delegate void DelegateRevMessage(Label label, ITextMessage message);

        public void RevMessage(Label label,ITextMessage message)
        {
            label.Text += string.Format(@"接收到:{0}{1}", message.Text, Environment.NewLine);
        }


        void ShowLable(Label label, ITextMessage msg)
        {
            label.Invoke(new DelegateRevMessage(RevMessage), label, msg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitConsumer();

        }
    }
}
