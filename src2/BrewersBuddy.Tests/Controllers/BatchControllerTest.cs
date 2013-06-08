using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using BrewersBuddy.Controllers;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using BrewersBuddy.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            foreach (Batch batch in batches)
            {
                //Assert.IsTrue(data.Contains(batch));
            }
        }


        [TestMethod]
        public void TestEditBatch()
        {
            Assert.Fail("Implement me");
        }

    }
}
