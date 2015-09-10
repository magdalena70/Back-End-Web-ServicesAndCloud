namespace BidSystem.Tests
{
    using System;
    using System.Net;
    using BidSystem.Tests.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OffersTests
    {
        [TestMethod]
        public void CreateOffers_ValidOffers_ShouldCreateOffersCorrectly()
        {
            // Arrange -> clean the database and register new user
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");

            // Act -> create a few offers
            var offersToAdds = new OfferModel[]
            {
                new OfferModel() { Title = "First Offer (Expired)", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(-5)},
                new OfferModel() { Title = "Another Offer (Expired)", InitialPrice = 15.50m, ExpirationDateTime = DateTime.Now.AddDays(-1)},
                new OfferModel() { Title = "Second Offer (Active 3 months)", Description = "Description", InitialPrice = 500, ExpirationDateTime = DateTime.Now.AddMonths(3)},
                new OfferModel() { Title = "Third Offer (Active 6 months)", InitialPrice = 120, ExpirationDateTime = DateTime.Now.AddMonths(6)},
            };
            foreach (var offer in offersToAdds)
            {
                var httpResult = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offer.Title, offer.Description, offer.InitialPrice, offer.ExpirationDateTime);
                Assert.AreEqual(HttpStatusCode.Created, httpResult.StatusCode);
            }

            // Assert -> offers created correctly
            var offersCount = TestingEngine.GetOffersCountFromDb();
            Assert.AreEqual(4, offersCount);
        }

        [TestMethod]
        public void CreateOffers_InvalidOffers_ShouldReturnBadRequest()
        {
            // Arrange -> clean the database
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");

            // Act -> try to create a few offers
            var offersToAdds = new OfferModel[]
            {
                new OfferModel() { Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(-5)},
                new OfferModel() { Title = "Another Offer (Expired)", ExpirationDateTime = DateTime.Now.AddDays(-1)},
                new OfferModel() { Title = "Second Offer (Active 3 months)", Description = "Description", InitialPrice = 500 },
            };
            foreach (var offer in offersToAdds)
            {
                var httpResult = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offer.Title, offer.Description, offer.InitialPrice, offer.ExpirationDateTime);
                Assert.AreEqual(HttpStatusCode.BadRequest, httpResult.StatusCode);
            }

            // Assert -> offers not created
            var offersCount = TestingEngine.GetOffersCountFromDb();
            Assert.AreEqual(0, offersCount);
        }

        [TestMethod]
        public void CreateOffer_Unauthorized_ShouldReturnUnauthorized()
        {
            // Arrange -> clean the database
            TestingEngine.CleanDatabase();

            // Act -> try to create an offer
            var offer = new OfferModel() { Title = "Title", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(5)};
            var httpResult = TestingEngine.CreateOfferHttpPost(null, offer.Title, offer.Description, offer.InitialPrice, offer.ExpirationDateTime);

            // Assert -> offer not created
            Assert.AreEqual(HttpStatusCode.Unauthorized, httpResult.StatusCode);
            var offersCount = TestingEngine.GetOffersCountFromDb();
            Assert.AreEqual(0, offersCount);
        }
    }
}
