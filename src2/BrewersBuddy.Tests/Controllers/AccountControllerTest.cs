using System.Linq;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrewersBuddy.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void RegisterUser_TEST()
        {
            // Arrange
            BrewersBuddyContext db = new BrewersBuddyContext();

            // Act
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";

            db.UserProfiles.Add(userProfile);
            db.SaveChanges();

            var user = db.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
            Assert.AreEqual("NUNIT_Test", user.UserName);

            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
        }

        [TestMethod]
        public void Login_TEST()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = "NUNIT_Test";
            loginModel.Password = "123456";
            ViewResult result = controller.Login(loginModel, "URL") as ViewResult;

            // Assert
            var membership = System.Web.Security.Membership.GetUser();
            Assert.AreEqual("Logged in username is NUNIT_Test", result.ViewBag.Message);
        }


        [TestMethod]
        public void LogOff_TEST()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            LoginModel loginModel = new LoginModel();
            loginModel.UserName = "NUNIT_Test";
            loginModel.Password = "123456";
            controller.Login(loginModel, "URL");
            ViewResult result = controller.LogOff() as ViewResult;

            // Assert
            // Find the global user for this instance and make sure the user is not NUNIT_Test
            Assert.AreEqual("NUNIT_Test is logged off", result.ViewBag.Message);
        }


        [TestMethod]
        public void RecoverPassword_TEST()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            ViewResult result = controller.RecoverPassword("NUNIT_Test") as ViewResult;

            // Assert
            Assert.AreEqual("An email with your password has been sent.", result.ViewBag.Message);
        }


        [TestMethod]
        public void UserCanEditAccountInformation_TEST()
        {
            // Arrange
            BrewersBuddyContext db = new BrewersBuddyContext();

            // Act
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";

            db.UserProfiles.Add(userProfile);
            db.SaveChanges();

            UserProfile user = db.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
            user.FirstName = "Test";
            user.LastName = "Nunit";
            db.Entry(userProfile).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            UserProfile user1 = db.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
            Assert.AreEqual("Nunit", user1.LastName);
            Assert.AreEqual("Test", user1.FirstName);
            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
        }


        [TestMethod]
        public void UserCanEnterZipToFindBrewers_TEST()
        {
            // Arrange
            BrewersBuddyContext db = new BrewersBuddyContext();

            // Act
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.Zip = "12345";
            db.UserProfiles.Add(userProfile);

            UserProfile userProfile2 = new UserProfile();
            userProfile2.UserName = "NUNIT2_Test";
            userProfile2.Email = "NUNIT2@Test.com";
            userProfile2.FirstName = "Nunit2";
            userProfile2.Zip = "12345";
            db.UserProfiles.Add(userProfile2);

            UserProfile userProfile3 = new UserProfile();
            userProfile3.UserName = "NUNIT3_Test";
            userProfile3.Email = "NUNIT3@Test.com";
            userProfile3.FirstName = "Nunit3";
            userProfile3.Zip = "12345";
            db.UserProfiles.Add(userProfile3);

            db.SaveChanges();

            var tmp = db.UserProfiles.Where(item => item.Zip == "12345").ToList();

            Assert.AreEqual(3, tmp.Count);

            foreach (UserProfile UP in db.UserProfiles)
            {
                db.UserProfiles.Remove(UP);
            }
            db.SaveChanges();
        }

    }
}
