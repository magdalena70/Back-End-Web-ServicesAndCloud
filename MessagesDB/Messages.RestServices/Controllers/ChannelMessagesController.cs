using Messages.Data.Models;
using Messages.RestServices.Models.BindingModels;
using Messages.RestServices.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Messages.RestServices.Controllers
{
    [RoutePrefix("api/channel-messages")]
    public class ChannelMessagesController : BaseApiController
    {
        //GET /api/channel-messages/{channelName}
        [HttpGet]
        [Route("{channelName}")]
        public IHttpActionResult GetChannelMessages(string channelName)
        {
            if (!this.Data.Channels.Any(ch => ch.Name == channelName))
            {
                return this.NotFound();
            }

            var channelMessages = this.Data.ChannelMessages
                .Where(m => m.Channel.Name == channelName)
                .OrderByDescending(m => m.DateSent)
                .ThenBy(m => m.Id)
                .Select(ChannelMessagesViewModel.Create);

            return this.Ok(channelMessages);
        }

        //GET /api/channel-messages/{channel}?limit={limit}
        [HttpGet]
        [Route("{channelName}")]
        public IHttpActionResult GetChannelMessagesWithLimit(string channelName, string limit)
        {
            if (!this.Data.Channels.Any(ch => ch.Name == channelName))
            {
                return this.NotFound();
            }

            int limitRange = 0;
            int.TryParse(limit, out limitRange);
            if (limitRange < 1 || limitRange > 1000)
            {
                return this.BadRequest("Limit is not integer or out of range [1 ... 1000]");
            }

            var channelMessages = this.Data.ChannelMessages
                .Where(m => m.Channel.Name == channelName)
                .OrderByDescending(m => m.DateSent)
                .ThenBy(m => m.Id)
                .Take(limitRange)
                .Select(ChannelMessagesViewModel.Create);

            return this.Ok(channelMessages);
        }

        //POST /api/channel-messages/{channel-name}
        [HttpPost]
        [Route("{channelName}")]
        public IHttpActionResult PostChannelMessages(string channelName, ChannelMessagesBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Name is required!Cannot be empty.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channel = this.Data.Channels
                .FirstOrDefault(ch => ch.Name == channelName);

            if (channel == null)
            {
                return this.NotFound();
            }

            var message = new ChannelMessage
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                ChannelId = channel.Id
            };

            if (this.User.Identity.IsAuthenticated)
            {
                message.SenderId = this.User.Identity.GetUserId();

            }

            this.Data.ChannelMessages.Add(message);
            this.Data.SaveChanges();
            
            if (this.User.Identity.IsAuthenticated)
            {
                return this.Ok(new
                {
                    Id = message.Id,
                    Sender = this.User.Identity.GetUserName(),
                    Message = "Message sent to channel " + channel.Name
                });

            }

            return this.Ok(new
            {
                Id = message.Id,
                Message = "Anonymous message sent to channel " + channel.Name
            });
        }
    }
}