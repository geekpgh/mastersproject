using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Web.Mvc;
using System;

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

        [Test]
        public void TestCreateWithAnonymousUserWillThrowUnauthorizedPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ActionResult result = controller.Create(new Container());

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullBatchWillReturn500ErrorPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();

            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ActionResult result = controller.Create(new Container());

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestCreateContainerPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch testBatch = new Batch();
            batchService.Get(999).Returns(testBatch);

            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            Container container = new Container();
            container.BatchId = 999;
            ActionResult result = controller.Create(container);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(3, ((RedirectToRouteResult)result).RouteValues.Values.Count);
        }

        [Test]
        public void TestCreateContainer()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch testBatch = new Batch();
            batchService.Get(999).Returns(testBatch);

            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            Container container = new Container();
            container.BatchId = 999;
            ActionResult result = controller.Create(999);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestCreateWithAnonymousUserWillThrowUnauthorized()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            Batch testBatch = new Batch();
            batchService.Get(999).Returns(testBatch);

            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            Container container = new Container();
            container.BatchId = 999;
            ActionResult result = controller.Create(999);

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullBatchWillReturn500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();

            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ActionResult result = controller.Create(new Container());

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestDetailsWithNonExistingContainerReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ActionResult result = controller.Details(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestContainerDetails()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var containerService = Substitute.For<IContainerService>();
            containerService.Get(999).Returns(new Container());

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ActionResult result = controller.Details(999);

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}
