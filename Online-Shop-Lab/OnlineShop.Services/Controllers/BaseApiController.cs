using OnlineShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OnlineShop.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new OnlineShopContext())
        {

        }

        public BaseApiController(OnlineShopContext data)
        {
            this.Data = data;
        }

        protected OnlineShopContext Data { get; set; }
    }
}