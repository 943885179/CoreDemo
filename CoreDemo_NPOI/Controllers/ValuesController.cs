using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CoreDemo_NPOI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            // NPOIUtil.Write();
            // var e = test();
            //var str = "";
            //try
            //{

            //    throw new Exception("ZHESHICUOWU");
            //}
            //catch (Exception ex)
            //{

            //    str = ex.Message + ":::" + ex.InnerException + ":::" + ex.Data + ":::" + ex.HelpLink + ":::";
            //}
            string x = "mick";
            string t = string.Copy(x);
            if (x==t)
            {
                return new string[] { "true", "value2" };
            }
            return new string[] { "value1", "value2" };
        }
        [HttpPost("upload")]
        public ActionResult<object> Upload(IFormFileCollection files)
        {
            var fileDir = "excel";
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }
            foreach (var file in files)
            {
                /*
                var fileName = Path.GetExtension(file.FileName);
                var filePath = Path.Combine(fileDir,"sss" +fileName);
                using (FileStream fss = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fss);
                    fss.Flush();
                }
                var fs= new FileStream(filePath, FileMode.Open, FileAccess.Read);
                IWorkbook wb = new XSSFWorkbook(fs);
                var sheet=wb.GetSheetAt(0);
                var row = sheet.GetRow(0);
                var col = row.GetCell(0);*/
                 IWorkbook wb = new XSSFWorkbook(file.OpenReadStream());
                    var sheet = wb.GetSheetAt(0);
                    var row = sheet.GetRow(0);
                    var col = row.GetCell(0);
                    return col.ToString();
            }
            return null;
        }
        public IEnumerable<int> test()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i;
            }
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
