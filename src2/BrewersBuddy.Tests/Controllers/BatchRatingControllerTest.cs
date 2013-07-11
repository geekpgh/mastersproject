using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using NSubstitute;
using NUnit.Framework;
using System.Web.Mvc;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class BatchRatingControllerTest
    {
        [Test]
        public void TestGetCreateWithAnonymousUserWillThrowUnauthorized()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var ratingService = Substitute.For<IBatchRatingService>();
            var batchService = Substitute.For<IBatchService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(1);

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestPostCreateWithAnonymousUserWillThrowUnauthorized()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var ratingService = Substitute.For<IBatchRatingService>();
            var batchService = Substitute.For<IBatchService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(new BatchRating());

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestInvalidModelStateWillReturn500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(new BatchRating()
            {
                Comment = "Test comment"
            });

            ViewResult view = result as ViewResult;

            Assert.IsInstanceOf<BatchRating>(view.Model);
            Assert.AreEqual("Test comment", ((BatchRating)view.Model).Comment);
        }

        [Test]
        public void TestPostPreviousRatingWillReturnNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(new Batch());

            var ratingService = Substitute.For<IBatchRatingService>();
            ratingService.GetUserRatingForBatch(1, 1).Returns(new BatchRating());

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(new BatchRating()
            {
                BatchId = 1
            });

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestGetPreviousRatingWillReturnNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(new Batch());

            var ratingService = Substitute.For<IBatchRatingService>();
            ratingService.GetUserRatingForBatch(1, 1).Returns(new BatchRating());

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(1);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestPostNonExistingBatchRetuns500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.Get(1).Returns(null, null);

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(new BatchRating()
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

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

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

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(new BatchRating()
            {
                BatchId = 1,
                Rating = 95,
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

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchRatingController controller = new BatchRatingController(ratingService, batchService, userService);

            ActionResult result = controller.Create(1);

            ViewResult view = result as ViewResult;

            Assert.NotNull(view.ViewBag.Ratings);
            Assert.AreEqual(1, view.ViewBag.BatchId);
            Assert.NotNull("Batch name", view.ViewBag.BatchName);
        }
    }
}
