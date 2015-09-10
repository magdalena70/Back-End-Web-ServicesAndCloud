namespace BidSystem.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using BidSystem.Data;
    using BidSystem.Data.Models;
    using BidSystem.RestServices;
    using BidSystem.Tests.Models;

    using EntityFramework.Extensions;

    using Microsoft.Owin.Testing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Owin;
    using System.Net.Http.Headers;

    [TestClass]
    public static class TestingEngine
    {
        private static TestServer TestWebServer { get; set; }

        public static HttpClient HttpClient { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Start OWIN testing HTTP server with Web API support
            TestWebServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var webAppStartup = new Startup();
                webAppStartup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });
            HttpClient = TestWebServer.HttpClient;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Stop the OWIN testing HTTP server
            TestWebServer.Dispose();
        }

        public static void CleanDatabase()
        {
            using (var dbContext = new BidSystemDbContext())
            {
                dbContext.Bids.Delete();
                dbContext.Offers.Delete();
                dbContext.Users.Delete();
                dbContext.SaveChanges();
            }
        }

        public static int GetOffersCountFromDb()
        {
            using (var dbContext = new BidSystemDbContext())
            {
                return dbContext.Offers.Count();
            }
        }

        public static int GetBidsCountFromDb()
        {
            using (var dbContext = new BidSystemDbContext())
            {
                return dbContext.Bids.Count();
            }
        }

        public static HttpResponseMessage RegisterUserHttpPost(string username, string password)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            var httpResponse = TestingEngine.HttpClient.PostAsync("/api/user/register", postContent).Result;
            return httpResponse;
        }

        public static HttpResponseMessage LoginUserHttpPost(string username, string password)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            var httpResponse = TestingEngine.HttpClient.PostAsync("/api/user/login", postContent).Result;
            return httpResponse;
        }

        public static UserSessionModel RegisterUser(string username, string password)
        {
            var httpResponse = TestingEngine.RegisterUserHttpPost(username, password);
            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            var userSession = httpResponse.Content.ReadAsAsync<UserSessionModel>().Result;
            return userSession;
        }

        public static UserSessionModel LoginUser(string username, string password)
        {
            var httpResponse = TestingEngine.LoginUserHttpPost(username, password);
            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
            var userSession = httpResponse.Content.ReadAsAsync<UserSessionModel>().Result;
            return userSession;
        }

        public static HttpResponseMessage CreateOfferHttpPost(string userSessionToken, string title, string description, decimal? initialPrice, DateTime? expirationDateTime)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("title", title),
                new KeyValuePair<string, string>("description", description),
                new KeyValuePair<string, string>("initialPrice", initialPrice != null ?
                    initialPrice.Value.ToString(CultureInfo.InvariantCulture) : null),
                new KeyValuePair<string, string>("expirationDateTime", expirationDateTime != null ?
                    expirationDateTime.Value.ToString("r") : null),
            });
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/offers", UriKind.Relative),
                Content = postContent
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSessionToken);
            var httpResponse = TestingEngine.HttpClient.SendAsync(httpRequest).Result;
            return httpResponse;
        }

        public static HttpResponseMessage CreateBidHttpPost(string userSessionToken, int? offerId, decimal? bidPrice, string comment)
        {
            var postContent = new FormUrlEncodedContent(new[] 
            {
                new KeyValuePair<string, string>("bidPrice", bidPrice != null ?
                    bidPrice.Value.ToString(CultureInfo.InvariantCulture) : null),
                new KeyValuePair<string, string>("comment", comment)
            });
            var httpRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("/api/offers/" + offerId + "/bid", UriKind.Relative),
                Content = postContent
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userSessionToken);
            var httpResponse = TestingEngine.HttpClient.SendAsync(httpRequest).Result;
            return httpResponse;
        }
    }
}
