using BidSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BidSystem.RestServices.Controllers
{
    public class BaseApiController : ApiController
    {
        protected BaseApiController()
            : this(new BidSystemDbContext())
        {
        }

        protected BaseApiController(BidSystemDbContext data)
        {
            this.Data = data;
        }

        protected BidSystemDbContext Data { get; set; }
    }
}