using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class BatchCommentControllerTest
    {
        [Test]
        public void TestGetCreateWithAnonymousUserWillThrowUnauthorized()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var commentService = Substitute.For<IBatchCommentService>();
            var batchService = Substitute.For<IBatchService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(1);

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestPostCreateWithAnonymousUserWillThrowUnauthorized()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var commentService = Substitute.For<IBatchCommentService>();
            var batchService = Substitute.For<IBatchService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(new BatchComment());

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestInvalidModelStateWillReturn500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(new BatchComment()
            {
                Comment = "Test comment"
            });

            ViewResult view = result as ViewResult;

            Assert.IsInstanceOf<BatchComment>(view.Model);
            Assert.AreEqual("Test comment", ((BatchComment)view.Model).Comment);
        }

        [Test]
        public void TestPostNonExistingBatchRetuns500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(null, null);

            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(new BatchComment()
            {
                BatchId = 1
            });

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestGetNonExistingBatchRetuns500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(null, null);

            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(1);

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void ValidInputRedirectsToBatchDetails()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(new Batch());

            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(new BatchComment()
            {
                BatchId = 1,
                Comment = "My comment"
            });

            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToRouteResult>(result);

            RedirectToRouteResult redirect = result as RedirectToRouteResult;

            Assert.AreEqual("Batch", redirect.RouteValues["controller"]);
            Assert.AreEqual("Details", redirect.RouteValues["action"]);
        }

        [Test]
        public void TestViewBagPopulated()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(new Batch()
            {
                Name = "Batch name"
            });

            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(1);

            ViewResult view = result as ViewResult;

            Assert.AreEqual(1, view.ViewBag.BatchId);
            Assert.NotNull("Batch name", view.ViewBag.BatchName);
        }
    }
}
