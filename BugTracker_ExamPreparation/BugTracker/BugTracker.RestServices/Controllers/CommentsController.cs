using BugTracker.Data.Models;
using BugTracker.RestServices.Models.BindingModels;
using BugTracker.RestServices.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    public class CommentsController : BaseApiController
    {
        //GET /api/comments
        [HttpGet]
        [Route("api/comments")]
        public IHttpActionResult GetAllComments()
        {
            var comments = this.Data.Comments
                .OrderByDescending(c => c.DateCreated)
                .ThenByDescending(c => c.Id)
                .Select(CommentWithBugDataViewModel.Create);

            return this.Ok(comments);
        }

        //GET /api/bugs/{id}/comments
        [HttpGet]
        [Route("api/bugs/{id}/comments")]
        public IHttpActionResult GetCommentsByBugId(int id)
        {
            var bug = this.Data.Bugs
                .Find(id);
            if(bug == null)
            {
                return this.NotFound();
            }

            var comments = bug.Comments
                .OrderByDescending(c => c.DateCreated)
                .ThenByDescending(c => c.Id)
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    Author = c.Author != null ? c.Author.UserName : null,
                    DateCreated = c.DateCreated
                });

            return this.Ok(comments);
        }

        //POST /api/bugs/{id}/comments
        [HttpPost]
        [Route("api/bugs/{id}/comments")]
        public IHttpActionResult AddCommentForGivenBug(int id, PostCommentBindingModel model)
        {
            if (model == null)
            {
                return BadRequest("Text cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bug = this.Data.Bugs.Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();
            var comment = new Comment
            {
                Text = model.Text,
                AuthorId = currentUserId != null ? currentUserId : null,
                BugId = bug.Id,
                DateCreated = DateTime.Now
            };

            this.Data.Comments.Add(comment);
            this.Data.SaveChanges();

            if (this.User.Identity.IsAuthenticated)
            {
                var currentUserName = User.Identity.GetUserName();
                return this.Ok(new
                {
                    Id = comment.Id,
                    Author = currentUserName,
                    Message = "User comment added for bug #" + bug.Id
                });
            }

            return this.Ok(new 
            {
                Id = comment.Id,
                Message = "Added anonymous comment for bug #" + bug.Id
            });
        }
    }
}