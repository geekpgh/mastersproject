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
            Recipe recipe = _recipeService.Get(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            CheckViewAuthorization(id);

            int currentUserId = _userService.GetCurrentUserId();

            //See if they can edit so we can disable things if this is read only
            //This removed all buttons from the view that require edit privs.
            ViewBag.CanEdit = recipe.CanEdit(currentUserId);
            //If they aren't the owner remove the add collaborator links
            ViewBag.IsOwner = recipe.IsOwner(currentUserId);

            return View(recipe);
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
		// GET: /Account/Edit/

		public ActionResult Edit(int id = 0)
		{
            CheckEditAuthorization(id);

			foreach (Recipe recipe in _recipeService.GetAllForUser(_userService.GetCurrentUserId()))
			{
				if (recipe.RecipeId == id)
				{
					return View(recipe);
				}
			}

			return HttpNotFound();
		}

		//
		// GET: /Account/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Recipe recipe)
		{
            CheckEditAuthorization(recipe.RecipeId);

			if (ModelState.IsValid)
			{
				_recipeService.Update(recipe);
                return RedirectToAction("Details/" + recipe.RecipeId);
			}

			ModelState.AddModelError("", "Error saving changes to recipe.");
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

            CheckEditAuthorization(id);

            return View(Recipe);
        }


        //
        // POST: /Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckEditAuthorization(id);
            Recipe recipe = _recipeService.Get(id);
            _recipeService.Delete(recipe);
            return RedirectToAction("Index");
        }


        //
        // GET: /Recipe/Friends
        public ActionResult Friends()
        {
            int currentUserId = _userService.GetCurrentUserId();
            ICollection<UserProfile> friendProfiles = _userService.FriendProfiles(currentUserId);
            List<Recipe> friendRecipess = new List<Recipe>();

            foreach (UserProfile friendProfile in friendProfiles)
            {
                IEnumerable<Recipe> recipes = _recipeService.GetAllForUser(friendProfile.UserId);

                foreach (Recipe recipe in recipes)
                {
                    if (!friendRecipess.Contains(recipe))
                    {
                        friendRecipess.Add(recipe);
                    }
                }
            }

            return View(friendRecipess);
        }


        private void CheckViewAuthorization(int recipeId)
        {
            int currentUser = _userService.GetCurrentUserId();
            Recipe recipe = _recipeService.Get(recipeId);

            if (!recipe.CanView(currentUser))
            {
                throw new UnauthorizedAccessException("Cannot view this data.");
            }
        }


        private void CheckEditAuthorization(int recipeId)
        {
            int currentUser = _userService.GetCurrentUserId();
            Recipe recipe = _recipeService.Get(recipeId);

            if (!recipe.CanEdit(currentUser))
            {
                throw new UnauthorizedAccessException("Cannot edit data you do not own.");
            }
            else
            {
                ViewBag.CanEdit = true;
            }
        }

    }
}

