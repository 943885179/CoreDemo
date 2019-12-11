using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace CoreDemo_Core3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet()]
        public object Get()
        {
            var en = new Test(1, "testaaa");//英文:通过
            var wx = new Test(1, "😊");//其他:通过,但是转成字符串是ASCII,返回给前端会识别
            var zh = new Test(1, "微笑可爱至");//中文:通过,但是转成字符串是ASCII,返回给前端会识别
            zh.TestEnum = TestEnum.Secend;//枚举测试:返回枚举的值
            zh.Children = new List<Test>() { en,wx};
            var zf=new Test(1, @",./?.、\\][{};:;/“\"); //字符：通过，同样转成字符串是ASCII，转回来后特殊字符也会修改如“\”改成了“\\” 如果不加@则和中文一样
            var jsonStr = JsonSerializer.Serialize(zh);
            var json = JsonSerializer.Deserialize<Test>(jsonStr);
            var jsStr = "{\"TestEnum\":\"Secend\"}"; //全是数组的字符串不能转为数字，报错，字段加[JsonConverter(typeof(JsonStringEnumConverter))]枚举返回字段对应的名称“Secend”
            var jsons = JsonSerializer.Deserialize<Test>(jsStr);
             var options = new JsonSerializerOptions() {
                 Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
             };
             var zhStr = JsonSerializer.Serialize(zh,typeof(Test),options);// 中文不转码
             return jsonStr;
        }
    }
    public class Test
    {
        public Test()
        {
        }
        public Test(int id, string str)
        {
            Id = id;
            Str = str;
        }
        public int Id { get; set; }
        public string Str { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TestEnum TestEnum { get; set; }
        public List<Test> Children { get; set; }
    }

    public enum TestEnum
    {
        First,
        Secend
    }
}