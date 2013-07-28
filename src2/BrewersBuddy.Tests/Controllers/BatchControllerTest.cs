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
    public class BatchControllerTest : DbTestBase
    {
        [Test]
        public void TestBatchList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            batchService.GetAllForUser(1).Returns(
                new Batch[] {
                    new Batch(),
                    new Batch(),
                    new Batch(),
                    new Batch(),
                    new Batch()
                });

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ViewResult result = (ViewResult)controller.Index();
            ViewDataDictionary data = result.ViewData;

            IList batchesList = result.ViewData.Model as IList;

            Assert.IsTrue(batchesList.Count == 5);
        }

        [Test]
        public void TestBatchOnlyOwnedList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();

            var batchService = Substitute.For<IBatchService>();
            batchService.GetAllForUser(1).Returns(
                new Batch[] {
                    new Batch() { Name = "Batch 1" },
                    new Batch() { Name = "Batch 2" },
                    new Batch() { Name = "Batch 3" },
                    new Batch() { Name = "Batch 4" },
                    new Batch() { Name = "Batch 5" }
                });
            batchService.GetAllForUser(2).Returns(
                new Batch[] {
                    new Batch()
                });
            batchService.GetAllForUser(3).Returns(
                new Batch[] {
                    new Batch(),
                    new Batch()
                });

            var noteService = Substitute.For<IBatchNoteService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ViewResult result;
            ViewDataDictionary data;
            IList batchesList;

            // Check for user 1
            userService.GetCurrentUserId().Returns(1);

            result = (ViewResult)controller.Index();
            data = result.ViewData;
            batchesList = result.ViewData.Model as IList;

            Assert.IsTrue(batchesList.Count == 5);

            // Check for user 2
            userService.GetCurrentUserId().Returns(2);

            result = (ViewResult)controller.Index();
            data = result.ViewData;
            batchesList = result.ViewData.Model as IList;

            Assert.IsTrue(batchesList.Count == 1);

            // Check for user 3
            userService.GetCurrentUserId().Returns(3);

            result = (ViewResult)controller.Index();
            data = result.ViewData;
            batchesList = result.ViewData.Model as IList;

            Assert.IsTrue(batchesList.Count == 2);
        }

        [Test]
        public void TestNullBatchServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchRatingService = Substitute.For<IBatchRatingService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchController(null, batchRatingService, userService)
                );
        }

        [Test]
        public void TestNullBatchRatingServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchController(batchService, null, userService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var batchService = Substitute.For<IBatchService>();
            var batchRatingService = Substitute.For<IBatchRatingService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchController(batchService, batchRatingService, null)
                );
        }

        [Test]
        public void TestBatchCreate()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Create();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestCreateBatchPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Create(batch);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.IsNotNull(batch.StartDate);
            Assert.AreEqual(batch.OwnerId, 999);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [Test]
        public void TestCreateBatchModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();

            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, ratingService, userService);

            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(batch);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        //Requires further testing
        public void TestBatchDetailsUserRatingNAAverageRatingNA()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Details(1);
            ViewResult view = result as ViewResult;
            Batch returnedBatch = (Batch)view.Model;

            Assert.IsTrue(view.ViewBag.CanEdit);
            Assert.IsTrue(view.ViewBag.IsOwner);
            Assert.AreEqual(view.ViewBag.UserRating, "N/A");
            Assert.AreEqual(view.ViewBag.AverageRating, "N/A");

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Batch>(view.Model);
            Assert.AreEqual(batch.Name, returnedBatch.Name);
        }

        [Test]
        public void TestBatchUserCannotView()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

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
        public void TestBatchEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Edit(1);
            ViewResult view = result as ViewResult;
            Batch returnedBatch = (Batch)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Batch>(view.Model);
            Assert.AreEqual(batch.Name, returnedBatch.Name);
        }

        [Test]
        public void TestBatchUserCannotEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

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
        public void TestBatchEditPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Edit(batch);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }

        [Test]
        public void TestBatchEditModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Edit(batch);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestBatchDelete()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.Delete(1);
            ViewResult view = result as ViewResult;
            Batch returnedBatch = (Batch)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Batch>(view.Model);
            Assert.AreEqual(batch.Name, returnedBatch.Name);
        }

        [Test]
        public void TestBatchDeleteNoteConfirmed()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ActionResult result = controller.DeleteConfirmed(1);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Index", view.RouteValues["action"]);
        }

        [Test]
        public void TestBatchRatingList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batchService.Get(1).Returns(batch);

            ratingService.GetAllForBatch(1).Returns(
                new BatchRating[] {
                    new BatchRating(),
                    new BatchRating(),
                    new BatchRating(),
                    new BatchRating(),
                    new BatchRating()
                });

            BatchController controller = new BatchController(batchService, ratingService, userService);

            ViewResult result = (ViewResult)controller.Ratings(1);
            ViewDataDictionary data = result.ViewData;

            IList batchRatingList = result.ViewData.Model as IList;

            Assert.IsTrue(batchRatingList.Count == 5);
        }
    }
}
