using System;
using System.Collections.Generic;
using System.Data.Entity;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using BrewersBuddy.Tests.Utilities;
using System.Data.Entity.Infrastructure;

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
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";
            userProfile.UserId = 1;

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Wine;
            batch.StartDate = DateTime.Now;
            batch.BatchId = 1;
            batch.Owner = userProfile;

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
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";
            userProfile.UserId = 1;

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Wine;
            batch.StartDate = DateTime.Now;
            batch.BatchId = 1;
            batch.Owner = userProfile;

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
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";
            userProfile.UserId = 1;

            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Beer;
            batch.StartDate = DateTime.Now;
            batch.BatchId = 1;
            batch.Owner = userProfile;

            context.Batches.Add(batch);

            BatchAction action = new BatchAction();
            action.Batch = batch;
            action.Description = "99 bottles of beer on the wall!";
            action.Type = ActionType.Bottle;
            action.Title = "Bottles the beer";
            action.ActionDate = DateTime.Now;
            action.Performer = userProfile;

            context.BatchActions.Add(action);
            context.SaveChanges();

            Assert.IsTrue(batch.Actions.Contains(action));
        }

        [Test]
        public void TestRemoveBatchAction()
        {
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = "NUNIT_Test";
            userProfile.Email = "NUNIT@Test.com";
            userProfile.FirstName = "Nunit";
            userProfile.LastName = "Test";
            userProfile.City = "Brewery";
            userProfile.State = "KS";
            userProfile.Zip = "12345";
            userProfile.UserId = 1;

            context.UserProfiles.Add(userProfile);
            Batch batch = new Batch();
            batch.Name = "Test";
            batch.Type = BatchType.Beer;
            batch.StartDate = DateTime.Now;
            batch.BatchId = 1;
            batch.OwnerId = 1;

            context.Batches.Add(batch);

            BatchAction action = new BatchAction();
            action.Batch = batch;
            action.Description = "99 bottles of beer on the wall!";
            action.Type = ActionType.Bottle;
            action.Title = "Bottles the beer";
            action.ActionDate = DateTime.Now;
            action.Performer = userProfile;

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

			Friend newFriend = new Friend();
			newFriend.UserId = bob.UserId;
			newFriend.FriendUserId = fred.UserId;
			newFriend.User = bob;

			bob.Friends.Add(newFriend);
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

			Friend newFriend = new Friend();
			newFriend.UserId = bob.UserId;
			newFriend.FriendUserId = fred.UserId;
			newFriend.User = bob;

			bob.Friends.Add(newFriend);
            context.SaveChanges();

            //Verify the owner can view
            Assert.IsFalse(batch.CanEdit(fred.UserId));
        }

        [Test]
        public void TestCanHaveMultipleCollaborators()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile bill = TestUtils.createUser(context, "Bill", "Smith");
            UserProfile ben = TestUtils.createUser(context, "Ben", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            batch.Collaborators.Add(bill);
            context.SaveChanges();

            Assert.AreEqual(1, batch.Collaborators.Count);

            batch.Collaborators.Add(ben);
            context.SaveChanges();

            Assert.AreEqual(2, batch.Collaborators.Count);
        }

        [Test]
        public void TestCanOnlyAddSameCollaboratorOnce()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile bill = TestUtils.createUser(context, "Bill", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            batch.Collaborators.Add(bill);
            context.SaveChanges();

            Assert.AreEqual(1, batch.Collaborators.Count);

            batch.Collaborators.Add(bill);
            context.SaveChanges();

            // For whatever reason, the list will always increment
            // and the current context will show the collaborator
            // count as 2. However, if a new context is created, and
            // the database is requieried, then the expected number
            // is returned
            BrewersBuddyContext context2 = new BrewersBuddyContext();
            batch = context2.Batches.Find(batch.BatchId);
            Assert.AreEqual(1, batch.Collaborators.Count);
        }
    }
}
