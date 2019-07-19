using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using CoreDemo_AutoFac.Test;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo_AutoFac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private IUser _user;
        public ValuesController(IUser user, IComponentContext componentContext)
        {
            //_user = user;
            _user = componentContext.ResolveNamed<IUser>(typeof(User).Name);
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string name)
        {
            var s = _user.GetName(name);
            return _user.GetName(name);
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
