using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrewersBuddy.Models;

namespace BrewersBuddy.Tests.Models
{
    [TestClass]
    public class BatchTests
    {
        [TestMethod]
        public void TestAddMeasurement()
        {
            BatchDBContext db = new BatchDBContext();

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Wine;
            batch.StartDate = DateTime.Now;

            db.Batches.Add(batch);

            Measurement measurement = new Measurement();
            measurement.Batch = batch;
            measurement.Description = "This is a test!";
            measurement.Measured = "Gravity";
            measurement.Value = 1.01;
            measurement.MeasurementDate = DateTime.Now;

            db.Measurements.Add(measurement);

            Assert.IsTrue(batch.Measurements.Contains(measurement));
        }


        [TestMethod]
        public void TestAddBatchAction()
        {
            BatchDBContext db = new BatchDBContext();

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Beer;
            batch.StartDate = DateTime.Now;

            db.Batches.Add(batch);

            BatchAction action = new BatchAction();
            action.Batch = batch;
            action.Description = "99 bottles of beer on the wall!";
            action.Type = ActionType.Bottle;
            action.Title = "Bottles the beer";
            action.ActionDate = DateTime.Now;

            db.BatchActions.Add(action);

            Assert.IsTrue(batch.Actions.Contains(action));
        }
    }
}
