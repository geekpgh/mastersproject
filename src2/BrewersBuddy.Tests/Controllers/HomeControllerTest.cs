using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using BrewersBuddy;
using BrewersBuddy.Controllers;
using NSubstitute;
using System.Web;
using System.Web.Routing;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest : DbTestBase
    {
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        }
    }
}
