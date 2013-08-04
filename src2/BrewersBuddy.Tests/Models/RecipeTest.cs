using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    class RecipeTest : DbTestBase
    {
        [Test]
        public void TestCreateRecipe()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Test", bob);

            DbSet<Recipe> recipes = context.Recipes;
            Recipe foundRecipe = recipes.Find(recipe.RecipeId);

            //Verify it was properly created
            Assert.AreEqual(recipe.RecipeId, foundRecipe.RecipeId);
        }

        [Test]
        public void TestCanViewOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Test", bob);

            //Verify the owner can view
            Assert.IsTrue(recipe.CanView(bob.UserId));
        }

        [Test]
        public void TestCanEditOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Test", bob);

            //Verify the collaborator can edit
            Assert.IsTrue(recipe.CanEdit(bob.UserId));
        }


        [Test]
        public void TestCanViewFriend()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Test", bob);
            Friend newFriend = TestUtils.createFriend(context, fred, bob);

            //Verify the collaborator can view
            Assert.IsTrue(recipe.CanView(fred.UserId));
        }

        [Test]
        public void TestCannotEditFriend()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Test", bob);
            Friend newFriend = TestUtils.createFriend(context, fred, bob);

            //Verify the owner can view
            Assert.IsFalse(recipe.CanEdit(fred.UserId));
        }

        [Test]
        public void TestIsRecipeOwner()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Test", bob);

            Assert.AreEqual(recipe.IsOwner(bob.UserId), true);
            Assert.AreNotEqual(recipe.IsOwner(fred.UserId), true);
        }

        [Test]
        public void TestRecipeNameIsRequired()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");

            Recipe recipe = new Recipe();

            var validationContext = new ValidationContext(recipe, null, null);
            var validationResults = new List<ValidationResult>();

            Validator.TryValidateObject(recipe, validationContext, validationResults);

            foreach (var validationResult in validationResults)
            {
                if (validationResult.MemberNames.Contains("Name"))
                {
                    Assert.AreEqual("The Name field is required.", validationResult.ErrorMessage);
                    return;
                }
            }

            Assert.Inconclusive();
        }
    }
}
