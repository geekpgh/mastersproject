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
        private readonly IUserService _userService;

        public RecipeController(
            IRecipeService recipeService,
            IUserService userService)
        {
            if (recipeService == null)
                throw new ArgumentNullException("recipeService");
            if (userService == null)
                throw new ArgumentNullException("userService");

            _recipeService = recipeService;
            _userService = userService;
        }

        //
        // GET: /Recipe/
        public ActionResult Index()
        {
            int currentUserId = _userService.GetCurrentUserId();
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
                recipe.OwnerId = _userService.GetCurrentUserId();

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

