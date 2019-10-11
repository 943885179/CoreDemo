using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Spire.Pdf;
using Spire.Pdf.Annotations;
using Spire.Pdf.Graphics;

namespace CoreDemo_Spire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpGet(@"ReadByFilePath/{filePath=G:\陈贵年 2019.08.06个人信用报告.pdf}")]
        public string ReadByFilePath(string filePath = @"G:\陈贵年 2019.08.06个人信用报告.pdf")
        {
            //创建PdfDocument实例  
            PdfDocument pdf = new PdfDocument();
            //加载PDF文档  
            pdf.LoadFromFile(filePath);
            var sb = new StringBuilder();
            foreach (PdfPageBase page in pdf.Pages)
            {
                string text = page.ExtractText(new RectangleF(0, 0, 1000, 1000));
                var spStr = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                //清除第一和最后一个：无效
                var spStrs = spStr.Skip(1).Take(spStr.Count() - 2).ToList();
                foreach (var sp in spStrs)
                {
                    sb.Append(sp);
                }
                /* 
                 Image[] ss = page.ExtractImages();
                   Bitmap images = new Bitmap(ss[0]);
                   images.Save("G:\\name.png");*/
            }
            var x = sb.ToString();
            return sb.ToString();
        }
        [HttpPost("ReadByFile")]
        public string ReadByFile(IFormFile file)
        {
            if (file == null)
            {
                return "请上传文件";
            }
            var dir = "pdf";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, file.FileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                PdfDocument pdf = new PdfDocument();
                pdf.LoadFromStream(fs);
                var sb = new StringBuilder();
                foreach (PdfPageBase page in pdf.Pages)
                {
                    string text = page.ExtractText(new RectangleF(0, 0, 1000, 1000));
                    var spStr = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    //清除第一和最后一个：无效
                    var spStrs = spStr.Skip(1).Take(spStr.Count() - 2).ToList();
                    foreach (var sp in spStrs)
                    {
                        sb.Append(sp);
                    }
                }
                fs.Flush();
                return sb.ToString();
            }
        }
        [HttpPost("ReadByBytes")]
        public string ReadByBytes(IFormFile file)
        {
            if (file == null)
            {
                return "请上传文件";
            }
            var dir = "pdf";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var filePath = Path.Combine(dir, file.FileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                byte[] pReadByte = new byte[0];
                BinaryReader r = new BinaryReader(fs);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);

                PdfDocument pdf = new PdfDocument();
                pdf.LoadFromBytes(pReadByte);
                var sb = new StringBuilder();
                foreach (PdfPageBase page in pdf.Pages)
                {
                    string text = page.ExtractText(new RectangleF(0, 0, 1000, 1000));
                    var spStr = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    //清除第一和最后一个：无效
                    var spStrs = spStr.Skip(1).Take(spStr.Count() - 2).ToList();
                    foreach (var sp in spStrs)
                    {
                        sb.Append(sp);
                    }
                }
                fs.Flush();
                return sb.ToString();
            }
        }
        [HttpGet(@"ReadImageByPath/{filePath=G:\1.pdf}/{imageNumber=0}")]
        public FileResult ReadImageByPath(string filePath = @"G:\陈贵年 2019.08.06个人信用报告.pdf", int pageIndex = 0, int imageNumber = 0)
        {
            filePath = filePath.Replace("%2F", "\\");
            //创建PdfDocument实例  
            PdfDocument pdf = new PdfDocument();
            //加载PDF文档  
            pdf.LoadFromFile(filePath);
            var sb = new StringBuilder();
            if (pdf.Pages.Count < pageIndex + 1)
            {//页面不存在
                return null;
            }
            Image[] ss = pdf.Pages[pageIndex].ExtractImages();
            if (ss != null && imageNumber < ss.Count())
            {
                var imageFirst = ss[imageNumber];
                Bitmap bmp = new Bitmap(ss[imageNumber]);
                //bmp.Save("G:\\name.png");
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                bmp.Save(ms, ImageFormat.Png);
                bmp.Dispose();

                FileResult fileResult = new FileContentResult(ms.ToArray(), "image/png");
                return fileResult;

                //return File(ms, "image/png");
            }
            return null;
        }
        [HttpPost("ReadImageByFile")]
        public ActionResult ReadImageByFile(IFormFile file, [FromForm]int pageIndex = 0, [FromForm]int imageNumber = 0)
        {
            if (file == null)
            {
                return Ok(new { code = 200, msg = "未上传图片" });
            }
            var dir = "pdf";
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
            }
            Directory.CreateDirectory(dir);
            var filePath = Path.Combine(dir, file.FileName);
            using (FileStream fs = System.IO.File.Create(filePath))
            {
                file.CopyTo(fs);
                fs.Flush();
                PdfDocument pdf = new PdfDocument();
                pdf.LoadFromStream(fs);
                if (pdf.Pages.Count < pageIndex + 1)
                {//页面不存在
                    return Ok(new { code = 200, msg = "页面不存在" });
                }
                Image[] ss = pdf.Pages[pageIndex].ExtractImages();
                if (ss != null && imageNumber < ss.Count())
                {
                    var imageFirst = ss[imageNumber];
                    Bitmap bmp = new Bitmap(ss[imageNumber]);
                    var dirs = "images";
                    if (Directory.Exists(dirs))
                    {
                        Directory.Delete(dirs, true);
                    }
                    Directory.CreateDirectory(dirs);
                    var imageName = "1.png";
                    bmp.Save(dirs + "/" + imageName);
                    bmp.Dispose();
                    //return Redirect(this.Request.Scheme + "://" + this.Request.Host + "/"+dirs+"/"+imageName);
                    //return File(ms, "image/png");
                    return Ok(new { code = 200, msg = "成功", Url = this.Request.Scheme + "://" + this.Request.Host + "/" + dirs + "/" + imageName });
                }
                return Ok(new { code = 200, msg = "找不到图片" });
            }
        }
       [HttpGet("CreatPDF")]
        public void CreatPDF()
        {//
            float x = 50;
            float y = 50;
            string hang1 = "测试文字1";
            string hang2 = "测试文字2";
            string hang3 = "测试文字3";
            PdfDocument pdf = new PdfDocument();//新建一个pdf
            PdfPageBase page = pdf.Pages.Add();//新建pdf的一页
            PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 30f, FontStyle.Regular), true);//创建一个pdf的字体
            page.Canvas.DrawString(hang1, font1, PdfBrushes.Black, new PointF(x, y)); y += 50;
            page.Canvas.DrawString(hang2, font1, PdfBrushes.Black, new PointF(x, y)); y += 50;
            page.Canvas.DrawString(hang3, font1, PdfBrushes.Black, new PointF(x, y)); y += 50;
            // PdfStringFormat format = new PdfStringFormat();
            // format.MeasureTrailingSpaces = true;
            //// x += font1.MeasureString(str, format).Width;

            //超链接
            // PdfTrueTypeFont font2 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 30f, FontStyle.Underline), true);
            // PdfTextWebLink webLink = new PdfTextWebLink();
            // webLink.Url = "https://www.baidu.com";
            // webLink.Text = "百度";
            // webLink.Font = font2;
            // webLink.Brush = PdfBrushes.Blue;
            // webLink.DrawTextWebLink(page.Canvas, new PointF(x, y));
            pdf.SaveToFile("1.pdf");////创建pdf
        }
    }
}