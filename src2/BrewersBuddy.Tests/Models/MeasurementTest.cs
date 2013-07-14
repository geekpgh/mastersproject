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

        [Test]
        //Test that it isn't truncated if short
        public void TestSummaryLengthShort()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(batch, "Test Measurement", "measurement", "PH", 7.0);

            Assert.AreEqual(measurment.SummaryText, "measurement");
        }

        [Test]
        //test that it is truncated
        public void TestSummaryTruncate()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            string longText = "This is a very very very long string it is very long. This is a very very very long string it is very long. ";

            while (longText.Length < 200)
            {
                longText += "This is a very very very long string it is very long. This is a very very very long string it is very long. ";
            }

            //Make sure the string is setup correctly
            Assert.True(longText.Length >= 200);

            Measurement measurment = TestUtils.createMeasurement(batch, "Test Measurement", longText, "PH", 7.0);

            //The 3 is for the ...
            Assert.True(measurment.SummaryText.Length == 203);
        }
    }
}
