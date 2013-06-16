using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using BrewersBuddy.Tests.Utilities;
using NUnit.Framework;
using System.Web;
using NSubstitute;
using System.Web.Routing;

namespace BrewersBuddy.Tests.Controllers
{
    [TestFixture]
    public class BatchControllerTest : DbTestBase
    {
        [Test]
        public void TestBatchList()
        {
            UserProfile user = TestUtils.createUser(111, "Mike", "Smith");
            ICollection<Batch> batches = TestUtils.createBatches(5, user);

            var context = Substitute.For<HttpContextBase>();
            context.Session["UserID"].Returns(111);
            var controllerContext = new ControllerContext(context, new RouteData(), new BatchController());
            BatchController controller = (BatchController)controllerContext.Controller;

            
            ViewResult result = (ViewResult)controller.Index();
            ViewDataDictionary data = result.ViewData;

            IList batchesList = result.ViewData.Model as IList;

            Assert.IsTrue(batchesList.Count == 5);
        }

        [Test]
        public void TestBatchOnlyOwnedList()
        {
            //Create 5 batches for mike
            UserProfile userMike = TestUtils.createUser(111, "Mike", "Smith");
            ICollection<Batch> batchesMike = TestUtils.createBatches(5, userMike);

            //Now create some batches own by others these should not be returned 
            //by the view
            UserProfile userBob = TestUtils.createUser(112, "Bob", "Smith");
            ICollection<Batch> batchesBob = TestUtils.createBatches(3, userBob);

            UserProfile userTim = TestUtils.createUser(113, "Tim", "Smith");
            ICollection<Batch> batchesTim = TestUtils.createBatches(3, userTim);
    
            BatchController controller = new BatchController();

            //TODO
            //Mock that user mike is logged in

            ViewResult result = (ViewResult)controller.Index();
            ViewDataDictionary data = result.ViewData;

            IList batchesList = result.ViewData.Model as IList;

            //Verify that the controller only returns the logged in user's Batches.
            Assert.IsTrue(batchesList.Count == 5);
        }


        [Test]
        public void TestEditBatch()
        {
            Assert.Fail("Implement me");
        }

    }
}
