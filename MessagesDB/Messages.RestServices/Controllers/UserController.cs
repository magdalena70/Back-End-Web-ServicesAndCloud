using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Messages.RestServices.Models.ViewModels;
using Messages.Data.Models;
using Messages.RestServices.Models.BindingModels;

namespace Messages.RestServices.Controllers
{
    //GET /api/user/personal-messages
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        [HttpGet]
        [Route("personal-messages")]
        public IHttpActionResult GetAllPersonalMessages()
        {
            var userId = this.User.Identity.GetUserId();
            var messages = this.Data.UserMessages
                .Where(um => um.RecipientId == userId)
                .OrderByDescending(um => um.DateSent)
                .ThenBy(um => um.Id)
                .Select(UserMessageViewModel.Create);

            return this.Ok(messages);
        }

        //POST /api/user/personal-messages
        [HttpPost]
        [AllowAnonymous]
        [Route("personal-messages")]
        public IHttpActionResult PostMessage(UserMessageBindingModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var recipient = this.Data.Users
                .FirstOrDefault(x => x.UserName == model.Recipient); // get all data
            
            if (recipient == null)
            {
                return this.BadRequest("Recipient is not found!");
            }

            var userMessage = new UserMessage
            {
                RecipientId = recipient.Id,
                Text = model.Text,
                DateSent = DateTime.Now
            };

            if (this.User.Identity.IsAuthenticated)
            {
                var senderId = this.User.Identity.GetUserId();
               /* if (senderId == userMessage.RecipientId)
                {
                    return this.BadRequest("Тhe recipient must be a different user");
                }*/

                userMessage.SenderId = senderId;
            }

            this.Data.UserMessages.Add(userMessage);
            this.Data.SaveChanges();

            if (this.User.Identity.IsAuthenticated)
            {
                return this.Ok(new
                {
                    Id = userMessage.Id,
                    Sender = this.User.Identity.GetUserName(),
                    Message = "Message sent to user " + recipient.UserName
                });
            }

            return this.Ok(new
            {
                Id = userMessage.Id,
                Message = "Anonymous message sent successfully to user " + recipient.UserName
            });
        }

    }
}