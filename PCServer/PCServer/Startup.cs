#define PUBLISH_GONGAN
//#define AT_COMPANY

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
using SHSecurityServer.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using MKServerWeb.Server;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.HttpOverrides;
using NLog.Extensions.Logging;
using MKServerWeb.Model.RealData;
using PCServer.Server;
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
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
            });

            services.AddNodeServices();

            services.AddScoped<IJJDRepository, JJDRepository>();
            services.AddScoped<ISysWifiTableRepository, SysWifiTableRepository>();
            services.AddScoped<ISys110WarningRepository, Sys110WarningRepository>();
            services.AddScoped<ISysTicketresRepository, SysTicketresRepository>();
            services.AddScoped<ISysConfigRepository, SysConfigRepository>();
            services.AddScoped<IPoliceGpsRepository, PoliceGpsRepository>();
            services.AddScoped<IGpsGridRepository, GpsGridRepository>();
            services.AddScoped<ICamerasRepository, CamerasRepository>();
            services.AddScoped<ICamePeopleCountRepository, CamePeopleCountRepository>();
            services.AddScoped<IPoliceGPSAreaStaticRepository, PoliceGPSAreaStaticRepository>();
            services.AddScoped<IWifiDataPeoplesRepository, WifiDataPeoplesRepository>();
            services.AddScoped<IWifiDataPeoplesHistoryRepository, WifiDataPeoplesHistoryRepository>();
            services.AddScoped<IKaKouDataJinRepository, KaKouDataJinRepository>();
            services.AddScoped<IKaKouDataJinHistoryRepository, KaKouDataJinHistoryRepository>();
            services.AddScoped<ITravioDataRepositoy, TravioDataRepositoy>();
            services.AddScoped<IKaKouTopRepository, KaKouTopRepository>();
            services.AddScoped<IRoadDataRecordRepository, RoadDataRecordRepository>();
            services.AddScoped<IMQServerDataRepository, MQServerDataRepository>();
            services.AddScoped<IHongWaiPeopleDataRepositoy, HongWaiPeopleDataRepositoy>();
            services.AddScoped<IFaceAlarmDataRepositoy, FaceAlarmDataRepositoy>();
            services.AddScoped<IPeopleCountConfigRepositoy, PeopleCountConfigRepositoy>();

            var conn = Configuration.GetConnectionString("DefaultConnection");

            services.AddEntityFrameworkMySql()
                    .AddOptions()
                    .AddDbContext<SHSecuritySysContext>(opt => { opt.UseMySql(conn); });

            //services.Configure<IISOptions>(options =>
            //{
            //});

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Info { Title = "SHSecuritySys API", Version = "v1" });
            //});

            //> 安装Swagger中间件
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SHSecuritySys API",
                    Version = "v1",
                    Description = "静安公安服务器WebAPI",
                    TermsOfService = "None",
                    License = new License { Name = "License", Url = "" }
                });

                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                    "API.XML"));
                c.DescribeAllEnumsAsStrings();
            });

            services.Configure<RealDataUrl>(this.Configuration.GetSection("RealDataUrl"));

            //> 配置跨域
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider, INodeServices nodeServices, ISysTicketresRepository systicketRepo
            , ISysConfigRepository configRepo, ISys110WarningRepository sys110WarningRepo)

        {
            loggerFactory.AddConsole();
            loggerFactory.AddNLog();

            //app.UseExceptionHandler("/Home/Error");

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SHSecuritySys API V1");
            });


            // Set up custom content types -associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            //provider.Mappings[".myapp"] = "application/x-msdownload";
            //provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";

            // Replace an existing mapping
            //provider.Mappings[".rtf"] = "application/x-msdownload";
            // Remove MP4 videos.
            //provider.Mappings.Remove(".mp4");

            provider.Mappings[".mem"] = "application/octet-stream";
            provider.Mappings[".data"] = "application/octet-stream";
            provider.Mappings[".memgz"] = "application/octet-stream";
            provider.Mappings[".datagz"] = "application/octet-stream";
            provider.Mappings[".jsgz"] = "application/x-javascript; charset=UTF-8";

            provider.Mappings[".mp4"] = "video/mp4";

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
            //app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider
            });
            //app.UseStaticFiles(new StaticFileOptions()
            //t
            //    FileProvider = new PhysicalFileProvider(
            //    Path.Combine(Directory.GetCurrentDirectory(), @"static", "sceneModules")),
            //    RequestPath = new PathString("/module")
            //});


            //app.UseCors(builder =>
            //    builder.WithOrigins("http://10.1.30.207:8000")
            //           .AllowAnyHeader());

            app.UseCors("CorsPolicy");


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });




            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();





            ServiceLocator.Instance = app.ApplicationServices;


            bool isPublishGongAn = false;

#if PUBLISH_GONGAN
            isPublishGongAn = true;
#endif
            //内部服务初始化
            PCServerMain.Instance = new PCServerEntry();
            PCServerMain.Instance.Init(isPublishGongAn);

//#if PUBLISH_GONGAN
//            //如果是在公安内部系统中运行
//            if (isPublishGongAn)
//            {

//            }
//#endif

#if AT_COMPANY
            // 用于在公司测试生成数据
            //InternalTestServer.Add110WarnningData(sys110WarningRepo);
            //InternalTestServer.AddTicketresData(systicketRepo);
            //InternalTestServer.ClearData();
#endif

            #region
            //var dbContext = serviceProvider.GetService<SHSecuritySysContext>();
            //dbContext.Database.EnsureCreated();

            //SampleData.InitDB(ref dbContext);
            //MyTest.TestOracle(nodeServices);
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
