using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiDemo.Controllers
{
    [RoutePrefix("api/posts")]
    public class PostController : ApiController
    {
        [HttpGet]
        public Post Post()
        {
            Post post = new Post();
            return post;
        }

        public Post Get(string name)
        {
            Post post = new Post(name);
            return post;
        }

        [HttpPost]
        public IHttpActionResult Post(string name, int rating)
        {
            Post post = new Post(name, rating);
            return this.Ok(new PostViewModel() { 
                Name = post.Name,
                Rating = post.Rating,
                Author = "Some Author",
                AnswersCount = 17
            });
        }

        [HttpGet]
        [Route("binding")]
        public IHttpActionResult GetPostModel([FromUri]PostBindingModel model)
        {
            if(!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            Post post = new Post(model.Name, model.Rating);
            return this.Ok(post);
        }

        [HttpGet]
        [Route("{postName}/rating")]
        public string PostName(string postName)
        {
            return postName;
        }
    }

    public class Post
    {
        public string Name { get; set; }

        public int Rating { get; set; }

        public Post()
        {
            Name = "no name";
            Rating = 0;
        }

        public Post(string name)
        {
            Name = name;
            Rating = 0;
        }

        public Post(string name, int rating)
        {
            Name = name;
            Rating = rating;
        }
    }

    public class PostBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        public int Rating { get; set; }
    }

    public class PostViewModel
    {
        public string Name { get; set; }

        public int Rating { get; set; }

        public string Author { get; set; }

        public int AnswersCount { get; set; }
    }
}
