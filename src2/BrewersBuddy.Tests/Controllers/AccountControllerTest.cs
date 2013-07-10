using System.Linq;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using WebMatrix.WebData;
using NUnit.Framework;

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
        public void RegisterUser_TEST()
        {
            // Act
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";

            context.UserProfiles.Add(userProfile);
            context.SaveChanges();

            var user = context.UserProfiles.FirstOrDefault(item => item.UserName == "NUNIT_Test");
            Assert.AreEqual("NUNIT_Test", user.UserName);

            context.UserProfiles.Remove(userProfile);
            context.SaveChanges();
        }

        [Test]
        public void UserCanEditAccountInformation_TEST()
        {
            // Act
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";

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
       public void TestRemoveAccount()
       {
           // Act
           UserProfile userProfile = new UserProfile();
           userProfile.UserName = "NUNIT_Test";
           userProfile.Email = "NUNIT@Test.com";
           userProfile.FirstName = "Nunit";
           userProfile.LastName = "Test";
           userProfile.City = "Brewery";
           userProfile.State = "KS";
           userProfile.Zip = "12345";

           context.UserProfiles.Add(userProfile);
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
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.Zip = "12345";
            context.UserProfiles.Add(userProfile);

            UserProfile userProfile2 = new UserProfile();
            userProfile2.UserName = "NUNIT2_Test";
            userProfile2.Email = "NUNIT2@Test.com";
            userProfile2.FirstName = "Nunit2";
            userProfile2.Zip = "12345";
            context.UserProfiles.Add(userProfile2);

            UserProfile userProfile3 = new UserProfile();
            userProfile3.UserName = "NUNIT3_Test";
            userProfile3.Email = "NUNIT3@Test.com";
            userProfile3.FirstName = "Nunit3";
            userProfile3.Zip = "12345";
            context.UserProfiles.Add(userProfile3);

            context.SaveChanges();

            var tmp = context.UserProfiles.Where(item => item.Zip == "12345").ToList();

            Assert.AreEqual(3, tmp.Count);

            foreach (UserProfile UP in context.UserProfiles)
            {
                context.UserProfiles.Remove(UP);
            }
            context.SaveChanges();
        }

    }
}
