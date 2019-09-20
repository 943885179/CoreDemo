using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public async Task<object> GetAsync()
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                 return disco.Error;
            }
            return disco;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<object> GetAsync(int id)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                 return disco.Error;
            }
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
               return tokenResponse.Error;
            }

            Console.WriteLine(tokenResponse.Json);

            var clients = new HttpClient();
            clients.SetBearerToken(tokenResponse.AccessToken);

            var response = await clients.GetAsync("https://localhost:5005/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                var s = response.StatusCode;
                return response.StatusCode;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }
        [HttpGet("test")]
        public async Task<object> Test() {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                return disco.Error;
            }
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest() {

                Address = disco.TokenEndpoint,
                ClientId= "PasswordClient",
                ClientSecret= "secretPasswordxxxxx",
                UserName="weixiao",
                Password="123",
                Scope="api1"
            });
            Console.WriteLine(tokenResponse.Json);

            var clients = new HttpClient();
            clients.SetBearerToken(tokenResponse.AccessToken);

            var response = await clients.GetAsync("https://localhost:5005/api/values");
            if (!response.IsSuccessStatusCode)
            {
                var s = response.StatusCode;
                return response.StatusCode;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
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
