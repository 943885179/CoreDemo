using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.exceptions;
using iTextSharp.text.pdf;
using System.Text;

namespace CoreDemo_iTextSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpGet("Test")]
        public void Test()
        {
            Console.WriteLine("读取PDF文档");
            try
            {
                // 创建一个PdfReader对象
                PdfReader reader = new PdfReader(@"G:\1.pdf");
                // 获得文档页数
                int n = reader.NumberOfPages;
                // 获得第一页的大小
                Rectangle psize = reader.GetPageSize(1);
                float width = psize.Width;
                float height = psize.Height;
                // 创建一个文档变量
                Document document = new Document(psize, 50, 50, 50, 50);
                // 创建该文档
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"G:\Read.pdf", FileMode.Create));
                // 打开文档
                document.Open();
                // 添加内容
                PdfContentByte cb = writer.DirectContent;
                int i = 0;
                int p = 0;
                Console.WriteLine("一共有 " + n + " 页.");
                while (i < n)
                {
                    document.NewPage();
                    p++;
                    i++;
                    PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page1, .5f, 0, 0, .5f, 0, height / 2);
                    Console.WriteLine("处理第 " + i + " 页");
                    if (i < n)
                    {
                        i++;
                        PdfImportedPage page2 = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page2, .5f, 0, 0, .5f, width / 2, height / 2);
                        Console.WriteLine("处理第 " + i + " 页");
                    }
                    if (i < n)
                    {
                        i++;
                        PdfImportedPage page3 = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page3, .5f, 0, 0, .5f, 0, 0);
                        Console.WriteLine("处理第 " + i + " 页");
                    }
                    if (i < n)
                    {
                        i++;
                        PdfImportedPage page4 = writer.GetImportedPage(reader, i);
                        cb.AddTemplate(page4, .5f, 0, 0, .5f, width / 2, 0);
                        Console.WriteLine("处理第 " + i + " 页");
                    }
                    /*cb.SetColorStroke(BaseColor.Blue);
                    cb.MoveTo(0, height / 2);
                    cb.LineTo(width, height / 2);
                    cb.Stroke();
                    cb.MoveTo(width / 2, height);
                    cb.LineTo(width / 2, 0);
                    cb.Stroke();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 14);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "page " + p + " of " + ((n / 4) + (n % 4 > 0 ? 1 : 0)), width / 2, 40, 0);
                    cb.EndText();*/
                }
                // 关闭文档
                document.Close();
            }
            catch (Exception de)
            {
                Console.Error.WriteLine(de.Message);
                Console.Error.WriteLine(de.StackTrace);
            }
        }
        [HttpGet("ReadPdf")]
        public FileResult ReadPdf()
        {
            //获取中文字体，第三个参数表示为是否潜入字体，但只要是编码字体就都会嵌入。
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //读取模板文件
            //PdfReader reader = new PdfReader(@"G:\简版征信PDF样本\陈贵年 2019.08.06个人信用报告.pdf");
            PdfReader reader = new PdfReader(@"G:\11.pdf");
            //创建文件流用来保存填充模板后的文件
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            PdfStamper stamp = new PdfStamper(reader, stream);
            //设置表单字体，在高版本有用，高版本加入这句话就不会插入字体，低版本无用
            //stamp.AcroFields.AddSubstitutionFont(baseFont);

            AcroFields form = stamp.AcroFields;
            var blankPages = 0;
            var streamBytes = reader.GetPageContent(1);
            var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(streamBytes));

            var stringsList = new List<string>();
            for (var pageNum = 1; pageNum <= reader.NumberOfPages; pageNum++)
            {
                // first check, examine the resource dictionary for /Font or /XObject keys.
                // If either are present -> not blank.
                var pageDict = reader.GetPageN(pageNum);
                var resDict = (PdfDictionary)pageDict.Get(PdfName.Resources);

                var hasFont = resDict.Get(PdfName.Font) != null;
                if (hasFont)
                {
                    var fonts = resDict.GetAsString(PdfName.Font);
                    Console.WriteLine($"Page {pageNum} has font(s).");
                    continue;
                }

                var hasImage = resDict.Get(PdfName.Xobject) != null;
                if (hasImage)
                {
                    Console.WriteLine($"Page {pageNum} has image(s).");
                    continue;
                }

                var content = reader.GetPageContent(pageNum);
                if (content.Length <= 20)
                {
                    Console.WriteLine($"Page {pageNum} is blank");
                    blankPages++;
                }
            }
            //表单文本框是否锁定
            stamp.FormFlattening = true;
            var sb = new StringBuilder();
            var cont = string.Empty;
            for (int i = 0; i < reader.NumberOfPages; i++)
            {
                var s = reader.GetPageContent(i);
                //取得每一页的字节数组,将每一个字节转换为字符,并将数组转换为字符串  
                if (s != null)
                {
                    cont += Encoding.UTF8.GetString(s);
                    for (int j = 0; j < s.Length; j++)
                    {
                        sb.Append(Convert.ToChar(s[j]));
                    }
                }
            }
            var tt = sb.ToString();
            var sbb = new StringBuilder();
            var sr = stream.ToArray();
            for (int j = 0; j < sr.Length; j++)
            {
                sbb.Append(Convert.ToChar(sr[j]));
            }
            var ss = sbb.ToString();
            //按顺序关闭io流


            reader.Close();

            var x = Encoding.BigEndianUnicode.GetString(stream.GetBuffer());
            var x1 = Encoding.Unicode.GetString(stream.GetBuffer());
            var x2 = Encoding.ASCII.GetString(stream.GetBuffer());
            var x3 = Encoding.Default.GetString(stream.GetBuffer());
            //生成文件
            FileResult fileResult = new FileContentResult(stream.ToArray(), "application/pdf");
            var t = reader.GetType();
            //fileResult.FileDownloadName = "4.pdf";
            return fileResult;
        }
        [HttpGet("GetFile")]
        public void GetFile()
        {
            string tempFilePath = $"{Guid.NewGuid()}.pdf";
            string[] title = { "货品编号",
                "货品名称",
                "条码",
                "规格",
                "基本单位",
                "当前库存",
                "库存下限",
                "库存上限"
            };
            var s = new PdfGState();
            using (FileStream wfs = new FileStream(tempFilePath, FileMode.OpenOrCreate))
            {
                //PageSize.A4.Rotate();当需要把PDF纸张设置为横向时
                Document docPDF = new Document(PageSize.A4, 10, 10, 20, 20);
                PdfWriter write = PdfWriter.GetInstance(docPDF, wfs);
                docPDF.Open();
                //在这里需要注意的是，itextsharp不支持中文字符，想要显示中文字符的话需要自己设置字体 
                BaseFont bsFont = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(bsFont);

                float[] clos = new float[] { 40, 40, 40, 20, 20, 30, 30, 30 };// 宽度
                PdfPTable tablerow1 = new PdfPTable(clos);
                foreach (string t in title)
                {
                    PdfPCell cell = new PdfPCell(new Paragraph(t, font));
                    cell.MinimumHeight = 4f;
                    tablerow1.AddCell(cell);
                }
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                tablerow1.AddCell(new PdfPCell(new Paragraph("213", font)));
                docPDF.Add(tablerow1);//将表格添加到pdf文档中
                docPDF.Close();//关闭
                write.Close();
                wfs.Close();
            }
        }

