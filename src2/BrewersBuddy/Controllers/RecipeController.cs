using BrewersBuddy.Models;
using BrewersBuddy.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BrewersBuddy.Controllers
{
    [Authorize]
    public class RecipeController : Controller
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            if (recipeService == null)
                throw new ArgumentNullException("recipeService");

            _recipeService = recipeService;
        }

        //
        // GET: /Recipe/
        public ActionResult Index()
        {
            int currentUserId = ControllerUtils.GetCurrentUserId(User);
            IEnumerable<Recipe> recipes = _recipeService.GetAllForUser(currentUserId);

            return View(recipes);
        }

        //
        // GET: /Recipe/Details/5
        public ActionResult Details(int id = 0)
        {
            Recipe Recipe = _recipeService.Get(id);
            if (Recipe == null)
            {
                return HttpNotFound();
            }
            return View(Recipe);
        }

        //
        // GET: /Recipe/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                //Set the start date to now
                recipe.AddDate = DateTime.Now;
                //Tie the object to the user
                recipe.OwnerId = ControllerUtils.GetCurrentUserId(User);

                _recipeService.Create(recipe);
                return RedirectToAction("Index");
            }


            return View(recipe);
        }

        //
        // GET: /Recipe/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Recipe Recipe = _recipeService.Get(id);
            if (Recipe == null)
            {
                return HttpNotFound();
            }
            return View(Recipe);
        }


        //
        // POST: /Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = _recipeService.Get(id);
            _recipeService.Delete(recipe);
            return RedirectToAction("Index");
        }

    }
}

