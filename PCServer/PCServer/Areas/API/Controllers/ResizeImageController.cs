using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.NodeServices;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace MKServerWeb.Controllers
{
    public class ResizeImageController : Controller
    {

        //[Route("resize/{*imagePath}")]
        //public async Task<IActionResult> ResizeImage(
        //   [FromServices] IHostingEnvironment hostingEnvironment,
        //   [FromServices] INodeServices nodeServices,
        //   string imagePath,
        //   int width = 300 )
        //{
        //   //Find the image on disk
        //   var fileInfo = hostingEnvironment
        //       .WebRootFileProvider.GetFileInfo(imagePath);

        //   if(!fileInfo.Exists)
        //   {
        //       return NotFound();
        //   }

        //   var result = await nodeServices.InvokeAsync<Stream>(
        //       "Node/resizeImage.js",
        //       fileInfo.PhysicalPath,
        //       width
        //       );

        //   //return Content($"You want to resize {fileInfo.PhysicalPath}");
        //   return File(result, "image/jpeg");
        //}

        //例子:  简单测试npm的 nodeservice
        //https://dotnetthoughts.net/using-node-services-in-aspnet-core/
        //[Route("rtest")]
        [HttpGet("rtest")]
        public async Task<IActionResult> RTest([FromServices] INodeServices nodeServices)
        {

            var result = await nodeServices.InvokeAsync<string>(
                "Node/test/rtest.js"
                );

            return Content(result);
        }

        [HttpGet]
        [Produces("video/mp4")]
        public HttpResponseMessage Get()
        {
            string videoPath = @"D:\sample.mp4";
            if (System.IO.File.Exists(videoPath))
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(new FileStream(videoPath, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "sample.mp4"
                };
                return response;
            }
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }

        //[HttpGet("oracletest")]
        //public async Task<IActionResult> OracleTest([FromServices] INodeServices nodeServices)
        //{

        //    var result = await nodeServices.InvokeAsync<string>(
        //        "Node/test/oracleTest.js"
        //        );

        //    return Content(result);
        //}


        //[HttpGet("mysqltest")]
        //public async Task<IActionResult> MysqlTest([FromServices] INodeServices nodeServices)
        //{

        //    var result = await nodeServices.InvokeAsync<string>(
        //        "Node/test/mysqlTest.js"
        //        );

        //    return Content(result);
        //}

        [HttpGet("plrun")]
        public async Task<IActionResult> PlRun([FromServices] INodeServices nodeServices)
        {

            var result = await nodeServices.InvokeAsync<string>(
                "Node/gongan/SyncPoliceData_1.js"
                );

            return Content(result);
        }


        /*
         * 
        //例子:  npm包 url-to-image 可以生成网页截图
        [Route("rurl")]
        //[HttpPost]
        public async Task<IActionResult> GenerateUrlPreview([FromServices] INodeServices nodeServices)
        {
            //var url = Request.Form["Url"].ToString();
            var url = "http://www.baidu.com";
            var fileName = System.IO.Path.ChangeExtension(DateTime.UtcNow.Ticks.ToString(), "png");
            var file = await nodeServices.InvokeAsync<string>("Node/UrlPreviewModule.js", url, 
                System.IO.Path.Combine("PreviewImages", fileName));

            return Content($"<a class=\"btn btn-default\" target=\"_blank\" href=\"/Home/Download?img={fileName}\">Download image</a>");
        }
        */

    }
}