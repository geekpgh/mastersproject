using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;
using System;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class BatchCommentControllerTest
    {
        [Test]
        public void TestCreateWithAnonymousUserWillThrowUnauthorized()
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

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestNonExistingBatchRetunsNotFoundResult()
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

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
            Assert.AreEqual(404, ((HttpNotFoundResult)result).StatusCode);
        }

        [Test]
        public void ValidInputReturnsJson()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(new Batch()
            {
                OwnerId = 1
            });

            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(new BatchComment()
            {
                BatchId = 1,
                Comment = "My comment"
            });

            Assert.IsNotNull(result);

            JsonResult json = result as JsonResult;

            Assert.IsInstanceOf<BatchComment>(json.Data);
            Assert.AreEqual(1, ((BatchComment)json.Data).BatchId);
            Assert.AreEqual("My comment", ((BatchComment)json.Data).Comment);
        }

        [Test]
        public void TestNullRatingServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchCommentController(null, batchService, userService)
                );
        }

        [Test]
        public void TestNullBatchServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var commentService   = Substitute.For<IBatchCommentService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchCommentController(commentService, null, userService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var batchService = Substitute.For<IBatchService>();
            var commentService = Substitute.For<IBatchCommentService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchCommentController(commentService, batchService, null)
                );
        }

        [Test]
        public void TestCreateUserCantViewBatchReturnsHttpUnauthorized()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(new Batch()
            {
                OwnerId = 2
            });

            var commentService = Substitute.For<IBatchCommentService>();

            BatchCommentController controller = new BatchCommentController(commentService, batchService, userService);

            ActionResult result = controller.Create(new BatchComment()
            {
                BatchId = 1,
                Comment = "my comment"
            });

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }
    }
}
