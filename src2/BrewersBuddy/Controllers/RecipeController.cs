using System.Data;
using System.Linq;
using System.Web.Mvc;
using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Web.Security;
using WebMatrix.WebData;

namespace BrewersBuddy.Controllers
{
    public class RecipeController : Controller
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();



        //
        // GET: /Recipe/


        public ActionResult Index()
        {
            int currentUserId = ControllerUtils.getCurrentUserId(User);

            //Get only the Recipes for the current user
            var owndedRecipes = from Recipe in db.Recipes
                                where (Recipe.OwnerId.Equals(currentUserId))
                                select Recipe;

            return View(owndedRecipes.ToList());
        }


        //
        // GET: /Recipe/Details/5


        public ActionResult Details(int id = 0)
        {
            Recipe Recipe = db.Recipes.Find(id);
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
 ;

            return View();
        }


        //
        // POST: /Recipe/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recipe Recipe)
        {
            if (ModelState.IsValid)
            {
                //Set the start date to now
                Recipe.AddDate = DateTime.Now;
                //Tie the object to the user
                Recipe.OwnerId = ControllerUtils.getCurrentUserId(User);

                db.Recipes.Add(Recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(Recipe);
        }


      

        //
        // GET: /Recipe/Delete/5


        public ActionResult Delete(int id = 0)
        {
            Recipe Recipe = db.Recipes.Find(id);
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
            Recipe Recipe = db.Recipes.Find(id);
            db.Recipes.Remove(Recipe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}

