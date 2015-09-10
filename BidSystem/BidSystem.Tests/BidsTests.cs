namespace BidSystem.Tests
{
    using System;
    using System.Net;
    using System.Net.Http;

    using BidSystem.Tests.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BidsTests
    {
        [TestMethod]
        public void CreateBid_ValidBids_ShouldCreateBidCorrectly()
        {
            // Arrange -> clean database, register new user, create an offer
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");
            var offerModel = new OfferModel() { Title = "Title", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(5) };
            var httpResultOffer = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offerModel.Title, offerModel.Description, offerModel.InitialPrice, offerModel.ExpirationDateTime);
            Assert.AreEqual(HttpStatusCode.Created, httpResultOffer.StatusCode);
            var offer = httpResultOffer.Content.ReadAsAsync<OfferModel>().Result;

            // Act -> create a few bids
            var bidsToAdds = new BidModel[]
            {
                new BidModel() { BidPrice = 250, Comment = "My initial bid" },
                new BidModel() { BidPrice = 300, Comment = "My second bid" },
                new BidModel() { BidPrice = 400, Comment = "My third bid" },
                new BidModel() { BidPrice = 500 }
            };
            foreach (var bid in bidsToAdds)
            {
                var httpResultBid = TestingEngine.CreateBidHttpPost(userSession.Access_Token, offer.Id, bid.BidPrice, bid.Comment);
                Assert.AreEqual(HttpStatusCode.OK, httpResultBid.StatusCode);
            }

            // Assert -> bids created successfully
            var bidsCount = TestingEngine.GetBidsCountFromDb();
            Assert.AreEqual(4, bidsCount);
        }

        [TestMethod]
        public void CreateBid_InvalidBids_ShouldCreateBidCorrectly()
        {
            // Arrange -> clean database, register new user, create an offer
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");
            var offerModel = new OfferModel() { Title = "Title", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(5) };
            var httpResultOffer = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offerModel.Title, offerModel.Description, offerModel.InitialPrice, offerModel.ExpirationDateTime);
            Assert.AreEqual(HttpStatusCode.Created, httpResultOffer.StatusCode);
            var offer = httpResultOffer.Content.ReadAsAsync<OfferModel>().Result;

            // Act -> try to create a few bids
            var bids = new BidModel[]
            {
                new BidModel() { BidPrice = 150, Comment = "Invalid: less than the initioal price" },
                new BidModel() { BidPrice = null, Comment = "Invalid: null price" },
                new BidModel() { BidPrice = 300, Comment = "Valid" },
                new BidModel() { BidPrice = 280, Comment = "Invalid: less than the max price" },
            };
            var httpResultBid0 = TestingEngine.CreateBidHttpPost(userSession.Access_Token, offer.Id, bids[0].BidPrice, bids[0].Comment);
            var httpResultBid1 = TestingEngine.CreateBidHttpPost(userSession.Access_Token, offer.Id, bids[1].BidPrice, bids[1].Comment);
            var httpResultBid2 = TestingEngine.CreateBidHttpPost(userSession.Access_Token, offer.Id, bids[2].BidPrice, bids[2].Comment);
            var httpResultBid3 = TestingEngine.CreateBidHttpPost(userSession.Access_Token, offer.Id, bids[3].BidPrice, bids[3].Comment);

            // Assert -> valid bids are created, invalid not created
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResultBid0.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResultBid1.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, httpResultBid2.StatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResultBid3.StatusCode);
            var bidsCount = TestingEngine.GetBidsCountFromDb();
            Assert.AreEqual(1, bidsCount);
        }

        [TestMethod]
        public void CreateBid_InvalidOffer_ShouldReturnNotFound()
        {
            // Arrange -> clean database, register new user
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");

            // Act -> try to create a bid
            var invalidOfferId = -999;
            var httpResultBid = TestingEngine.CreateBidHttpPost(userSession.Access_Token, invalidOfferId, 1000, "Some comment");

            // Assert -> bid not created
            Assert.AreEqual(HttpStatusCode.NotFound, httpResultBid.StatusCode);
            var bidsCount = TestingEngine.GetBidsCountFromDb();
            Assert.AreEqual(0, bidsCount);
        }

        [TestMethod]
        public void CreateBid_Unauthorized_ShouldReturnUnauthorized()
        {
            // Arrange -> clean database, register new user, create an offer
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("peter", "pAssW@rd#123456");
            var offerModel = new OfferModel() { Title = "Title", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(5) };
            var httpResultOffer = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offerModel.Title, offerModel.Description, offerModel.InitialPrice, offerModel.ExpirationDateTime);
            Assert.AreEqual(HttpStatusCode.Created, httpResultOffer.StatusCode);
            var offer = httpResultOffer.Content.ReadAsAsync<OfferModel>().Result;

            // Act -> try to create a bid
            var httpResultBid = TestingEngine.CreateBidHttpPost(null, offer.Id, 1000, "Some comment");

            // Assert -> bid not created
            Assert.AreEqual(HttpStatusCode.Unauthorized, httpResultBid.StatusCode);
            var bidsCount = TestingEngine.GetBidsCountFromDb();
            Assert.AreEqual(0, bidsCount);
        }
    }
}
