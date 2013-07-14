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
    public class BatchControllerTest
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

            var noteService = Substitute.For<IBatchNoteService>();
            var ratingService = Substitute.For<IBatchRatingService>();

            BatchController controller = new BatchController(batchService, noteService, ratingService, userService);

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

            BatchController controller = new BatchController(batchService, noteService, ratingService, userService);

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

       
    }
}
