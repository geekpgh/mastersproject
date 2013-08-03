using BrewersBuddy.Models;
using BrewersBuddy.Services;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class BatchRatingServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, peter);

            BatchRatingService ratingService = new BatchRatingService();

            BatchRating rating = new BatchRating()
            {
                BatchId = batch.BatchId,
                UserId = peter.UserId,
                Rating = 50
            };

            ratingService.Create(rating);

            BatchRating foundRating = context.BatchRatings.Find(rating.BatchId, rating.UserId);

            Assert.IsNotNull(foundRating);
            Assert.AreEqual(rating.BatchId, foundRating.BatchId);
            Assert.AreEqual(rating.UserId, foundRating.UserId);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchRating rating = TestUtils.createBatchRating(context, batch, bilbo, 50, "This is my rating");

            BatchRatingService ratingService = new BatchRatingService();
            BatchRating foundRating = ratingService.GetUserRatingForBatch(batch.BatchId, bilbo.UserId);

            //Now delete it and see that it is gone
            ratingService.Delete(foundRating);

            BatchRating foundratingDelete = ratingService.GetUserRatingForBatch(batch.BatchId, bilbo.UserId);
            Assert.IsNull(foundratingDelete);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchRating rating = TestUtils.createBatchRating(context, batch, bilbo, 50, "This is my rating");

            //Now change it
            rating.Rating = 75;

            BatchRatingService ratingService = new BatchRatingService();
            ratingService.Update(rating);

            //Get it  and see it changed
            BatchRating alteredRating = context.BatchRatings.Find(batch.BatchId, bilbo.UserId);
            Assert.AreEqual(75, alteredRating.Rating);
        }

        [Test]
        public void TestGetAllForBatch()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            UserProfile frodo = TestUtils.createUser(context, "frodo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchRating rating1 = TestUtils.createBatchRating(context, batch, bilbo, 50, "This is bilbos rating");
            BatchRating rating2 = TestUtils.createBatchRating(context, batch, frodo, 75, "This is frodos rating");

            Batch batch2 = TestUtils.createBatch(context, "Wrong Batch", BatchType.Beer, frodo);
            BatchRating note3 = TestUtils.createBatchRating(context, batch2, frodo, 10, "This is frodos rating two");

            BatchRatingService ratingService = new BatchRatingService();
            IEnumerable<BatchRating> ratings = ratingService.GetAllForBatch(batch.BatchId);

            Assert.AreEqual(2, ratings.Count());
            Assert.AreEqual(rating1.BatchId, ratings.ElementAt(0).BatchId);
            Assert.AreEqual(rating2.BatchId, ratings.ElementAt(1).BatchId);
            Assert.AreEqual(rating1.UserId, ratings.ElementAt(0).UserId);
            Assert.AreEqual(rating2.UserId, ratings.ElementAt(1).UserId);
            Assert.AreEqual(50, ratings.ElementAt(0).Rating);
            Assert.AreEqual(75, ratings.ElementAt(1).Rating);
        }

        [Test]
        public void TestGetUserRatingForBatch()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            UserProfile frodo = TestUtils.createUser(context, "frodo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchRating rating1 = TestUtils.createBatchRating(context, batch, bilbo, 50, "This is bilbos rating");
            BatchRating rating2 = TestUtils.createBatchRating(context, batch, frodo, 75, "This is frodos rating");

            Batch batch2 = TestUtils.createBatch(context, "Wrong Batch", BatchType.Beer, frodo);
            BatchRating note3 = TestUtils.createBatchRating(context, batch2, frodo, 10, "This is frodos rating two");

            BatchRatingService ratingService = new BatchRatingService();

            BatchRating rating;

            rating = ratingService.GetUserRatingForBatch(batch.BatchId, bilbo.UserId);
            Assert.AreEqual(50, rating.Rating);

            rating = ratingService.GetUserRatingForBatch(batch.BatchId, frodo.UserId);
            Assert.AreEqual(75, rating.Rating);
        }
    }
}
