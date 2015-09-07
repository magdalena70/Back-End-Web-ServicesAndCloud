using Messages.Data.Models;
using Messages.RestServices.Models.BindingModels;
using Messages.RestServices.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Messages.RestServices.Controllers
{
    public class ChannelsController : BaseApiController
    {
        // GET /api/channels
        [HttpGet]
        public IHttpActionResult GetChannels()
        {
            var channels = this.Data.Channels
                .OrderBy(ch => ch.Name)
                .Select(ChannelViewModel.Create);

            return this.Ok(channels);
        }

        //GET /api/Channels/id
        [HttpGet]
        public IHttpActionResult GetChannelByID(int id)
        {
            var channel = this.Data.Channels
                .Where(ch => ch.Id == id)
                .Select(ChannelViewModel.Create)
                .FirstOrDefault();

            if (channel == null)
            {
                return this.NotFound();
            }

            return this.Ok(channel);
        }

        //POST /api/channels
        [HttpPost]
        public IHttpActionResult CreateNewChannel(AddChannelBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Name is required!Cannot be empty.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (this.Data.Channels.Any(ch => ch.Name == model.Name))
            {
                return this.Conflict();
            }

            var channel = new Channel
            {
                Name = model.Name
            };

            this.Data.Channels.Add(channel);
            this.Data.SaveChanges();

            var result = new ChannelViewModel()
            {
                Id = channel.Id,
                Name = channel.Name
            };

            // return status 201, Location and (channel Id and name)
            return this.CreatedAtRoute("DefaultApi", new { id = channel.Id }, result);
        }

        //PUT /api/channels/{id}
        [HttpPut]
        public IHttpActionResult EditExistingChannel(int id, AddChannelBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Name is required!cannot be empty.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channel = this.Data.Channels
                .FirstOrDefault(ch => ch.Id == id);
            if (channel == null)
            {
                return this.NotFound();
            }

            if (this.Data.Channels
                .Any(ch => ch.Name == model.Name && ch.Id != channel.Id))
            {
                return this.Conflict();
            }

            channel.Name = model.Name;
            this.Data.SaveChanges();
            return this.Ok(new
            {
                Message = string.Format("Channel #{0} edited successfully.", channel.Id)
            });
        }

        //DELETE /api/channels/{id}
        [HttpDelete]
        public IHttpActionResult DeleteChannelByID(int id)
        {
            var channel = this.Data.Channels
                .Find(id);
            if (channel == null)
            {
                return this.NotFound();

            }

            if (channel.ChannelMessages.Any())
            {
                return this.Content(HttpStatusCode.Conflict, new
                {
                    Message = string.Format(
                    "Cannot delete channel #{0} because it is not empty!",
                    channel.Id)
                });
            }

            this.Data.Channels.Remove(channel);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                Message = string.Format("Channel #{0} deleted.", channel.Id)
            });

        }
    }
}