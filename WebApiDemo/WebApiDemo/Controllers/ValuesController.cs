using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiDemo.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        //[AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/parameters
        [HttpGet]
        [Route("api/values/parameters")]
        public IEnumerable<string> Parameters()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            return "Post: " + value;
        }

        // PUT api/values/5
        public string Put(int id, [FromBody]string value)
        {
            return "Put";
        }

        // DELETE api/values/5
        public string Delete(int id)
        {
            return "Delete";
        }
    }
}
