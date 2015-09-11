namespace OnlineShop.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using Infrastructure;
    using Microsoft.AspNet.Identity;
    using Models.BindingModels;
    using Models.ViewModels;
    using OnlineShop.Models;
    using OnlineShop.Data.UnitOfWork;

    [Authorize]
    public class AdsController : BaseApiController
    {
        public AdsController(IOnlineShopData data, IUserIdProvider userIdProvider)
            : base(data, userIdProvider)
        {
        }

        // get all open ads
        [HttpGet]
        [Route("api/ads")]
        [AllowAnonymous]
        public IHttpActionResult GetAds()
        {
            var ads = this.Data.Ads.All()
                .Where(a => a.Status == AdStatus.Open)
                .OrderByDescending(a => a.TypeId)
                .ThenBy(a => a.PostedOn)
                .Select(AdViewModel.Create);

            return this.Ok(ads);
        }

        // post ad
        [HttpPost]
        [Route("api/ads")]
        public IHttpActionResult CreateNewAd(CreateAdBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("model is empty");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (!this.Data.AdTypes.All().Any(t => t.Id == model.TypeId))
            {
                string badRequest = string.Format("There is no type with id - {0}",
                    model.TypeId);
                return this.BadRequest(badRequest);
            }

            var userId = this.User.Identity.GetUserId();
            var ad = new Ad
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                PostedOn = DateTime.Now,
                Status = AdStatus.Open,
                OwnerId = userId,
                TypeId = model.TypeId
            };

            if(model.Categories.Count() >= 1 && model.Categories.Count() <= 3){
                foreach (var categoryId in model.Categories)
                {
                    var category = this.Data.Categories.Find(categoryId);
                    if (category == null)
                    {
                        return this.BadRequest("Non existing category");
                    }

                    ad.Categories.Add(category);
                }
            }
            else
            {
                return this.BadRequest("You can add at least 1 category and no more than 3");
            }

            this.Data.Ads.Add(ad);
            this.Data.SaveChanges();

            var result = this.Data.Ads.All()
                .Where(a => a.Id == ad.Id)
                .Select(AdViewModel.Create)
                .FirstOrDefault();

            return this.Ok(result);
        }

        // close your own ad by adId
        [HttpPut]
        [Route("api/ads/{id}/close")]
        public IHttpActionResult CloseAd(int id)
        {
            var ad = this.Data.Ads.Find(id);
            if (ad == null)
            {
                return this.NotFound();
            }

            string userId = this.User.Identity.GetUserId();
            if (ad.OwnerId != userId)
            {
                return this.Unauthorized();
            }

            if (ad.Status == AdStatus.Closed)
            {
                return this.BadRequest("Ad already closed");
            }

            ad.Status = AdStatus.Closed;
            ad.ClosedOn = DateTime.Now;
            this.Data.SaveChanges();

            return this.Ok();
        }
    }
}