using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly BrewersBuddyContext db;

        public RecipeService(BrewersBuddyContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            db = context;
        }

        public void Create(Recipe @object)
        {
            db.Recipes.Add(@object);
            db.SaveChanges();
        }

        public void Delete(Recipe @object)
        {
            db.Recipes.Remove(@object);
            db.SaveChanges();
        }

        public Recipe Get(int id)
        {
            return db.Recipes.Find(id);
        }

        public IEnumerable<Recipe> GetAllForUser(int userId)
        {
            return from recipe in db.Recipes
                   where (recipe.OwnerId == userId)
                   select recipe;
        }

        public void Update(Recipe @object)
        {
            db.Entry(@object).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}