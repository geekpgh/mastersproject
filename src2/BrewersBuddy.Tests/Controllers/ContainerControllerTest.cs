using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using System.Web.Mvc;
using System;
using BrewersBuddy.Tests.TestUtilities;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class ContainerControllerTest : DbTestBase
    {
        [Test]
        public void TestNullBatchServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var containerService = Substitute.For<IContainerService>();

            Assert.Throws<ArgumentNullException>(() =>
                new ContainerController(null, containerService, userService)
                );
        }

        [Test]
        public void TestNullContainerServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new ContainerController(batchService, null, userService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var batchService = Substitute.For<IBatchService>();
            var containerService = Substitute.For<IContainerService>();

            Assert.Throws<ArgumentNullException>(() =>
                new ContainerController(batchService, containerService, null)
                );
        }

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
        public void TestCreateContainerModelInvalid()
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
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(container);

            Assert.IsInstanceOf<ViewResult>(result);
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

            ActionResult result = controller.Create(999);

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

            Container container = new Container();
            container.Name = "Test Container";
            containerService.Get(999).Returns(container);

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            ActionResult result = controller.Details(999);
            ViewResult view = result as ViewResult;
            Container returnedContainer = (Container)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Container>(view.Model);
            Assert.AreEqual(container.Name, returnedContainer.Name);
        }

       [Test]
        public void TestContainerDeleteConfirmed()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var containerService = Substitute.For<IContainerService>();

            ContainerController controller = new ContainerController(batchService, containerService, userService);

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);
            containerService.Get(1).Returns(container);

            ActionResult result = controller.DeleteConfirmed(container.ContainerId);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

       [Test]
       public void TestContainerUnauthorizedAccessException()
       {
           // Set up the controller
           var userService = Substitute.For<IUserService>();
           userService.GetCurrentUserId().Returns(999);

           var batchService = Substitute.For<IBatchService>();
           var containerService = Substitute.For<IContainerService>();

           ContainerController controller = new ContainerController(batchService, containerService, userService);

           UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
           Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
           Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);
           containerService.Get(1).Returns(container);

           var resultIs = ""; 
           try
           {
               ActionResult result = controller.DeleteConfirmed(container.ContainerId);
           }
           catch (UnauthorizedAccessException e)
           {
               resultIs = "UnauthorizedAccessException";
           }
           Assert.AreEqual("UnauthorizedAccessException", resultIs);
       }

       [Test]
       public void TestContainerDelete()
       {
           // Set up the controller
           var userService = Substitute.For<IUserService>();
           userService.GetCurrentUserId().Returns(1);

           var batchService = Substitute.For<IBatchService>();
           var containerService = Substitute.For<IContainerService>();

           ContainerController controller = new ContainerController(batchService, containerService, userService);

           UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
           Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
           Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);
           containerService.Get(1).Returns(container);

           ActionResult result = controller.Delete(container.ContainerId);
           ViewResult view = result as ViewResult;
           Container returnedContainer = (Container)view.Model;

           Assert.NotNull(view.Model);
           Assert.IsInstanceOf<Container>(view.Model);
           Assert.AreEqual(container.Name, returnedContainer.Name);
       }

       [Test]
       public void TestContainerEditPost()
       {
           // Set up the controller
           var userService = Substitute.For<IUserService>();
           userService.GetCurrentUserId().Returns(1);

           var batchService = Substitute.For<IBatchService>();
           var containerService = Substitute.For<IContainerService>();

           ContainerController controller = new ContainerController(batchService, containerService, userService);

           UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
           Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
           Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);
           containerService.Get(1).Returns(container);

           ActionResult result = controller.Edit(container);
           RedirectToRouteResult view = result as RedirectToRouteResult;

           Assert.IsInstanceOf<RedirectToRouteResult>(result);
           Assert.NotNull(view.RouteValues);
           Assert.AreEqual("Details/1", view.RouteValues["action"]);
       }

       [Test]
       public void TestContainerEdit()
       {
           // Set up the controller
           var userService = Substitute.For<IUserService>();
           userService.GetCurrentUserId().Returns(1);

           var batchService = Substitute.For<IBatchService>();
           var containerService = Substitute.For<IContainerService>();

           ContainerController controller = new ContainerController(batchService, containerService, userService);

           UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
           Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
           Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);
           containerService.Get(1).Returns(container);

           ActionResult result = controller.Edit(container.ContainerId);
           ViewResult view = result as ViewResult;
           Container returnedContainer = (Container)view.Model;

           Assert.NotNull(view.Model);
           Assert.IsInstanceOf<Container>(view.Model);
           Assert.AreEqual(container.Name, returnedContainer.Name);
       }

       [Test]
       public void TestContainerEditModelInvalid()
       {
           // Set up the controller
           var userService = Substitute.For<IUserService>();
           userService.GetCurrentUserId().Returns(1);

           var batchService = Substitute.For<IBatchService>();
           var containerService = Substitute.For<IContainerService>();

           ContainerController controller = new ContainerController(batchService, containerService, userService);

           UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
           Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
           Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);
           containerService.Get(1).Returns(container);
           controller.ModelState.AddModelError("key", "not valid");

           ActionResult result = controller.Edit(container);

           Assert.IsInstanceOf<ViewResult>(result);
       }


    }
}
