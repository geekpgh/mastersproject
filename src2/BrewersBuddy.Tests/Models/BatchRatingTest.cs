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
        [ExpectedException(typeof(DbUpdateException))]
        public void TestBatchAndUserMustBeUnique()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);

            // Create the first rating with user + batch combination
            TestUtils.createBatchRating(batch, bob, 100, "");

            // Create the second rating with the user + batch combination
            // This should fail with DbUpdateException because of a duplicate key
            TestUtils.createBatchRating(batch, bob, 90, "");
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

            BatchRating rating = context.BatchRatings.Find(bob.UserId, batch.BatchId);

            Assert.IsNotNull(rating.User);
            Assert.AreEqual(bob.UserId, rating.User.UserId);
        }

        [Test]
        public void TestCanRetrieveAssociatedBatch()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchRating(batch, bob, 100, "");

            BatchRating rating = context.BatchRatings.Find(bob.UserId, batch.BatchId);

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

        [Test]
        //Test that it isn't truncated if short
        public void TestSummaryLengthShort()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            BatchRating rating = TestUtils.createBatchRating(batch, bob, 100, "rating");

            Assert.AreEqual(rating.SummaryText, "rating");
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

            BatchRating rating = TestUtils.createBatchRating(batch, bob, 100, longText);

            //The 3 is for the ...
            Assert.True(rating.SummaryText.Length == 203);
        }
    }
}
