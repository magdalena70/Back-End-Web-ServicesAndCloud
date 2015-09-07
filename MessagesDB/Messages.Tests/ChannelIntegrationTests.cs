using Messages.Data;
using Messages.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messages.Tests
{
    //Write Integration Tests for "Delete Channel" Service
    [TestClass]
    public class ChannelIntegrationTests
    {
        [TestMethod]
        public void DeleteChannel_WhenIsNonExisting_ShoulReturn404()
        {
            // Arrange -> create a channel
            TestingEngine.CleanDatabase();
            var nonExistingChannelId = 5;

            // Act -> delete the channel
            var httpDeleteResponse = TestingEngine.HttpClient.DeleteAsync(
                "/api/channels/" + nonExistingChannelId).Result;

            // Assert -> HTTP status code is 404
            Assert.AreEqual(HttpStatusCode.NotFound, httpDeleteResponse.StatusCode);
        }

        [TestMethod]
        public void DeleteChannel_WhenIsExisting_ShoulReturn200()
        {
            // Arrange -> create a channel
            TestingEngine.CleanDatabase();
            var existingChannel = new Channel() 
            {
                Name = "Existing Channel"
            };
            MessagesDbContext dbContext = new MessagesDbContext();
            dbContext.Channels.Add(existingChannel);
            dbContext.SaveChanges();

            // Act -> delete the channel
            var httpDeleteResponse = TestingEngine.HttpClient.DeleteAsync(
                "/api/channels/" + existingChannel.Id).Result;

            // Assert -> HTTP status code is 200
            Assert.AreEqual(HttpStatusCode.OK, httpDeleteResponse.StatusCode);
            var result = httpDeleteResponse.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, dbContext.Channels.Count());
        }

        [TestMethod]
        public void DeleteChannel_WhenIsExistingAndNonEmpty_ShoulReturn409()
        {
            // Arrange -> create a channel
            TestingEngine.CleanDatabase();

            MessagesDbContext dbContext = new MessagesDbContext();
            var existingChannel = new Channel()
            {
                Name = "Existing Channel"
            };
            existingChannel.ChannelMessages.Add(new ChannelMessage()
            {
                Id = 11,
                Text = "Some text",
                DateSent = DateTime.Now,
                Sender = null
            });
            dbContext.Channels.Add(existingChannel);
            dbContext.SaveChanges();

            // Act -> delete the channel
            var httpDeleteResponse = TestingEngine.HttpClient.DeleteAsync(
                "/api/channels/" + existingChannel.Id).Result;

            // Assert -> HTTP status code is 409
            Assert.AreEqual(HttpStatusCode.Conflict, httpDeleteResponse.StatusCode);
            var result = httpDeleteResponse.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, dbContext.Channels.Count());
        }
    }
}
