using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Task6_ANDRE2.Controllers
{
    public class CreateSubscriptionController : ApiController
    {
        // GET: api/CreateSubscription
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CreateSubscription/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CreateSubscription
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/CreateSubscription/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CreateSubscription/5
        public void Delete(int id)
        {
        }
    }
}
