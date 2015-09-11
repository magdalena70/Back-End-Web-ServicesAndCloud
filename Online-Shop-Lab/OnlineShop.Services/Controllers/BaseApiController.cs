namespace OnlineShop.Services.Controllers
{
    using System.Web.Http;
    using Infrastructure;
    using OnlineShop.Data.UnitOfWork;

    public class BaseApiController : ApiController
    {
        protected BaseApiController(IOnlineShopData data,
            IUserIdProvider userIdProvider)
        {
            this.Data = data;
            this.UserIdProvider = userIdProvider;
        }

        protected IOnlineShopData Data { get; set; }

        protected IUserIdProvider UserIdProvider { get; set; }
    }
}