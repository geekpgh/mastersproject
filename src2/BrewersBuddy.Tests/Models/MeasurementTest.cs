using System;
using NUnit.Framework;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class MeasurementTest
    {
        [Test]
        public void TestAddMeasurement()
        {
            UserProfile jon = TestUtils.createUser(111, "Jon", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Beer, jon);
            Measurement measurment = TestUtils.createMeasurement(batch, "Test Measurement", "Taking weekly PH measurement", "PH", 7.0);

            Assert.IsTrue(batch.Measurements.Contains(measurment));
        }
    }
}
