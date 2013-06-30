using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;

namespace BrewersBuddy.Tests.Models
{
    [TestClass]
    public class MeasurementTest
    {
        [TestMethod]
        public void TestAddMeasurement()
        {
            UserProfile jon = TestUtils.createUser(111, "Jon", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Beer, jon);
            Measurement measurment = TestUtils.createMeasurement(batch, "Test Measurement", "Taking weekly PH measurement", "PH", 7.0);

            Assert.IsTrue(batch.Measurements.Contains(measurment));
        }
    }
}