[HttpGet("d")]
        public void Detect_Blank_Pages_In_Pdf()
        {
            // value where we can consider that this is a blank image
            // can be much higher or lower depending of what is considered as a blank page
            const int blankThreshold = 20;

            var pdfFile = createSamplePdfFile();
            var reader = new PdfReader(pdfFile);

            var blankPages = 0;
            for (var pageNum = 1; pageNum <= reader.NumberOfPages; pageNum++)
            {
                // first check, examine the resource dictionary for /Font or /XObject keys.
                // If either are present -> not blank.
                var pageDict = reader.GetPageN(pageNum);
                var resDict = (PdfDictionary)pageDict.Get(PdfName.Resources);

                var hasFont = resDict.Get(PdfName.Font) != null;
                if (hasFont)
                {
                    var s = resDict.Get(PdfName.Font);
                    Console.WriteLine($"Page {pageNum} has font(s).");
                    continue;
                }

                var hasImage = resDict.Get(PdfName.Xobject) != null;
                if (hasImage)
                {
                    var s = resDict.Get(PdfName.Xobject);
                    Console.WriteLine($"Page {pageNum} has image(s).");
                    continue;
                }

                var content = reader.GetPageContent(pageNum);
                if (content.Length <= blankThreshold)
                {//空白页
                    Console.WriteLine($"Page {pageNum} is blank");
                    blankPages++;
                }
            }

            reader.Close();

           // Assert.AreEqual(expected: 1, actual: blankPages, message: $"{reader.NumberOfPages} page(s) with {blankPages} blank page(s).");
        }

        [HttpGet("a")]
        public void Test_Extract_Text()
        {
            var pdfFile = createSamplePdfFile();
            var reader = new PdfReader(pdfFile);

            var streamBytes = reader.GetPageContent(1);
            var tokenizer = new PrTokeniser(new RandomAccessFileOrArray(streamBytes));

            var stringsList = new List<string>();
            while (tokenizer.NextToken())
            {
                if (tokenizer.TokenType == PrTokeniser.TK_STRING)
                {
                    stringsList.Add(tokenizer.StringValue);
                }
            }

            reader.Close();

            // Assert.IsTrue(stringsList.Contains("Hello DNT!"));
        }
        private static byte[] createSamplePdfFile()
        {
            using (var stream = new MemoryStream())
            {
                var document = new Document();

                // step 2
                var writer = PdfWriter.GetInstance(document, stream);
                // step 3
                document.AddAuthor("123");
                document.Open();
                // step 4
                document.Add(new Paragraph("Hello DNT!"));

                document.NewPage();
                // we don't add anything to this page: newPage() will be ignored
                document.Add(new Phrase(""));

                document.NewPage();
                writer.PageEmpty = false;

                document.Close();
                return stream.ToArray();
            }
        }
        [HttpGet("c")]
        public void Test_Draw_Text()
        {
            var fileStream = new FileStream(@"G:\11.pdf", FileMode.Create);
            var pdfDoc = new Document(PageSize.A4);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, fileStream);

            pdfDoc.AddAuthor("mzj");
            pdfDoc.Open();

            pdfDoc.Add(new Paragraph("Test"));

            PdfContentByte cb = pdfWriter.DirectContent;
            BaseFont bf = BaseFont.CreateFont();
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.MoveText(88.66f, 367);
            cb.ShowText("ld");
            cb.MoveText(-22f, 0);
            cb.ShowText("Wor");
            cb.MoveText(-15.33f, 0);
            cb.ShowText("llo");
            cb.MoveText(-15.33f, 0);
            cb.ShowText("He");
            cb.EndText();

            PdfTemplate tmp = cb.CreateTemplate(250, 25);
            tmp.BeginText();
            tmp.SetFontAndSize(bf, 12);
            tmp.MoveText(0, 7);
            tmp.ShowText("Hello People");
            tmp.EndText();
            cb.AddTemplate(tmp, 36, 343);

            pdfDoc.Close();
            fileStream.Dispose();

            //TestUtils.VerifyPdfFileIsReadable(pdfFilePath);
        }
    }
}