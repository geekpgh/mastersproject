using System;
using System.Collections;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using BrewersBuddy.Tests.TestUtilities;
using NSubstitute;
using NUnit.Framework;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class BatchActionControllerTest : DbTestBase
    {
       
        [Test]
        public void TestNullBatchActionServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchActionController(null, batchService, userService)
                );
        }

        [Test]
        public void TestNullBatchServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchActionController(batchActionService, null, userService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchActionController(batchActionService, batchService, null)
                );
        }

        [Test]
        public void TestBatchActionList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();

            var batchService = Substitute.For<IBatchService>();
            batchService.GetAllForUser(1).Returns(
                new Batch[] {
                    new Batch(),
                });

            var batchActionService = Substitute.For<IBatchActionService>();
            batchActionService.GetAllForBatch(1).Returns(
                new BatchAction[] {
                    new BatchAction(),
                    new BatchAction(),
                    new BatchAction(),
                    new BatchAction(),
                    new BatchAction()
                });

            BatchActionController action = new BatchActionController(batchActionService, batchService, userService);

            ViewResult result = (ViewResult)action.Index(1);
            ViewDataDictionary data = result.ViewData;

            IList batchActionList = result.ViewData.Model as IList;

            Assert.IsTrue(batchActionList.Count == 5);
        }
 
        [Test]
        public void TestCreateBatchAction()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            BatchAction action = new BatchAction();
            action.BatchId = 999;
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
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Create(999);

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullBatchActionWillReturn500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Create(999);

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestCreateWithAnonymousUserWillThrowUnauthorizedPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Create(new BatchAction());

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullBatchWillReturn500ErrorPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Create(new BatchAction());

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestCreateBatchActionPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            BatchAction action = new BatchAction();
            action.BatchId = 999;
            ActionResult result = controller.Create(action);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(3, ((RedirectToRouteResult)result).RouteValues.Values.Count);
        }

        [Test]
        public void TestCreateBatchActionModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            BatchAction action = new BatchAction();
            action.BatchId = 999;
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(action);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestBatchActionDetails()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
            
            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Details(1);
            ViewResult view = result as ViewResult;
            BatchAction returnedAction = (BatchAction)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<BatchAction>(view.Model);
            Assert.AreEqual(action.Title, returnedAction.Title);
        }

        [Test]
        public void TestBatchActionUserCannotView()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            var resultIs = "";
            try
            {
                ActionResult result = controller.Details(1);
            }
            catch (UnauthorizedAccessException e)
            {
                resultIs = "UnauthorizedAccessException";
            }
            Assert.AreEqual("UnauthorizedAccessException", resultIs);
        }

        [Test]
        public void TestDetailsWithNonExistingActionReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Details(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestBatchActionEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Edit(1);
            ViewResult view = result as ViewResult;
            BatchAction returnedAction = (BatchAction)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<BatchAction>(view.Model);
            Assert.AreEqual(action.Title, returnedAction.Title);
        }

        [Test]
        public void TestBatchActionUserCannotEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            var resultIs = "";
            try
            {
                ActionResult result = controller.Edit(1);
            }
            catch (UnauthorizedAccessException e)
            {
                resultIs = "UnauthorizedAccessException";
            }
            Assert.AreEqual("UnauthorizedAccessException", resultIs);
        }

        [Test]
        public void TestEditWithNonExistingActionReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Edit(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestBatchActionEditPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Edit(action);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }

        [Test]
        public void TestBatchActionEditModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Edit(action);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestBatchActionDelete()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Delete(1);
            ViewResult view = result as ViewResult;
            BatchAction returnedAction = (BatchAction)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<BatchAction>(view.Model);
            Assert.AreEqual(action.Title, returnedAction.Title);
        }

        [Test]
        public void TestDeleteWithNonExistingActionReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.Delete(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestBatchActionDeleteConfirmed()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var batchActionService = Substitute.For<IBatchActionService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);
            batchActionService.Get(1).Returns(action);

            BatchActionController controller = new BatchActionController(batchActionService, batchService, userService);

            ActionResult result = controller.DeleteConfirmed(1);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }
    }
}
