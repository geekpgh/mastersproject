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
    }
}
