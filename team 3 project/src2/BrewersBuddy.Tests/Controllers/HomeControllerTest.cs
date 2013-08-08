using System.Linq;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using WebMatrix.WebData;
using NUnit.Framework;
using NSubstitute;
using BrewersBuddy.Services;
using System.Collections.Generic;
using System;
using System.Security.Principal;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        [Test]
        public void TestIndexDoesNotRequireAuthentication()
        {
            Type type = typeof(HomeController);
            Attribute[] classAttributes = Attribute.GetCustomAttributes(type, typeof(AuthorizeAttribute));

            Assert.AreEqual(0, classAttributes.Length);

            object[] methodAttributes = type.GetMethod("Index")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.AreEqual(0, methodAttributes.Length);
        }

        [Test]
        public void TestRedirectToBatchIndexForAuthenticatedUser()
        {
            var identity = Substitute.For<IIdentity>();
            identity.Name.Returns("user1");

            var principal = Substitute.For<IPrincipal>();
            principal.Identity.Returns(identity);

            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUser().Returns(principal);

            HomeController controller = new HomeController(userService);

            RedirectToRouteResult result = controller.Index() as RedirectToRouteResult;

            Assert.NotNull(result);
            Assert.AreEqual("Batch", result.RouteValues["Controller"]);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
    }
}
