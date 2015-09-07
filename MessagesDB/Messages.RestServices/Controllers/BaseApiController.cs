using Messages.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Messages.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        protected BaseApiController()
            : this(new MessagesDbContext())
        {

        }

        protected BaseApiController(MessagesDbContext data)
        {
            this.Data = data;
        }

        protected MessagesDbContext Data { get; set; }
    }
}