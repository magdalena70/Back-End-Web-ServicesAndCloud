using BookShopSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BookShopSystem.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new BookShopSystemContext())
        {

        }

        public BaseApiController(BookShopSystemContext data)
        {
            this.Data = data;
        }

        public BookShopSystemContext Data { get; set; }
    }
}