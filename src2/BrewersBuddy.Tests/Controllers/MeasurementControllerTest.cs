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
    public class MeasurementControllerTest : DbTestBase
    {
        [Test]
        public void TestNullMeasurementServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new MeasurementController(null, userService, batchService)
                );
        }

        [Test]
        public void TestNullUserServiceThrowsArgumentNullException()
        {
            var measurementService = Substitute.For<IMeasurementService>();
            var batchService = Substitute.For<IBatchService>();

            Assert.Throws<ArgumentNullException>(() =>
                new MeasurementController(measurementService, null, batchService)
                );
        }

        [Test]
        public void TestMeasurementList()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();
            measurementService.GetAllForBatch(1).Returns(
                new Measurement[] {
                    new Measurement(),
                    new Measurement(),
                    new Measurement(),
                    new Measurement(),
                    new Measurement()
                });

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ViewResult result = (ViewResult)controller.Index(1);
            ViewDataDictionary data = result.ViewData;

            IList recipeList = result.ViewData.Model as IList;

            Assert.IsTrue(recipeList.Count == 5);
        }
        
        [Test]
        public void TestNullBatchServiceThrowsArgumentNullException()
        {
            var userService = Substitute.For<IUserService>();
            var measurementService = Substitute.For<IMeasurementService>();

            Assert.Throws<ArgumentNullException>(() =>
                new MeasurementController(measurementService, userService, null)
                );
        }

        [Test]
        public void TestCreateMeasurement()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            Measurement measurement = new Measurement();
            measurement.BatchId = 999;
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
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

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
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

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
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Create(new Measurement());

            Assert.IsInstanceOf<HttpUnauthorizedResult>(result);
        }

        [Test]
        public void TestNullMeasurementWillReturn500ErrorPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Create(new Measurement());

            Assert.IsInstanceOf<HttpStatusCodeResult>(result);
            Assert.AreEqual(500, ((HttpStatusCodeResult)result).StatusCode);
        }

        [Test]
        public void TestCreateMeasurementPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            Measurement measurement = new Measurement();
            measurement.BatchId = 999;
            ActionResult result = controller.Create(measurement);

            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.AreEqual(3, ((RedirectToRouteResult)result).RouteValues.Values.Count);
        }

        [Test]
        public void TestCreateMeasurementModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(999);

            var batchService = Substitute.For<IBatchService>();
            Batch batch = new Batch();
            batchService.Get(999).Returns(batch);

            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            Measurement measurement = new Measurement();
            measurement.BatchId = 999;
            controller.ModelState.AddModelError("key", "not valid");

            ActionResult result = controller.Create(measurement);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
         public void TestMeasurmentDetails()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
            
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Details(1);
            ViewResult view = result as ViewResult;
            Measurement returnedmeasurement = (Measurement)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Measurement>(view.Model);
            Assert.AreEqual(measurement.Name, returnedmeasurement.Name);
        }

        [Test]
        public void TestMeasurementUserCannotView()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);

            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

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
        public void TestDetailsWithNonExistingMeasurementReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Details(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestMeasurementEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Edit(1);
            ViewResult view = result as ViewResult;
            Measurement returnedmeasurement = (Measurement)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Measurement>(view.Model);
            Assert.AreEqual(measurement.Name, returnedmeasurement.Name);
        }

        [Test]
        public void TestMeasurementUserCannotEdit()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(0);
                    
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);
                    
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
        public void TestEditWithNonExistingMeasurementReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Edit(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestMeasurmentEditPost()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);

            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);
                    
            ActionResult result = controller.Edit(measurement);
            RedirectToRouteResult view = result as RedirectToRouteResult;
                    
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }

        [Test]
        public void TestMeasurementEditModelInvalid()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
                    
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);
            controller.ModelState.AddModelError("key", "not valid");
                    
            ActionResult result = controller.Edit(measurement);
                    
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void TestMeasurementDelete()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
                    
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);
                    
            ActionResult result = controller.Delete(1);
            ViewResult view = result as ViewResult;
            Measurement returnedmeasurement = (Measurement)view.Model;

            Assert.NotNull(view.Model);
            Assert.IsInstanceOf<Measurement>(view.Model);
            Assert.AreEqual(measurement.Name, returnedmeasurement.Name);
        }

        [Test]
        public void TestDeleteWithNonExistingMeasurementReturnsNotFound()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);

            ActionResult result = controller.Delete(0);

            Assert.IsInstanceOf<HttpNotFoundResult>(result);
        }

        [Test]
        public void TestMeasurementDeleteNoteConfirmed()
        {
            // Set up the controller
            var userService = Substitute.For<IUserService>();
            userService.GetCurrentUserId().Returns(1);
                    
            var batchService = Substitute.For<IBatchService>();
            var measurementService = Substitute.For<IMeasurementService>();

            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);
            measurementService.Get(1).Returns(measurement);

            MeasurementController controller = new MeasurementController(measurementService, userService, batchService);
                    
            ActionResult result = controller.DeleteConfirmed(1);
            RedirectToRouteResult view = result as RedirectToRouteResult;
                    
            Assert.IsInstanceOf<RedirectToRouteResult>(result);
            Assert.NotNull(view.RouteValues);
            Assert.AreEqual("Details/1", view.RouteValues["action"]);
        }
    }
}
