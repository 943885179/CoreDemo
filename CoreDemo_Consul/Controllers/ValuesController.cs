using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreDemo_Consul.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IConfiguration _configuration;
        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "fff", "fff" };
        }
        [HttpGet("c")]
        public async Task<string> C()
        {
            var url = _configuration["ConsulAddress"].ToString();

            using (var consulClient = new ConsulClient(a => a.Address = new Uri(url)))
            {
                var services = consulClient.Catalog.Service("ServiceA").Result.Response;
                if (services != null && services.Any())
                {
                    // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                    Random r = new Random();
                    int index = r.Next(services.Count());
                    var service = services.ElementAt(index);

                    using (HttpClient client = new HttpClient())
                    {
                        // var response = await client.GetAsync($"http://localhost:{service.ServicePort}/api/values");
                        var response = await client.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/values");
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                return "has error";
            }
        }
        [HttpGet("Test/{Service}/{path}")]
        public async Task<string> Test(string Service= "ServiceA", string path="values")
        {
            var url = _configuration["ConsulAddress"].ToString();

            using (var consulClient = new ConsulClient(a => a.Address = new Uri(url)))
            {
                var services = consulClient.Catalog.Service(Service).Result.Response;
                if (services != null && services.Any())
                {
                    // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                    Random r = new Random();
                    int index = r.Next(services.Count());
                    var service = services.ElementAt(index);

                    using (HttpClient client = new HttpClient())
                    {
                        // var response = await client.GetAsync($"http://localhost:{service.ServicePort}/api/values");
                        var response = await client.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/{path}");
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                return "has error";
            }
        }
        [HttpPost("Tests")]
        public async Task<string> Tests([FromBody]TestDto input)
        {
            var serverUrl = _configuration["ConsulAddress"].ToString();
            using (var consulClient = new ConsulClient(a => a.Address = new Uri(serverUrl)))
            {
                var services = consulClient.Catalog.Service(input.Service).Result.Response;
                if (services != null && services.Any())
                {
                    // 模拟随机一台进行请求，这里只是测试，可以选择合适的负载均衡工具或框架
                    Random r = new Random();
                    int index = r.Next(services.Count());
                    var service = services.ElementAt(index);
                    HttpClientHandler handler = new HttpClientHandler();

                    using (HttpClient client = new HttpClient(handler))
                    {
                        //Data为json,
                        HttpContent content = new StringContent(input.Data)
                        {
                          //  Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                        };
                        
                        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        // var response = await client.GetAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/{input.Path}");
                        var response = await client.PostAsync($"http://{service.ServiceAddress}:{service.ServicePort}/api/{input.Path}", content);
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                }
                return "has error";
            }
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "valuebbb"+id;
        }

        // POST api/values
        [HttpPost]
        public string  Post(string value)
        {
            return "post:"+value;
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id,string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class TestDto
    {
        public string Service { get; set; }
        public string Path { get; set; }
        public string Data { get; set; }
    }
}
