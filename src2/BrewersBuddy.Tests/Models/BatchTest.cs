using System.Data.Entity;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;

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
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);

            Assert.IsTrue(batch.Measurements.Contains(measurement));
        }

        [Test]
        public void TestRemoveMeasurement()
        {
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            Measurement measurement = TestUtils.createMeasurement(context, batch, "Test Name",
                "This is a test!", "Gravity", 1.01);

            Assert.IsTrue(batch.Measurements.Contains(measurement));

            context.Measurements.Remove(measurement);
            batch.Measurements.Remove(measurement);
            
            context.SaveChanges();

            Assert.IsFalse(batch.Measurements.Contains(measurement));
        }


        [Test]
        public void TestAddBatchAction()
        {
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            BatchAction action = TestUtils.createBatchAction(context, batch, jon, 
                "Bottles the beer", "99 bottles of beer on the wall!", ActionType.Bottle);

            Assert.IsTrue(batch.Actions.Contains(action));
        }

        [Test]
        public void TestRemoveBatchAction()
        {
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            BatchAction action = TestUtils.createBatchAction(context, batch, jon,
                "Bottles the beer", "99 bottles of beer on the wall!", ActionType.Bottle);

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
            Friend newFriend = TestUtils.createFriend(context, fred, bob);

            //Verify the collaborator can view
            Assert.IsTrue(batch.CanView(fred.UserId));
        }

        [Test]
        public void TestCannotEditFriend()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Friend newFriend = TestUtils.createFriend(context, fred, bob);

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

        [Test]
        //Test that it isn't truncated if short
        public void TestSummaryLengthShort()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            batch.Description = "Description of a Batch";
            context.SaveChanges();

            Assert.AreEqual(batch.SummaryText, "Description of a Batch");
        }

        [Test]
        //test that it is truncated
        public void TestSummaryTruncate()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            string longText = "This is a very very very long string it is very long. This is a very very very long string it is very long. ";

            while (longText.Length < 200)
            {
                longText += "This is a very very very long string it is very long. This is a very very very long string it is very long. ";
            }

            //Make sure the string is setup correctly
            Assert.True(longText.Length >= 200);

            batch.Description = longText;
            context.SaveChanges();

            //The 3 is for the ...
            Assert.True(batch.SummaryText.Length == 203);
        }

        [Test]
        public void TestGetType()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Wine, bob);
            batch.BatchTypeValue = 1;
            context.SaveChanges();

            Assert.AreEqual(batch.Type, BatchType.Mead);
        }

        [Test]
        public void TestIsBatchOwner()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Wine, bob);

            Assert.AreEqual(batch.IsOwner(bob.UserId), true);
            Assert.AreNotEqual(batch.IsOwner(fred.UserId), true);
        }

        [Test]
        public void TestHasRatedRatingsIsNull()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Wine, bob);

            Assert.AreEqual(batch.HasRated(bob.UserId), false);
        }

        [Test]
        public void TestHasRated()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Wine, bob);
            BatchRating rating = TestUtils.createBatchRating(context, batch, bob, 92, 
                "Great Beer!");

            Assert.AreEqual(batch.HasRated(bob.UserId), true);
            Assert.AreNotEqual(batch.HasRated(fred.UserId), true);
        }

        [Test]
        public void TestCanRate()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            Batch batch1 = TestUtils.createBatch(context, "Test", BatchType.Wine, bob);
            Batch batch2 = TestUtils.createBatch(context, "Test", BatchType.Wine, fred);
            BatchRating rating1 = TestUtils.createBatchRating(context, batch1, bob, 92,
                "Great Beer!");
            BatchRating rating2 = TestUtils.createBatchRating(context, batch2, bob, 98,
                "Greatest Beer!");

            // Should return False - canView(true) && !hasRated(true)
            Assert.AreEqual(batch1.CanRate(bob.UserId), false);

            // Should return False - canView(false) && !hasRated(false)
            Assert.AreEqual(batch1.CanRate(fred.UserId), false);

            // Should return True - canView(true) && !hasRated(false)
            Assert.AreEqual(batch2.CanRate(fred.UserId), true);

            // Should return False - canView(false) && !hasRated(true)
            Assert.AreEqual(batch2.CanRate(bob.UserId), false);
        }
    }
}
