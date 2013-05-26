using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrewersBuddy.Models;
using BrewersBuddy.Controllers;

using BrewersBuddy.Tests.Utils;
using System.Web.Mvc;

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

            Assert.IsTrue(data.Count == 5);

            foreach (Batch batch in batches)
            {
                //Assert.IsTrue(data.Contains(batch));
            }
        }
    }
}
