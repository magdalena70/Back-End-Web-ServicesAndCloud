namespace BidSystem.RestServices.Controllers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using BidSystem.Data;
    using BidSystem.Data.Models;
    using BidSystem.RestServices.Models;

    using Microsoft.AspNet.Identity;
    using System.Web.Http;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Testing;

    [RoutePrefix("api/user")]
    public class UsersController : ApiController
    {
        private ApplicationUserManager userManager;

        public UsersController()
        {
            this.userManager = new ApplicationUserManager(new UserStore<User>(new BidSystemDbContext()));
        }
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        // POST api/user/register
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterUser(UserAccountInputModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new User
            {
                UserName = model.Username
            };

            var identityResult = await this.UserManager.CreateAsync(user, model.Password);

            if (!identityResult.Succeeded)
            {
                return this.GetErrorResult(identityResult);
            }

            // Auto login after registrаtion (successful user registration should return access_token)
            var loginResult = await this.LoginUser(new UserAccountInputModel()
            {
                Username = model.Username,
                Password = model.Password
            });
            return loginResult;
        }

        // POST api/user/login
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> LoginUser(UserAccountInputModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            // Invoke the "token" OWIN service to perform the login (POST /api/token)
            var testServer = TestServer.Create<Startup>();

            var requestParams = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", model.Username),
                new KeyValuePair<string, string>("password", model.Password)
            };
            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
            var tokenServiceResponse = await testServer.HttpClient.PostAsync(
                Startup.TokenEndpointPath, requestParamsFormUrlEncoded);

            return this.ResponseMessage(tokenServiceResponse);
        }
      
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
