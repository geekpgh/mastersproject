using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using NUnit.Framework;

namespace BrewersBuddy.Tests.Controllers
{
    class HelpControllerTest
    {
        [Test]
        public void TestHelpFAQ()
        {
            // Set up the controller
            HelpController controller = new HelpController();

            ActionResult result = controller.FAQ();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestHelpUsersManual()
        {
            // Set up the controller
            HelpController controller = new HelpController();

            ActionResult result = controller.Manual();

            Assert.IsInstanceOf<ViewResult>(result);
        }


    }
}
