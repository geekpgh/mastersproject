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
        public void TestCreateWithAnonymousUserWillThrowUnauthorized()
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

            ActionResult result = controller.Create(new BatchRating());

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestPreviousRatingWillReturnNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();

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
        public void TestNonExistingBatchRetuns500Error()
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
        public void ValidInputReturnsJson()
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
            Assert.IsInstanceOf<JsonResult>(result);

            JsonResult json = result as JsonResult;
            BatchRating rating = json.Data as BatchRating;

            Assert.AreEqual(1, rating.UserId);
            Assert.AreEqual(1, rating.BatchId);
            Assert.AreEqual(95, rating.Rating);
            Assert.AreEqual("My comment", rating.Comment);
        }
    }
}
