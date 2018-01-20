using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PCServer.Redis;
using PCServer.Server.Config;
using PCServer.Server.Net;
using PCServer.TaskNodeServer;
using SHSecurityContext.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCServer.Server
{
    public class PCServerEntry
    {
        public PCServerConfig PCServerConfig = new PCServerConfig();
        public RedisConfig RedisConfig = new RedisConfig();

        //WebSocket 协议注册
        public ProtoProcess ProtoProcess = new ProtoProcess();

        public async Task<bool> Init()
        {

            using (var serviceScope = ServiceLocator.Instance.CreateScope())
            {
                //读取PCServerConfig配置
                PCServerConfig = serviceScope.ServiceProvider.GetService<IOptions<PCServerConfig>>().Value;
                //RedisConfig = serviceScope.ServiceProvider.GetService<IOptions<RedisConfig>>().Value;

                //if(PCServerConfig.UseProtobuf)
                //{
                //    ProtoProcess.Register();
                //}

                //获取Context
                var dbContext = serviceScope.ServiceProvider.GetService<PPCServerContext>();

                //自动创建database
                //await new DbInitializer().InitializeAsync(dbContext);
                await dbContext.Database.EnsureCreatedAsync();
                //迁移创建命令: Add-Migration init_data

                //初始化数据库数据
                InitDatabase(ref dbContext);
            }



            TaskManager.inst.TaskDispatcher();

            return true;
        }


        void AddSipPort(ref PPCServerContext context, string id, string ip, string port)
        {
            var q = context.sys_sipports.Count(p => p.Id == id);
            if (q <= 0)
            {
                context.sys_sipports.Add(new SHSecurityModels.sys_sipport()
                {
                    Id = id,
                    pushToIp = ip,
                    pushToPort = port,
                    sipSession = 0,
                     CameraId = ""
                });
            }
        }

        private  void InitDatabase(ref PPCServerContext context)
        {
            #region 测试mysql
            AddSipPort(ref context, "1", "15.160.16.120", "6010");
            AddSipPort(ref context, "2", "15.160.16.120", "6020");
            AddSipPort(ref context, "3", "15.160.16.120", "6030");
            AddSipPort(ref context, "4", "15.160.16.120", "6040");
            AddSipPort(ref context, "5", "15.160.16.120", "6050");

            context.SaveChanges();

     
            /*
            context.db_jjds.Add(new SHSecurityModels.db_jjd()
            {
                af_addr = "keven123"
            });
            context.SaveChanges();
            */
            #endregion

            #region 测试redis
            /* 
            var redisdb = RedisClientSingleton.Inst.GetDatabase();
            redisdb.ListRightPush("keven1", "哈哈");
            var key = "keven1";
            if (redisdb.IsConnected(key))
            {
                long length = redisdb.ListLength(key);
                var ff = redisdb.ListRange(key, 0, length).Select(p => (string)p).ToList();
            }
            */
            #endregion

            //TaskManager.inst.AddTask(new TaskNodeServer.Task.NodeTask()
            //{
            //    AuthorID = "123",
            //    TaskDesc = "测试Task",
            //    TaskName = "测试Task",
            //    AgentRunContent = new TaskNodeServer.Agent.AgentRunContent()
            //    {
            //        AgentType = TaskNodeServer.Agent.AgentTypeEnum.kTest,
            //        RunnerType = TaskNodeServer.Agent.AgentRunnerTypeEnum.kPowershell,
            //        CmdList = new List<string>()
            //           {
            //               "node --version",
            //               "npm --version"
            //           }
            //    }
            //});



            return;
        }











    }
}
