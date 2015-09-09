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
    [RoutePrefix("api/bugs")]
    public class BugsController : BaseApiController
    {
        //GET /api/bugs
        [HttpGet]
        public IQueryable<BugViewModel> GetBugs()
        {
            var bugs = this.Data.Bugs
                .OrderByDescending(b => b.DateCreated)
                .ThenByDescending(b => b.Id)
                .Select(BugViewModel.Create);
                
            return bugs;
        }

        //GET /api/bugs/filter
        [HttpGet]
        [Route("filter")]
        public IEnumerable<BugViewModel> GetBugsByFilter(
            [FromUri] string keyword = null,
            [FromUri] string statuses = null,
            [FromUri] string author = null)
        {
            IQueryable<BugViewModel> bugs = this.GetBugs();

            if (keyword != null)
            {
                bugs = bugs.Where(b => b.Title.Contains(keyword));
            }

            if (statuses != null)
            {
                string[] allowedStatuses = statuses.Split('|');
                bugs = bugs.Where(b => statuses.Contains(b.Status));
            }

            if (author != null)
            {
                bugs = bugs.Where(b => b.Author == author);
            }

            return bugs;
        }

        //GET /api/bugs/{id}
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetBugById(int id)
        {
            var bug = this.Data.Bugs
                .Where(b => b.Id == id)
                .Select(BugByIdViewModel.Create)
                .FirstOrDefault();

            if (bug == null)
            {
                return this.NotFound();
            }

            return Ok(bug);
        }

        //POST /api/bugs
        [HttpPost]
        public IHttpActionResult PostBug(PostBugBindingModel model)
        {
            if (model == null)
            {
                return BadRequest("Title is required, cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = User.Identity.GetUserId();
            var bug = new Bug
            { 
                Title = model.Title,
                Description = model.Description,
                AuthorId = currentUserId,
                DateCreated = DateTime.Now,
                Status = BugStatus.Open
            };

            this.Data.Bugs.Add(bug);
            this.Data.SaveChanges();

            if (this.User.Identity.IsAuthenticated)
            {
                var authorUserName = User.Identity.GetUserName();
                return this.CreatedAtRoute(
                    "DefaultApi",
                    new { id = bug.Id },
                    new 
                    {
                        bug.Id, 
                        Author = authorUserName, 
                        Message = "User bug submitted." 
                    });
            }

            return this.CreatedAtRoute(
                    "DefaultApi",
                    new { id = bug.Id },
                    new
                    {
                        bug.Id,
                        Message = "Anonymous  bug submitted."
                    });
        }

        //PATCH /api/bugs/{id}
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult EditBug(int id, EditBugBidingModel model)
        {
            var bug = this.Data.Bugs
                .FirstOrDefault(b => b.Id == id);
            if (bug == null)
            {
                return this.NotFound();
            }

            if (model == null)
            {
                return this.BadRequest("Title cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bug.Title = model.Title;
            if (model.Description != null)
            {
                bug.Description = model.Description;
            }

            if (model.Status != null)
            {
                bug.Status = model.Status.Value;
            }

            this.Data.SaveChanges();
            return this.Ok(new
            {
                Message = string.Format("Bug #{0} patched.", bug.Id)
            });
        }

        //DELETE /api/bugs/{id}
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteBugById(int id)
        {
            var bug = this.Data.Bugs
                .Find(id);
            if (bug == null)
            {
                return this.NotFound();
            }

            this.Data.Bugs.Remove(bug);
            this.Data.SaveChanges();

            return this.Ok(new 
            { 
                Message = string.Format("Bug #{0} deleted.", bug.Id) 
            });
        }
    }
}