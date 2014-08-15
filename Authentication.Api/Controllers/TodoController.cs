using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Authentication.API.Controllers
{
    [RoutePrefix("api/Todo")]
    public class TodoController : ApiController
    {
        [Authorize(Roles = "user")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return this.Ok(new List<string> { "mjölk", "socker", "ost" });
        }

        [Authorize(Roles = "admin")]
        [Route("Apa")]
        [HttpGet]
        public IHttpActionResult Apa()
        {
            return this.Ok(new List<string> { "mjölk", "socker", "bröd" });
        }
    }
}
