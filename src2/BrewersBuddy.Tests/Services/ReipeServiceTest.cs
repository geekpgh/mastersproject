using System;
using NUnit.Framework;
using BrewersBuddy.Services;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using System.Collections.Generic;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class ReipeServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            RecipeService recipeService = new RecipeService(context);
            Recipe recipe = new Recipe();
            recipe.RecipeId = 1;
            recipe.Costs = "$200";
            recipe.Prep="Do stuff";
            recipe.Process= "Do more stuff";
            recipe.Finishing = "Finish doing stuff";
            recipe.Description = "test";
            recipe.Name = "Test Recipe";
            recipe.AddDate = DateTime.Now;
            recipe.OwnerId = peter.UserId;

            recipeService.Create(recipe);

            Recipe foundRecipe = context.Recipes.Find(recipe.RecipeId);

            Assert.IsNotNull(foundRecipe);
            Assert.AreEqual(recipe.RecipeId, foundRecipe.RecipeId);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Recipe recipe = TestUtils.createRecipe(context, "Test Recipe", peter);

            //Now change it
            recipe.Name = "Altered Recipe";
            RecipeService recipeService = new RecipeService(context);
            recipeService.Update(recipe);

            //Get it  and see it changed
            Recipe alteredRecipe = context.Recipes.Find(recipe.RecipeId);
            Assert.AreEqual("Altered Recipe", alteredRecipe.Name);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Recipe recipe = TestUtils.createRecipe(context, "Test Recipe", bilbo);

            RecipeService recipeService = new RecipeService(context);
            Recipe foundRecipe = recipeService.Get(recipe.RecipeId);

            Assert.IsNotNull(foundRecipe);
            Assert.AreEqual(recipe.RecipeId, foundRecipe.RecipeId);
            Assert.AreEqual(recipe.Name, foundRecipe.Name);
        }


        [Test]
        public void TestGetNonExistant()
        {
            RecipeService recipeService = new RecipeService(context);
            Recipe foundRecipe = recipeService.Get(5);

            Assert.IsNull(foundRecipe);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Recipe recipe = TestUtils.createRecipe(context, "Test Recipe", bilbo);

            //See that the service can find it
            RecipeService recipeService = new RecipeService(context);
            Recipe foundRecipe = recipeService.Get(recipe.RecipeId);

            Assert.IsNotNull(foundRecipe);
            Assert.AreEqual(recipe.RecipeId, foundRecipe.RecipeId);
            Assert.AreEqual(recipe.Name, foundRecipe.Name);

            //Now delete it and see that it is gone
            recipeService.Delete(foundRecipe);

            Recipe foundRecipeDelete = recipeService.Get(foundRecipe.RecipeId);
            Assert.IsNull(foundRecipeDelete);
        }

 
        [Test]
        public void TestGetAllForUser()
        {
            UserProfile gandalf = TestUtils.createUser(context, "Gandalf", "TheGrey");
            UserProfile sauron = TestUtils.createUser(context, "Sauron", "EvilOne");
            Recipe recipe1 = TestUtils.createRecipe(context, "Test Recipe", gandalf);
            Recipe recipe2 = TestUtils.createRecipe(context, "Test Recipe", gandalf);

            Recipe recipe3 = TestUtils.createRecipe(context, "Test Recipe", sauron);

            RecipeService recipeService = new RecipeService(context);
            IEnumerable<Recipe> recipeesEnumerable = recipeService.GetAllForUser(gandalf.UserId);

            int foundCount = 0;
            foreach (Recipe foundRecipe in recipeesEnumerable)
            {
                if (foundRecipe.RecipeId == recipe3.RecipeId)
                {
                    Assert.Fail("Recipe found for wrong user");
                }

                if (foundRecipe.RecipeId == recipe1.RecipeId || foundRecipe.RecipeId == recipe2.RecipeId)
                {
                    foundCount++;
                }
            }

            Assert.AreEqual(2, foundCount);
        }
    }
}
