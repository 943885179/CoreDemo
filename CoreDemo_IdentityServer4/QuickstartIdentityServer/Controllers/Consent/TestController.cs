using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickstartIdentityServer.Controllers.Consent
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private MyDbContext db;

        public TestController(MyDbContext db)
        {
            this.db = db;
        }
        public ActionResult<object> Get()
        {
                return db.User.FirstOrDefault();
        }
    }
}