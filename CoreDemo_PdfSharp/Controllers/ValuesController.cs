using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo_PdfSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public FileResult Index()
        {
            //获取中文字体，第三个参数表示为是否潜入字体，但只要是编码字体就都会嵌入。
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //读取模板文件
            PdfReader reader = new PdfReader(@"G:\简版征信PDF样本\陈贵年 2019.08.06个人信用报告.pdf");

            //创建文件流用来保存填充模板后的文件
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            PdfStamper stamp = new PdfStamper(reader, stream);
            //设置表单字体，在高版本有用，高版本加入这句话就不会插入字体，低版本无用
            //stamp.AcroFields.AddSubstitutionFont(baseFont);

            AcroFields form = stamp.AcroFields;

            //表单文本框是否锁定
            stamp.FormFlattening = true;

            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("username", "国科");
            para.Add("usertel", "133333333");
            para.Add("workservice", "电脑开不机,可能是电源问题,维修前报价(主机缺侧盖,有重要资料,不能重装)");
            para.Add("maketime", "2017年12月11日 12:24");
            para.Add("recvicename", "某某某");
            para.Add("workername", "某某某");
            para.Add("weixinpic", "");

            //填充表单,para为表单的一个（属性-值）字典
            foreach (KeyValuePair<string, string> parameter in para)
            {
                //要输入中文就要设置域的字体;
                form.SetFieldProperty(parameter.Key, "textfont", baseFont, null);
                //为需要赋值的域设置值;
                form.SetField(parameter.Key, parameter.Value);
            }

            //按顺序关闭io流

            stamp.Close();
            reader.Close();
            //生成文件
            FileResult fileResult = new FileContentResult(stream.ToArray(), "application/pdf");
            //fileResult.FileDownloadName = "4.pdf";
            return fileResult;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
