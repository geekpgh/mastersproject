using BrewersBuddy.Models;
using BrewersBuddy.Services;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class UserServiceTest : DbTestBase
    {
        [Test]
        public void TestGetCurrentUserLoggedIn()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("username"),
                new string[0]
                );

            UserService userService = new UserService();

            IPrincipal currentUser = userService.GetCurrentUser();

            Assert.AreEqual("username", currentUser.Identity.Name);
        }

        [Test]
        public void TestGetCurrentUserLoggedOut()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity(""),
                new string[0]
                );

            UserService userService = new UserService();

            IPrincipal currentUser = userService.GetCurrentUser();

            Assert.AreEqual("", currentUser.Identity.Name);
        }

        [Test]
        public void TestGetCurrentUserIdUserNull()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            UserService userService = new UserService();

            int currentUserId = userService.GetCurrentUserId();

            Assert.AreEqual(0, currentUserId);
        }

        [Test]
        public void TestGetCurrentUserId()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            // User is logged in
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("testuser"),
                new string[0]
                );

            UserProfile user = TestUtils.createUser(context, "Test", "User");
            user.UserName = "testuser";
            context.SaveChanges();

            UserService userService = new UserService();

            int currentUserId = userService.GetCurrentUserId();

            Assert.AreEqual(user.UserId, currentUserId);
        }

        [Test]
        public void TestFindByUserName()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            UserProfile user1 = TestUtils.createUser(context, "Test", "User");
            user1.UserName = "testuser";
            context.SaveChanges();

            UserProfile user2 = TestUtils.createUser(context, "Test", "User1");
            user2.UserName = "testuser1";
            context.SaveChanges();

            UserProfile user3 = TestUtils.createUser(context, "Test", "User2");
            user3.UserName = "testuser2";
            context.SaveChanges();

            UserProfile user4 = TestUtils.createUser(context, "Test", "User3");
            user4.UserName = "testuser3";
            context.SaveChanges();

            UserProfile user5 = TestUtils.createUser(context, "Test", "User4");
            user5.UserName = "testuser4";
            context.SaveChanges();

            UserService userService = new UserService();

            IEnumerable<UserProfile> users = userService.Find(new UserSearchCriteria()
            {
                UserName = "testuser"
            });

            Assert.AreEqual(1, users.Count());
            Assert.AreEqual(user1.UserId, users.First().UserId);
        }

        [Test]
        public void TestFindByFisrtName()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            UserProfile user1 = TestUtils.createUser(context, "Test", "User");
            user1.UserName = "testuser";
            context.SaveChanges();

            UserProfile user2 = TestUtils.createUser(context, "Test", "User1");
            user2.UserName = "testuser1";
            context.SaveChanges();

            UserProfile user3 = TestUtils.createUser(context, "Test", "User2");
            user3.UserName = "testuser2";
            context.SaveChanges();

            UserProfile user4 = TestUtils.createUser(context, "Test", "User3");
            user4.UserName = "testuser3";
            context.SaveChanges();

            UserProfile user5 = TestUtils.createUser(context, "Testy", "User4");
            user5.UserName = "testuser4";
            context.SaveChanges();

            UserService userService = new UserService();

            List<UserProfile> users = userService.Find(new UserSearchCriteria()
            {
                FirstName = "Test"
            }).ToList();

            Assert.AreEqual(4, users.Count);
            Assert.AreEqual(user1.UserId, users[0].UserId);
            Assert.AreEqual(user2.UserId, users[1].UserId);
            Assert.AreEqual(user3.UserId, users[2].UserId);
            Assert.AreEqual(user4.UserId, users[3].UserId);
        }

        [Test]
        public void TestFindByLastName()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            UserProfile user1 = TestUtils.createUser(context, "Test", "User");
            user1.UserName = "testuser";
            context.SaveChanges();

            UserProfile user2 = TestUtils.createUser(context, "Test", "User1");
            user2.UserName = "testuser1";
            context.SaveChanges();

            UserProfile user3 = TestUtils.createUser(context, "Test", "User2");
            user3.UserName = "testuser2";
            context.SaveChanges();

            UserProfile user4 = TestUtils.createUser(context, "Test", "User3");
            user4.UserName = "testuser3";
            context.SaveChanges();

            UserProfile user5 = TestUtils.createUser(context, "Testy", "User4");
            user5.UserName = "testuser4";
            context.SaveChanges();

            UserService userService = new UserService();

            List<UserProfile> users = userService.Find(new UserSearchCriteria()
            {
                LastName = "User3"
            }).ToList();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(user4.UserId, users[0].UserId);
        }

        [Test]
        public void TestFindByZip()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            UserProfile user1 = TestUtils.createUser(context, "Test", "User");
            user1.UserName = "testuser";
            user1.Zip = "27540";
            context.SaveChanges();

            UserProfile user2 = TestUtils.createUser(context, "Test", "User1");
            user2.UserName = "testuser1";
            user2.Zip = "16509";
            context.SaveChanges();

            UserProfile user3 = TestUtils.createUser(context, "Test", "User2");
            user3.UserName = "testuser2";
            user3.Zip = "16506";
            context.SaveChanges();

            UserProfile user4 = TestUtils.createUser(context, "Test", "User3");
            user4.UserName = "testuser3";
            user4.Zip = "16506";
            context.SaveChanges();

            UserProfile user5 = TestUtils.createUser(context, "Testy", "User4");
            user5.UserName = "testuser4";
            user5.Zip = "25540";
            context.SaveChanges();

            UserService userService = new UserService();

            List<UserProfile> users = userService.Find(new UserSearchCriteria()
            {
                Zipcode = "16506"
            }).ToList();

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(user3.UserId, users[0].UserId);
            Assert.AreEqual(user4.UserId, users[1].UserId);
        }

        [Test]
        public void TestFindFiltersOutCurrentUser()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("testuser"),
                new string[0]
                );

            UserProfile user1 = TestUtils.createUser(context, "Test", "User");
            user1.UserName = "testuser";
            context.SaveChanges();

            UserProfile user2 = TestUtils.createUser(context, "Test", "User1");
            user2.UserName = "testuser1";
            context.SaveChanges();

            UserProfile user3 = TestUtils.createUser(context, "Test", "User2");
            user3.UserName = "testuser2";
            context.SaveChanges();

            UserProfile user4 = TestUtils.createUser(context, "Test", "User3");
            user4.UserName = "testuser3";
            context.SaveChanges();

            UserProfile user5 = TestUtils.createUser(context, "Testy", "User4");
            user5.UserName = "testuser4";
            context.SaveChanges();

            UserService userService = new UserService();

            List<UserProfile> users = userService.Find(new UserSearchCriteria()
            {
                FirstName = "Test"
            }).ToList();

            Assert.AreEqual(3, users.Count);
            Assert.AreEqual(user2.UserId, users[0].UserId);
            Assert.AreEqual(user3.UserId, users[1].UserId);
            Assert.AreEqual(user4.UserId, users[2].UserId);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");

            UserService userService = new UserService();
            UserProfile foundUser = userService.Get(bilbo.UserId);

            Assert.IsNotNull(foundUser);
            Assert.AreEqual(bilbo.UserId, foundUser.UserId);
            Assert.AreEqual(bilbo.FirstName, foundUser.FirstName);
            Assert.AreEqual(bilbo.LastName, foundUser.LastName);
        }

        [Test]
        public void TestGetNonExistant()
        {
            UserService userService = new UserService();
            UserProfile user = userService.Get(5);

            Assert.IsNull(user);
        }

        [Test]
        public void TestGetFriends()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("testuser"),
                new string[0]
                );

            // Create the user
            UserProfile user = TestUtils.createUser(context, "Test", "User");
            user.UserName = "testuser";
            context.SaveChanges();

            // Create some people to be friends with
            UserProfile friend1 = TestUtils.createUser(context, "Test", "User1");
            UserProfile friend2 = TestUtils.createUser(context, "Test", "User2");
            UserProfile friend3 = TestUtils.createUser(context, "Test", "User3");
            UserProfile friend4 = TestUtils.createUser(context, "Test", "User4");
            UserProfile friend5 = TestUtils.createUser(context, "Test", "User5");

            // Set up the associations
            TestUtils.createFriend(context, friend1, user);
            TestUtils.createFriend(context, friend2, user);
            TestUtils.createFriend(context, friend3, user);
            TestUtils.createFriend(context, friend4, user);
            TestUtils.createFriend(context, friend5, user);

            UserService userService = new UserService();

            ICollection<Friend> friends = userService.Friends(user.UserId);

            Assert.AreEqual(5, friends.Count);
            Assert.AreEqual(friend1.UserId, friends.ElementAt(0).FriendUserId);
            Assert.AreEqual(friend2.UserId, friends.ElementAt(1).FriendUserId);
            Assert.AreEqual(friend3.UserId, friends.ElementAt(2).FriendUserId);
            Assert.AreEqual(friend4.UserId, friends.ElementAt(3).FriendUserId);
            Assert.AreEqual(friend5.UserId, friends.ElementAt(4).FriendUserId);
        }

        [Test]
        public void TestGetFriendsWithNoFriends()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("testuser"),
                new string[0]
                );

            // Create the user
            UserProfile user = TestUtils.createUser(context, "Test", "User");
            user.UserName = "testuser";
            context.SaveChanges();

            UserService userService = new UserService();

            ICollection<Friend> friends = userService.Friends(user.UserId);

            Assert.AreEqual(0, friends.Count);
        }

        [Test]
        public void TestFriendProfiles()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter())
                );

            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("testuser"),
                new string[0]
                );

            // Create the user
            UserProfile user = TestUtils.createUser(context, "Test", "User");
            user.UserName = "testuser";
            context.SaveChanges();

            // Create some people to be friends with
            UserProfile friend1 = TestUtils.createUser(context, "Test", "User1");
            UserProfile friend2 = TestUtils.createUser(context, "Test", "User2");
            UserProfile friend3 = TestUtils.createUser(context, "Test", "User3");
            UserProfile friend4 = TestUtils.createUser(context, "Test", "User4");
            UserProfile friend5 = TestUtils.createUser(context, "Test", "User5");

            // Set up the associations
            TestUtils.createFriend(context, friend1, user);
            TestUtils.createFriend(context, friend2, user);
            TestUtils.createFriend(context, friend3, user);
            TestUtils.createFriend(context, friend4, user);
            TestUtils.createFriend(context, friend5, user);

            UserService userService = new UserService();

            ICollection<UserProfile> friends = userService.FriendProfiles(user.UserId);

            Assert.AreEqual(5, friends.Count);
            Assert.AreEqual(friend1.UserId, friends.ElementAt(0).UserId);
            Assert.AreEqual(friend2.UserId, friends.ElementAt(1).UserId);
            Assert.AreEqual(friend3.UserId, friends.ElementAt(2).UserId);
            Assert.AreEqual(friend4.UserId, friends.ElementAt(3).UserId);
            Assert.AreEqual(friend5.UserId, friends.ElementAt(4).UserId);
        }
    }
}
