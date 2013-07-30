using System;
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
    public class BatchNoteControllerTest : DbTestBase
    {
        [Test]
        public void TestNullBatchServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchNoteService = Substitute.For<IBatchNoteService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchNoteController(null, batchNoteService, userService)
                );
        }

        [Test]
        public void TestNullBatchNoteServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchNoteController(batchService, null, userService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var batchService = Substitute.For<IBatchService>();
            var batchNoteService = Substitute.For<IBatchNoteService>();

            Assert.Throws<ArgumentNullException>(() =>
                new BatchNoteController(batchService, batchNoteService, null)
                );
        }

        [Test]
        public void TestCreateBatchNote()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

            BatchNote note = new BatchNote();
            note.BatchId = 999;
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
            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

            ActionResult result = controller.Create(999);

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullBatchNoteWillReturn500Error()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();

            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

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
            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

            ActionResult result = controller.Create(new BatchNote());

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullBatchWillReturn500ErrorPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();

            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

            ActionResult result = controller.Create(new BatchNote());

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestCreateBatchNotePost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

            BatchNote note = new BatchNote();
            note.BatchId = 999;
            ActionResult result = controller.Create(note);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(3, ((RedirectToRouteResult)result).RouteValues.Values.Count);
        }

        [Test]
        public void TestCreateBatchNoteModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var batchNoteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, batchNoteService, userService);

            BatchNote note = new BatchNote();
            note.BatchId = 999;
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(note);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestBatchNoteDetails()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
            
            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Details(1);
            ViewResult view = result as ViewResult;
            BatchNote returnedNote = (BatchNote)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<BatchNote>(view.Model);
            Assert.AreEqual(note.Text, returnedNote.Text);
        }

        [Test]
        public void TestBatchNoteUserCannotView()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

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
        public void TestDetailsWithNonExistingNoteReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Details(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestBatchNoteEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Edit(1);
            ViewResult view = result as ViewResult;
            BatchNote returnedNote = (BatchNote)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<BatchNote>(view.Model);
            Assert.AreEqual(note.Text, returnedNote.Text);
        }

        [Test]
        public void TestBatchNoteUserCannotEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

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
        public void TestEditWithNonExistingNoteReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Edit(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestBatchNoteEditPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Edit(note);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }

        [Test]
        public void TestBatchNoteEditModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Edit(note);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestBatchNoteDelete()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Delete(1);
            ViewResult view = result as ViewResult;
            BatchNote returnedNote = (BatchNote)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<BatchNote>(view.Model);
            Assert.AreEqual(note.Text, returnedNote.Text);
        }

        [Test]
        public void TestDeleteWithNonExistingNoteReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.Delete(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestBatchNoteDeleteNoteConfirmed()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var noteService = Substitute.For<IBatchNoteService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
            noteService.Get(1).Returns(note);

            BatchNoteController controller = new BatchNoteController(batchService, noteService, userService);

            ActionResult result = controller.DeleteNoteConfirmed(1);
            RedirectToRouteResult view = result as RedirectToRouteResult;

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }
    }
}
