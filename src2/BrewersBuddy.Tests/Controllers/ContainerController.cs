using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Web.Mvc;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class ContainerControllerTest
    {
        [Test]
        public void TestContainerList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.GetAllForUser(1).Returns(
                new Batch[] {
                    new Batch(),
                });

            var containerService = Substitute.For<IContainerService>();
            containerService.GetAllForUser(1).Returns(
                new Container[] {
                    new Container(),
                    new Container(),
                    new Container(),
                    new Container(),
                    new Container()
                });

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ViewResult result = (ViewResult)controller.Index();
            ViewDataDictionary data = result.ViewData;

            IList containersList = result.ViewData.Model as IList;

            Assert.IsTrue(containersList.Count == 5);
        }

        [Test]
        public void TestContainerOnlyOwnedList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            var containerService = Substitute.For<IContainerService>();
            containerService.GetAllForUser(1).Returns(
                new Container[] {
                    new Container() { Name = "Container 1" },
                    new Container() { Name = "Container 2" },
                    new Container() { Name = "Container 3" },
                    new Container() { Name = "Container 4" },
                    new Container() { Name = "Container 5" }
                });
            containerService.GetAllForUser(2).Returns(
                new Container[] {
                    new Container()
                });
            containerService.GetAllForUser(3).Returns(
                new Container[] {
                    new Container(),
                    new Container()
                });

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ViewResult result;
            ViewDataDictionary data;
            IList containerList;

            // Check for user 1
            userService.GetCurrentUserId().Returns(1);

            result = (ViewResult)controller.Index();
            data = result.ViewData;
            containerList = result.ViewData.Model as IList;

            Assert.IsTrue(containerList.Count == 5);

            // Check for user 2
            userService.GetCurrentUserId().Returns(2);

            result = (ViewResult)controller.Index();
            data = result.ViewData;
            containerList = result.ViewData.Model as IList;

            Assert.IsTrue(containerList.Count == 1);

            // Check for user 3
            userService.GetCurrentUserId().Returns(3);

            result = (ViewResult)controller.Index();
            data = result.ViewData;
            containerList = result.ViewData.Model as IList;

            Assert.IsTrue(containerList.Count == 2);
        }
    }
}
