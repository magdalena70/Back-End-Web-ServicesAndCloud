using OnlineShop.Data.UnitOfWork;
using OnlineShop.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OnlineShop.Services.Controllers
{
    [Authorize]
    public class CategoriesController : BaseApiController
    {
        public CategoriesController(IOnlineShopData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }
    }
}