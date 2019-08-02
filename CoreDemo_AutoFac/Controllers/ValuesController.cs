using System.Collections.Generic;
using Autofac;
using CoreDemo_AutoFac.Test;
using Microsoft.AspNetCore.Mvc;
namespace CoreDemo_AutoFac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IAnimal animal;
        IAnimal cat;
        IAnimal dog;
        public ValuesController(IAnimal _animal, IComponentContext componentContext)
        {
            animal = _animal;
            cat = componentContext.ResolveNamed<IAnimal>(typeof(Cat).Name);
            dog = componentContext.ResolveNamed<IAnimal>(typeof(Cat).Name);
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
            var s = animal.GetArg(10);
            //var dd=dog.Name+cat.Name;
            return dog.Name;
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
