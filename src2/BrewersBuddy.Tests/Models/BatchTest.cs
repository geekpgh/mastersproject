using System;
using System.Collections.Generic;
using System.Data.Entity;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using BrewersBuddy.Tests.Utilities;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class BatchTest : DbTestBase
    {
        [Test]
        public void TestCreateBatch()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            DbSet<Batch> batches = context.Batches;
            Batch foundBatch = batches.Find(batch.BatchId);

            //Verify it was properly created
            Assert.AreEqual( batch.BatchId, foundBatch.BatchId);
        }


        [Test]
        public void TestAddMeasurement()
        {
            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Wine;
            batch.StartDate = DateTime.Now;

            context.Batches.Add(batch);

            Measurement measurement = new Measurement();
            measurement.Batch = batch;
            measurement.Description = "This is a test!";
            measurement.Measured = "Gravity";
            measurement.Value = 1.01;
            measurement.MeasurementDate = DateTime.Now;

            context.Measurements.Add(measurement);
            context.SaveChanges();

            Assert.IsTrue(batch.Measurements.Contains(measurement));
        }

        [Test]
        public void TestRemoveMeasurement()
        {
            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Wine;
            batch.StartDate = DateTime.Now;

            context.Batches.Add(batch);

            Measurement measurement = new Measurement();
            measurement.Batch = batch;
            measurement.Description = "This is a test!";
            measurement.Measured = "Gravity";
            measurement.Value = 1.01;
            measurement.MeasurementDate = DateTime.Now;

            context.Measurements.Add(measurement);
            context.SaveChanges();

            Assert.IsTrue(batch.Measurements.Contains(measurement));

            context.Measurements.Remove(measurement);
            batch.Measurements.Remove(measurement);
            
            context.SaveChanges();

            Assert.IsFalse(batch.Measurements.Contains(measurement));
        }


        [Test]
        public void TestAddBatchAction()
        {
            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Beer;
            batch.StartDate = DateTime.Now;

            context.Batches.Add(batch);

            BatchAction action = new BatchAction();
            action.Batch = batch;
            action.Description = "99 bottles of beer on the wall!";
            action.Type = ActionType.Bottle;
            action.Title = "Bottles the beer";
            action.ActionDate = DateTime.Now;

            context.BatchActions.Add(action);
            context.SaveChanges();

            Assert.IsTrue(batch.Actions.Contains(action));
        }

        [Test]
        public void TestRemoveBatchAction()
        {
            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Beer;
            batch.StartDate = DateTime.Now;

            context.Batches.Add(batch);

            BatchAction action = new BatchAction();
            action.Batch = batch;
            action.Description = "99 bottles of beer on the wall!";
            action.Type = ActionType.Bottle;
            action.Title = "Bottles the beer";
            action.ActionDate = DateTime.Now;

            context.BatchActions.Add(action);
            context.SaveChanges();

            Assert.IsTrue(batch.Actions.Contains(action));

            context.BatchActions.Remove(action);
            context.SaveChanges();

            Assert.IsFalse(batch.Actions.Contains(action));
        }

        [Test]
        public void TestCanViewOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            //Verify the owner can view
            Assert.IsTrue(batch.CanView(bob.UserId));
        }

        [Test]
        public void TestCanEditOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            //Verify the collaborator can edit
            Assert.IsTrue(batch.CanEdit(bob.UserId));
        }

        [Test]
        public void TestCanViewCollaborator()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(batch.CanView(fred.UserId));
        }

        [Test]
        public void TestCanEditCollaborator()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            Assert.IsTrue(batch.CanEdit(fred.UserId));
        }

        [Test]
        public void TestCanViewFriend()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            bob.Friends.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(batch.CanView(fred.UserId));
        }

        [Test]
        public void TestCannotEditFriend()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            bob.Friends.Add(fred);
            context.SaveChanges();

            //Verify the owner can view
            Assert.IsFalse(batch.CanEdit(fred.UserId));
        }

		//[Test]
		//public void TestAddToInvetory()
		//{
		//	Batch batch = new Batch();
		//	batch.Name = "Test";
		//	batch.Type = BatchType.Beer;
		//	batch.StartDate = DateTime.Now;

		//	context.Batches.Add(batch);
		//	context.SaveChanges();

		//	Container container = new Container();
		//	container.Batch = batch;
		//	container.Name = "Test container";
		//	container.Type = ContainerType.Bottle;
		//	container.Units = ContainerVolumeUnits.Milliliter;
		//	container.Volume = 750;

		//	context.Containers.Add(container);
		//	context.SaveChanges();

		//	Cellar cellar = new Cellar();
		//	cellar.Name = "test cellar";
		//	cellar.Description = "My stash";
		//	cellar.Containers = new List<Container>();
		//	cellar.Containers.Add(container);

		//	context.Cellars.Add(cellar);
		//	context.SaveChanges();

		//	Assert.IsTrue(cellar.Containers.Contains(container));
        //}

    }
}
