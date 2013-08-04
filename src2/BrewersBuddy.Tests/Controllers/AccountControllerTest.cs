using System.Linq;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using WebMatrix.WebData;
using NUnit.Framework;
using NSubstitute;
using BrewersBuddy.Services;
using System.Collections.Generic;
using BrewersBuddy.Tests.TestUtilities;
using System;

namespace BrewersBuddy.Tests.Controllers
{
	[TestFixture]
	public class AccountControllerTest : DbTestBase
	{
		[TestFixtureSetUp]
		public static void Initialize()
		{
			//WebSecurity.InitializeDatabaseConnection(
			//      connectionStringName: "DefaultConnection",
			//      userTableName: "UserProfile",
			//      userIdColumn: "UserID",
			//      userNameColumn: "UserName",
			//      autoCreateTables: true);
		}

		[Test]
		public void UserLoginModelInvalid_TEST()
		{
			// Set up the controller
			var userService = Substitute.For<IUserService>();
			userService.GetCurrentUserId().Returns(1);

			AccountController controller = new AccountController(userService);

			LoginModel login = new LoginModel();
			login.UserName = "NUNIT_Test";
			login.Password = "12345";
			login.RememberMe = false;

			ActionResult result = controller.Login(login, "");

			Assert.IsInstanceOf<ViewResult>(result);
			Assert.AreEqual("NUNIT_Test", ((LoginModel)((ViewResult)result).Model).UserName);
		}

        [Test]
        public void TestCreateBatchPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Create(batch);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.IsNotNull(batch.StartDate);
            Assert.AreEqual(batch.OwnerId, 999);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

		[Test]
		public void RegisterUser_TEST()
		{
			// Set up the controller
			var userService = Substitute.For<IUserService>();
			userService.GetCurrentUserId().Returns(1);

			AccountController controller = new AccountController(userService);

			RegisterModel regMod = new RegisterModel();
			regMod.UserName = "NUNIT_Test";
			regMod.Email = "NUNIT@Test.com";
			regMod.FirstName = "Nunit";
			regMod.LastName = "Test";
			regMod.City = "Brewery";
			regMod.State = "KS";
			regMod.Zip = "12345";

			ActionResult result = controller.Register(regMod);

			Assert.IsInstanceOf<ViewResult>(result);
			Assert.AreEqual("NUNIT_Test", ((RegisterModel)((ViewResult)result).Model).UserName);
		}

		[Test]
		public void UserCanEditAccountInformation_TEST()
		{
			// Act
            UserProfile userProfile = TestUtils.createUser(context, "Bob", "Smith");
			userProfile.UserName = "NUNIT_Test";

			context.UserProfiles.Add(userProfile);
			context.SaveChanges();

			UserProfile user = context.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
			user.FirstName = "Test";
			user.LastName = "Nunit";
			context.Entry(userProfile).State = System.Data.EntityState.Modified;
			context.SaveChanges();

			UserProfile user1 = context.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
			Assert.AreEqual("Nunit", user1.LastName);
			Assert.AreEqual("Test", user1.FirstName);
			context.UserProfiles.Remove(userProfile);
			context.SaveChanges();
		}

        [Test]
        public void TestUserCanEditAccountInformation()
        {
            // Act
            var userService = Substitute.For<IUserService>();
            UserProfile userProfile = TestUtils.createUser(context, "Bob", "Smith");
            AccountController controller = new AccountController(userService);

            // Verify user was created
            Assert.AreEqual("Bob", context.UserProfiles.Find(userProfile.UserId).FirstName);

            userProfile.FirstName = "Fred";
            ActionResult result = controller.Edit(userProfile);

            // Verify user has changed
            Assert.AreEqual("Fred", ((UserProfile)((ViewResult)result).Model).FirstName);
        }

        [Test]
		public void TestRemoveAccount()
		{
			// Act
            UserProfile userProfile = TestUtils.createUser(context, "Bob", "Smith");
            userProfile.UserName = "NUNIT_Test";
            context.SaveChanges();

			UserProfile user = context.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
			Assert.AreNotEqual(user, null);

			context.UserProfiles.Remove(user);
			context.SaveChanges();

			UserProfile user1 = context.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
			Assert.AreEqual(user1, null);
		}

		[Test]
		public void UserCanEnterZipToFindBrewers_TEST()
		{
			// Act
            UserProfile userProfile = TestUtils.createUser(context, "Bob", "Smith");
            userProfile.Zip = "12345";

            UserProfile userProfile2 = TestUtils.createUser(context, "Bob", "Smith");
            userProfile2.Zip = "12345";

            UserProfile userProfile3 = TestUtils.createUser(context, "Bob", "Smith");
            userProfile3.Zip = "12345";

			context.SaveChanges();

			var tmp = context.UserProfiles.Where(item => item.Zip == "12345").ToList();

			Assert.AreEqual(3, tmp.Count);

			foreach (UserProfile UP in context.UserProfiles)
			{
				context.UserProfiles.Remove(UP);
			}
			context.SaveChanges();
		}

        [Test]
        public void TestAccountRegister()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            AccountController controller = new AccountController(userService);

            ActionResult result = controller.Register();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestAccountIndex()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            AccountController controller = new AccountController(userService);

            ActionResult result = controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestAccountLogin()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            AccountController controller = new AccountController(userService);
            var url = "Test URL";

            ActionResult result = controller.Login(url);

            Assert.IsInstanceOf<ViewResult>(result);
            Assert.AreEqual(url, (((ViewResult)result).ViewBag).ReturnUrl);
        }

        [Test]
        public void TestAccountExternalLoginFailure()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            AccountController controller = new AccountController(userService);

            ActionResult result = controller.ExternalLoginFailure();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();

            Assert.Throws<ArgumentNullException>(() =>
                new AccountController(null)
                );
        }



	}
}
