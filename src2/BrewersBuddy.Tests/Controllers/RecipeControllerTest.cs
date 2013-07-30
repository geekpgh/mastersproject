using System;
using System.Collections;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using BrewersBuddy.Tests.TestUtilities;
using NSubstitute;
using NUnit.Framework;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class RecipeControllerTest : DbTestBase
    {
        [Test]
        public void TestNullRecipeServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();

            Assert.Throws<ArgumentNullException>(() =>
                new RecipeController(null, userService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var RecipeService = Substitute.For<IRecipeService>();

            Assert.Throws<ArgumentNullException>(() =>
                new RecipeController(RecipeService, null)
                );
        }

        [Test]
        public void TestCreateRecipe()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var recipeService = Substitute.For<IRecipeService>();

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestCreateRecipePost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var recipeService = Substitute.For<IRecipeService>();
            Recipe recipe = new Recipe();

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Create(recipe);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.IsNotNull(recipe.AddDate);
            Assert.AreEqual(recipe.OwnerId, 999);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [Test]
        public void TestCreateRecipeModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var recipeService = Substitute.For<IRecipeService>();
            Recipe recipe = new Recipe();

            RecipeController controller = new RecipeController(recipeService, userService);

            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(recipe);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestRecipeDetails()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Details(1);
            ViewResult view = result as ViewResult;
            Recipe returnedrecipe = (Recipe)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Recipe>(view.Model);
            Assert.AreEqual(recipe.Name, returnedrecipe.Name);
        }

        [Test]
        public void TestDetailsWithNonExistingRecipeReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var recipeService = Substitute.For<IRecipeService>();

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Details(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestRecipeUserCannotView()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);

            var resultIs = "";
            try
            {
                ActionResult result = controller.Details(1);
            }
            catch (UnauthorizedAccessException e)
            {
                resultIs = "UnauthorizedAccessException";
            }
            Assert.AreEqual("UnauthorizedAccessException", resultIs);
        }

        [Test]
        public void TestRecipeEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);
            recipeService.GetAllForUser(1).Returns(
                new Recipe[] {
                    recipe,
                });

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Edit(1);
            ViewResult view = result as ViewResult;
            Recipe returnedrecipe = (Recipe)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Recipe>(view.Model);
            Assert.AreEqual(recipe.Name, returnedrecipe.Name);
        }

        [Test]
        public void TestRecipeNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(recipe.RecipeId).Returns(recipe);
            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Edit(recipe.RecipeId);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestRecipeUserCannotEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);

            var resultIs = "";
            try
            {
                ActionResult result = controller.Edit(1);
            }
            catch (UnauthorizedAccessException e)
            {
                resultIs = "UnauthorizedAccessException";
            }
            Assert.AreEqual("UnauthorizedAccessException", resultIs);
        }
        [Test]
        public void TestRecipeEditPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Edit(recipe);
            ViewResult view = result as ViewResult;
            Recipe returnedrecipe = (Recipe)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Recipe>(view.Model);
            Assert.AreEqual(recipe.Name, returnedrecipe.Name);
        }

        [Test]
        public void TestRecipeEditModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Edit(recipe);
            ViewResult view = result as ViewResult;
            Recipe returnedrecipe = (Recipe)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Recipe>(view.Model);
            Assert.AreEqual(recipe.Name, returnedrecipe.Name);
        }

        [Test]
        public void TestRecipeDelete()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Delete(1);
            ViewResult view = result as ViewResult;
            Recipe returnedrecipe = (Recipe)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Recipe>(view.Model);
            Assert.AreEqual(recipe.Name, returnedrecipe.Name);
        }

        [Test]
        public void TestDeleteWithNonExistingRecipeReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var recipeService = Substitute.For<IRecipeService>();

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.Delete(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestDeleteRecipeConfirmed()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Recipe recipe = TestUtils.createRecipe(context, "Awesome Recipe", bob);
            recipeService.Get(1).Returns(recipe);

            RecipeController controller = new RecipeController(recipeService, userService);

            ActionResult result = controller.DeleteConfirmed(1);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [Test]
        public void TestRecipeList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var recipeService = Substitute.For<IRecipeService>();
            recipeService.GetAllForUser(1).Returns(
                new Recipe[] {
                    new Recipe(),
                    new Recipe(),
                    new Recipe(),
                    new Recipe(),
                    new Recipe()
                });

            RecipeController controller = new RecipeController(recipeService, userService);

            ViewResult result = (ViewResult)controller.Index();
            ViewDataDictionary data = result.ViewData;

            IList recipeList = result.ViewData.Model as IList;

            Assert.IsTrue(recipeList.Count == 5);
        }
    }
}
