using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class BatchRatingTest : DbTestBase
    {
        [Test]
        public void TestBatchAndUserMustBeUnique()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);

            // Create the first rating with user + batch combination
            TestUtils.createBatchRating(batch, bob, 100, "");

            // Create the second rating with the user + batch combination
            // This should fail with DbUpdateException because of a duplicate key
            Assert.Throws<InvalidOperationException>(() => TestUtils.createBatchRating(batch, bob, 90, ""));
        }

        [Test]
        public void TestCreateBatchRating()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchRating(batch, bob, 100, "");

            BatchRating rating = context.BatchRatings.Find(bob.UserId, batch.BatchId);

            Assert.IsNotNull(rating);
        }

        [Test]
        public void TestCanRetrieveAssociatedUser()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchRating(batch, bob, 100, "");

            BatchRating rating = context.BatchRatings.Find(batch.BatchId, bob.UserId);

            Assert.IsNotNull(rating.User);
            Assert.AreEqual(bob.UserId, rating.User.UserId);
        }

        [Test]
        public void TestCanRetrieveAssociatedBatch()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchRating(batch, bob, 100, "");

            BatchRating rating = context.BatchRatings.Find(batch.BatchId, bob.UserId);

            Assert.IsNotNull(rating.Batch);
            Assert.AreEqual(batch.BatchId, rating.Batch.BatchId);
        }

        [Test]
        public void TestUserCanHaveMultipleRatings()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");

            // Create 10 ratings and assign them to bob
            List<Batch> batches = new List<Batch>();
            for (int i = 0; i < 10; i++)
            {
                Batch batch = TestUtils.createBatch("Test" + i, BatchType.Beer, bob);
                TestUtils.createBatchRating(batch, bob, 50, "");
            }

            IEnumerable<BatchRating> ratingsForBob = context.BatchRatings
                .Where(r => r.UserId == bob.UserId);

            Assert.AreEqual(10, ratingsForBob.Count());
        }

        [Test]
        public void TestRatingCanHaveComment()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchRating(batch, bob, 100, "this is a comment");

            BatchRating rating = context.BatchRatings.Find(bob.UserId, batch.BatchId);

            Assert.IsNotNull(rating);
            Assert.AreEqual("this is a comment", rating.Comment);
        }

        [Test]
        public void TestRatingCanHaveNullComment()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchRating(batch, bob, 100, null);

            BatchRating rating = context.BatchRatings.Find(bob.UserId, batch.BatchId);

            Assert.IsNotNull(rating);
            Assert.AreEqual(null, rating.Comment);
        }
    }
}
