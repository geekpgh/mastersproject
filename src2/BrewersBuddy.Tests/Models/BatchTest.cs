using System;
using System.Collections.Generic;
using System.Data.Entity;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrewersBuddy.Tests.Models
{
    [TestClass]
    public class BatchTest
    {
        [TestMethod]
        public void TestCreateBatch()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);

            BrewersBuddyContext db = new BrewersBuddyContext();
            DbSet<Batch> batches = db.Batches;
            Batch foundBatch = batches.Find(batch.BatchId);

            //Verify it was properly created
            Assert.AreEqual( batch.BatchId, foundBatch.BatchId);
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
            db.SaveChanges();

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
            db.SaveChanges();

            Assert.IsTrue(batch.Measurements.Contains(measurement));

            batch.Measurements.Remove(measurement);
            db.SaveChanges();

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
            db.SaveChanges();

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
            db.SaveChanges();

            Assert.IsTrue(batch.Actions.Contains(action));

            db.BatchActions.Remove(action);
            db.SaveChanges();

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
            db.SaveChanges();

            Container container = new Container();
            container.Batch = batch;
            container.Name = "Test container";
            container.Type = ContainerType.Bottle;
            container.Unit = ContainerVolumeUnits.Milliliter;
            container.Volume = 750;

            db.Containers.Add(container);
            db.SaveChanges();

            Cellar cellar = new Cellar();
            cellar.Name = "test cellar";
            cellar.Description = "My stash";
            cellar.Containers = new List<Container>();
            cellar.Containers.Add(container);

            db.Cellars.Add(cellar);
            db.SaveChanges();

            Assert.IsTrue(cellar.Containers.Contains(container));
        }

        [TestMethod]
        public void TestViewBacthActions()
        {

        }

    }
}
