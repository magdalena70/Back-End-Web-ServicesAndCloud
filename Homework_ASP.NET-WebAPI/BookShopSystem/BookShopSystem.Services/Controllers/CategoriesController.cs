using BookShopSystem.Data;
using BookShopSystem.Models;
using BookShopSystem.Services.Models.BindingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace BookShopSystem.Services.Controllers
{
    public class CategoriesController : BaseApiController
    {
        // GET api/Categories
        public IHttpActionResult GetCategories()
        {
            var categories = this.Data.Categories
                .Select(c => new
            {
                Id = c.Id,
                Name = c.Name
            });
            return this.Ok(categories);
        }

        //GET /api/Categories/{id}
        public IHttpActionResult GetCategorieById(int id)
        {
            var category = this.Data.Categories
                .Where(c => c.Id == id)
                .Select(c => new
            {
                Name = c.Name
            });

            if (!category.Any())
            {
                return this.NotFound();
            }

            return this.Ok(category);
        }

        //PUT /api/Categories/{id}
        [HttpPut]
        [Authorize]
        public IHttpActionResult ChangeCategorieById(int id, Category category)
        {
            var categoryToChange = this.Data.Categories
                .First(c => c.Id == id);
            if (categoryToChange == null)
            {
                return this.NotFound();
            }

            categoryToChange.Name = category.Name;
            this.Data.SaveChanges();
            return this.Ok("successful change");
        }

        //DELETE /api/Categories/{id}
        [HttpDelete]
        [Authorize]
        public IHttpActionResult DeleteCategorie(int id)
        {
            try
            {
                var category = this.Data.Categories
                    .First(c => c.Id == id);
                this.Data.Categories.Remove(category);
                this.Data.SaveChanges();
                return this.Ok("deleted successfully");
            }
            catch (Exception)
            {
                return this.NotFound();
            }
        }

        //POST /api/categories
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddCategorie(CategoryBindingModel model)
        {
            if (model == null)
            {
                this.ModelState.AddModelError("model", "The model cannot be null - (request is empty).");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var category = new Category()
            {
                Name = model.Name,
            };

            this.Data.Categories.Add(category);
            this.Data.SaveChanges();
            return this.Ok("created category  with id = " + category.Id);
        }
    }
}