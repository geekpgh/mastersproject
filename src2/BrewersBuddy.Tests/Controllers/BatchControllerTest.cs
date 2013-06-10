﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using BrewersBuddy.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

namespace BrewersBuddy.Tests.Controllers
{
    [TestClass]
    public class BatchControllerTest : DbTestBase
    {
        [TestMethod]
        public void TestBatchList()
        {
            UserProfile user = TestUtils.createUser(111, "Mike", "Smith");
            ICollection<Batch> batches = TestUtils.createBatches(5, user);

            BatchController controller = new BatchController();
            ViewResult result = (ViewResult)controller.Index();
            ViewDataDictionary data = result.ViewData;

            IList batchesList = result.ViewData.Model as IList;

            Assert.IsTrue(batchesList.Count == 5);
        }

        [TestMethod]
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


        [TestMethod]
        public void TestEditBatch()
        {
            Assert.Fail("Implement me");
        }

    }
}
