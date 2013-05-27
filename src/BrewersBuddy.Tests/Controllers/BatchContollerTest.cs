using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrewersBuddy.Models;
using BrewersBuddy.Controllers;

using BrewersBuddy.Tests.Utils;
using System.Web.Mvc;
using System.Collections;

namespace BrewersBuddy.Tests.Controllers
{
    [TestClass]
    public class BatchContollerTest
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
