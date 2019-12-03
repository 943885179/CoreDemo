using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using org.apache.pdfbox.io;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace CoreDemo_PDFBOX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public PdfController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet("Test")]
        public String Test()
        {
            return "test";
        }
        [HttpPost("Read")]
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string Read(IFormFile file)
        {
             /*var ss = Directory.GetCurrentDirectory();//获取项目路径
             var st = _hostingEnvironment.ContentRootPath;//获取项目路径
             var fileDir = ss+"\\pdf";
             if (Directory.Exists(fileDir))
             {
                 Directory.Delete(fileDir,true);
             }
             Directory.CreateDirectory(fileDir);
             //string fileName = file.FileName;
             //string filePath= fileDir + $@"\{fileName}";
             var fileName = DateTime.Now.ToString("yyyyMMddHHmmss") +
                          Path.GetExtension(file.FileName);
             var filePath = Path.Combine(fileDir, fileName);
             using (FileStream fs = System.IO.File.Create(filePath))
             {
                 file.CopyTo(fs);
                 fs.Flush();
             }
             var files = new FileInfo(filePath);*/
            //  string currentDirectory = Path.GetDirectoryName((new PdfController()).GetType().Assembly.Location);

            PDDocument doc = PDDocument.load(@"G:/Read.pdf");

            PDFTextStripper pdfStripper = new PDFTextStripper();
            string text = pdfStripper.getText(doc);
            return text;
        }
    }
}