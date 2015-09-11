namespace OnlineShop.Tests.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using OnlineShop.Data;
    using OnlineShop.Models;
    using OnlineShop.Data.UnitOfWork;
    using OnlineShop.Services.Infrastructure;
    using OnlineShop.Services.Controllers;
    using OnlineShop.Services.Models.ViewModels;
    using Services.Models.BindingModels;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Threading;
  
    [TestClass]
    public class AdsControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();
        }

        [TestMethod]
        public void GetAllAds_Should_Return_Total_Ads_Sorted_By_TypeIndex()
        {
            //Arrange
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();

            var mockContext = new Mock<IOnlineShopData>();
            var mockUserIdProvider = new Mock<IUserIdProvider>();
            mockContext.Setup(c => c.Ads.All())
                .Returns(fakeAds);

            var adsController = new AdsController(mockContext.Object,
                mockUserIdProvider.Object);
            this.SetupController(adsController);

            //Act- invoke the GetAllAds() method from the controller.
            var response = adsController.GetAds()
                .ExecuteAsync(CancellationToken.None).Result;

            //Assert that the response status code is 200 OK
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //Deserialize the response data with ReadAsAsync<T>(). Select only the IDs.
            var adsResponse = response.Content.ReadAsAsync<IEnumerable<AdViewModel>>()
                .Result.Select(a => a.Id)
                .ToList();

            //Order the fake ads collection just like the controller does the ordering.
            //Select only the IDs.
            var orderedFakeAds = fakeAds
                .OrderBy(a => a.Type.Index)
                .ThenBy(a => a.PostedOn)
                .Select(a => a.Id)
                .ToList();

            //Assert that the two collections have the same elements
            CollectionAssert.AreEqual(orderedFakeAds, adsResponse);
        }

        private void SetupController(AdsController adsController)
        {
            adsController.Request = new HttpRequestMessage();
            adsController.Configuration = new HttpConfiguration();
        }
    }
}
