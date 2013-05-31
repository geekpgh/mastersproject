using System;
using System.Collections.Generic;
using System.Data.Entity;
using BrewersBuddy.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrewersBuddy.Tests.Models
{
    [TestClass]
    public class BatchTest
    {
        [TestMethod]
        public void TestCreateBatch()
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Mead;
            batch.StartDate = DateTime.Now;

            db.Batches.Add(batch);

            DbSet<Batch> batches = db.Batches;
            Assert.AreEqual(batches.Find(batch.BatchId), batch);
        }


        [TestMethod]
        public void TestAddMeasurement()
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

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
        public void TestRemoveMeasurement()
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

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

            batch.Measurements.Remove(measurement);

            Assert.IsFalse(batch.Measurements.Contains(measurement));
        }


        [TestMethod]
        public void TestAddBatchAction()
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

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

        [TestMethod]
        public void TestRemoveBatchAction()
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

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

            db.BatchActions.Remove(action);

            Assert.IsFalse(batch.Actions.Contains(action));
        }

        [TestMethod]
        public void TestAddToInvetory()
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Beer;
            batch.StartDate = DateTime.Now;

            db.Batches.Add(batch);

            Container container = new Container();
            container.Batch = batch;
            container.Name = "Test container";
            container.Type = ContainerType.Bottle;
            container.Unit = ContainerVolumeUnits.Milliliter;
            container.Volume = 750;

            db.Containers.Add(container);

            Cellar cellar = new Cellar();
            cellar.Name = "test cellar";
            cellar.Description = "My stash";
            cellar.Containers = new List<Container>();
            cellar.Containers.Add(container);

            db.Cellars.Add(cellar);

            Assert.IsTrue(cellar.Containers.Contains(container));
        }

        [TestMethod]
        public void TestViewBacthActions()
        {

        }

    }
}
