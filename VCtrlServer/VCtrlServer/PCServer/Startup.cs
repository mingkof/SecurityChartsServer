using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SHSecurityContext.DBContext;
using SHSecurityContext.IRepositorys;
using SHSecurityContext.Repositorys;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.HttpOverrides;
using NLog.Extensions.Logging;
using Microsoft.AspNetCore.WebSockets;
using PCServer.Server.Net;
using System.Net.WebSockets;
using System.Threading;
using PCServer.Server;
using PCServer.Server.Config;
using PCServer.Redis;
using Microsoft.Extensions.PlatformAbstractions;

namespace PCServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //> 设置api json传输Json格式不变
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            //> 设置开启Node服务
            services.AddNodeServices();

            //> 依赖注入业务层
            //services.AddScoped<IJJDRepository, JJDRepository>();
            //services.AddScoped<ISysWifiTableRepository, SysWifiTableRepository>();
            //services.AddScoped<ISys110WarningRepository, Sys110WarningRepository>();
            //services.AddScoped<ISysTicketresRepository, SysTicketresRepository>();
            services.AddScoped<ISysConfigRepository, SysConfigRepository>();

            //TBS 依赖注入业务层

            services.AddScoped<ISipPortRepository, SipPortRepository>();

            //> 设置数据库连接  Mysql Entity
            var conn = Configuration.GetConnectionString("DefaultConnection");
            services.AddEntityFrameworkMySql()
                    .AddOptions()
                    .AddDbContext<PPCServerContext>(opt => { opt.UseMySql(conn); });

            //> 服务器配置
            services.Configure<PCServerConfig>(this.Configuration.GetSection("PCServerConfig"));
            services.Configure<RedisConfig>(this.Configuration.GetSection("RedisConfig"));

            //> 安装Swagger中间件
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "TBS Server API",
                    Version = "v1",
                    Description = "服务器WebAPI",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "汪子文", Email = "417805862@qq.com", Url = "" },
                    License = new License { Name = "License", Url = "" }
                });

                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    "TBSServer.XML")); 
                c.DescribeAllEnumsAsStrings();
            });


            //> 配置跨域
            services.AddCors();


            
            /* 备用
            services.AddWebSocketManager();
            services.Configure<IISOptions>(options =>
            {
            });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider, INodeServices nodeServices)

        {
            //使用log和NLog
            loggerFactory.AddConsole();
            loggerFactory.AddNLog();
       
            //使用异常跳转
            app.UseExceptionHandler("/Home/Error");

            //使用Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TBS API V1");
            });


            #region WebSocket
            //启用WebSocket
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/websockets

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024,
            };
            app.UseWebSockets(webSocketOptions);

            //app.Map("/ws", SocketHandler.Map);
            //自定义WebSocket中间件  Protobuf时使用
            app.UseMiddleware<ChatWebSocketMiddleware>();  


            #endregion


            #region 配置MIME和使用staticFile
            // Set up custom content types -associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            //provider.Mappings[".myapp"] = "application/x-msdownload";
            //provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            provider.Mappings[".mem"] = "application/octet-stream";
            provider.Mappings[".data"] = "application/octet-stream";
            provider.Mappings[".memgz"] = "application/octet-stream";
            provider.Mappings[".datagz"] = "application/octet-stream";
            provider.Mappings[".jsgz"] = "application/x-javascript; charset=UTF-8";
            provider.Mappings[".mp4"] = "video/mp4";
            //provider.Mappings[".rtf"] = "application/x-msdownload";
            //Remove MP4 videos.   // Replace an existing mapping
            //provider.Mappings.Remove(".mp4");

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider
            });
            #endregion


            /*配置跨域时打开并设置*/
            /*
            app.UseCors(builder =>
                builder.WithOrigins("http://10.1.30.207:8000")
                       .AllowAnyHeader());
            */

            //使用路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                /*当启用spa时使用spa中路由打开*/
                /*如果只是打开普通的wwwroot下的index页面,不需要使用node中的路由配置则关闭这里*/
                
                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                
            });

            //配置nginx等反向代理设置需要  参考： https://www.cnblogs.com/haoliansheng/p/6595568.html
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //配置身份验证 参考： https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?tabs=visual-studio%2Caspnetcore2x
            app.UseAuthentication();
    

            //存取全局AppServer
            ServiceLocator.Instance = app.ApplicationServices;
            
            //内部服务初始化
            PCServerMain.Instance = new PCServerEntry();
            await PCServerMain.Instance.Init();


            #region 记录
            //var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;

            /*
             app.UseWebSockets();
             app.MapWebSocketManager("/chat", serviceProvider.GetService<ChatHandler>());
             app.UseStaticFiles(new StaticFileOptions() {
                 FileProvider = new PhysicalFileProvider(
                 Path.Combine(Directory.GetCurrentDirectory(), @"static", "sceneModules")),
                 RequestPath = new PathString("/module")
             });
            */
            /*
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images")),
                RequestPath = new PathString("/MyImages"),
                ContentTypeProvider = provider
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images")),
                RequestPath = new PathString("/MyImages")
            });
            */


            //MyTest.TestOracle(nodeServices);
            // NodeServer.SyncPoliceData(nodeServices, configRepo);
            //NodeServer.InitTicketResultData(nodeServices, systicketRepo, configRepo);
            // InternalTestServer.Add110WarnningData(sys110WarningRepo);
            // InternalTestServer.AddTicketresData(systicketRepo);
            // InternalTestServer.ClearData();

            #endregion
        }

    }



    public static class ServiceLocator
    {
        public static System.IServiceProvider Instance { get; set; }
    }

    public static class PCServerMain
    {
        public static PCServerEntry Instance { get; set; }
    }


}
