using System;
using NUnit.Framework;
using BrewersBuddy.Services;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using System.Collections.Generic;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class MeasurementServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, peter);
            MeasurementService measurementService = new MeasurementService();
            Measurement measurement = new Measurement();
            measurement.MeasurementId = 1;
            measurement.Measured = "PH";
            measurement.Name = "Test Measurement";
            measurement.MeasurementDate = DateTime.Now;
            measurement.Value = 7.7;
            measurement.BatchId = batch.BatchId;

            measurementService.Create(measurement);

            Measurement foundMeasurement = context.Measurements.Find(measurement.MeasurementId);

            Assert.IsNotNull(foundMeasurement);
            Assert.AreEqual(measurement.MeasurementId, foundMeasurement.MeasurementId);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Measurement", "This is a test", "PH", 5.5);

            //See that the service can find it
            MeasurementService measurementService = new MeasurementService();
            Measurement foundMeasurement = measurementService.Get(measurement.MeasurementId);

            Assert.IsNotNull(foundMeasurement);
            Assert.AreEqual(measurement.MeasurementId, foundMeasurement.MeasurementId);
            Assert.AreEqual(measurement.Name, foundMeasurement.Name);

            //Now delete it and see that it is gone
            measurementService.Delete(foundMeasurement);

            Measurement foundMeasurementDelete = measurementService.Get(foundMeasurement.BatchId);
            Assert.IsNull(foundMeasurementDelete);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Measurement", "This is a test", "PH", 5.5);

            MeasurementService measurementService = new MeasurementService();
            Measurement foundMeasurement = measurementService.Get(measurement.MeasurementId);

            Assert.IsNotNull(foundMeasurement);
            Assert.AreEqual(measurement.MeasurementId, foundMeasurement.MeasurementId);
            Assert.AreEqual(measurement.Name, foundMeasurement.Name);
        }


        [Test]
        public void TestGetNonExistant()
        {
            MeasurementService measurementService = new MeasurementService();
            Measurement foundMeasurement = measurementService.Get(5);

            Assert.IsNull(foundMeasurement);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Measurement", "This is a test", "PH", 5.5);

            //Now change it
            measurement.Name = "Altered Measurement";
            MeasurementService measurementService = new MeasurementService();
            measurementService.Update(measurement);

            //Get it  and see it changed
            Measurement alteredMeasurement = context.Measurements.Find(measurement.MeasurementId);
            Assert.AreEqual("Altered Measurement", alteredMeasurement.Name);
        }

        [Test]
        public void TestGetAllForBatch()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Measurement", "This is a test", "PH", 5.5);
            Measurement measurement2 = TestUtils.createMeasurement(context, batch, "Test Measurement", "This is a test", "PH", 5.5);

            Batch batch2 = TestUtils.createBatch(context, "Wrong Batch", BatchType.Beer, peter);
            Measurement measurement3 = TestUtils.createMeasurement(context, batch2, "Test Measurement", "This is a test", "PH", 5.5);

            MeasurementService measurementService = new MeasurementService();
            IEnumerable<Measurement> measurementsEnumerable = measurementService.GetAllForBatch(batch.BatchId);

            int foundCount = 0;
            foreach (Measurement foundMeasurement in measurementsEnumerable)
            {
                if (foundMeasurement.MeasurementId == measurement3.MeasurementId)
                {
                    Assert.Fail("Measurement found for wrong user");
                }

                if (foundMeasurement.MeasurementId == measurement.MeasurementId || foundMeasurement.MeasurementId == measurement2.MeasurementId)
                {
                    foundCount++;
                }
            }

            Assert.AreEqual(2, foundCount);
        }
    }
}
