using BrewersBuddy.Models;
using BrewersBuddy.Services;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class BatchCommentServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, peter);

            BatchCommentService commentService = new BatchCommentService();

            BatchComment comment = new BatchComment()
            {
                BatchId = batch.BatchId,
                UserId = peter.UserId,
                Comment = "This is my comment"
            };

            commentService.Create(comment);

            BatchComment foundComment = context.BatchComments.Find(comment.BatchCommentId);

            Assert.IsNotNull(foundComment);
            Assert.AreEqual(comment.BatchCommentId, foundComment.BatchCommentId);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchComment comment = TestUtils.createBatchComment(context, batch, bilbo, "This is my comment");

            //See that the service can find it
            BatchCommentService commentService = new BatchCommentService();
            BatchComment foundComment = commentService.Get(comment.BatchCommentId);

            Assert.IsNotNull(foundComment);
            Assert.AreEqual(comment.BatchCommentId, foundComment.BatchCommentId);
            Assert.AreEqual(comment.Comment, foundComment.Comment);

            //Now delete it and see that it is gone
            commentService.Delete(foundComment);

            BatchComment foundCommentDelete = commentService.Get(foundComment.BatchCommentId);
            Assert.IsNull(foundCommentDelete);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchComment comment = TestUtils.createBatchComment(context, batch, bilbo, "This is my comment");

            BatchCommentService commentService = new BatchCommentService();
            BatchComment foundComment = commentService.Get(comment.BatchCommentId);

            Assert.IsNotNull(foundComment);
            Assert.AreEqual(comment.BatchCommentId, foundComment.BatchCommentId);
            Assert.AreEqual(comment.Comment, foundComment.Comment);
        }

        [Test]
        public void TestGetNonExistant()
        {
            BatchCommentService commentService = new BatchCommentService();
            BatchComment foundComment = commentService.Get(5);

            Assert.IsNull(foundComment);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchComment comment = TestUtils.createBatchComment(context, batch, bilbo, "This is my comment");

            //Now change it
            comment.Comment = "Altered Comment";
            BatchCommentService commentService = new BatchCommentService();
            commentService.Update(comment);

            //Get it  and see it changed
            BatchComment alteredComment = context.BatchComments.Find(comment.BatchCommentId);
            Assert.AreEqual("Altered Comment", alteredComment.Comment);
        }

        [Test]
        public void TestGetAllForBatch()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            UserProfile frodo = TestUtils.createUser(context, "frodo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchComment comment1 = TestUtils.createBatchComment(context, batch, bilbo, "This is bilbos comment");
            BatchComment comment2 = TestUtils.createBatchComment(context, batch, frodo, "This is frodos comment");

            Batch batch2 = TestUtils.createBatch(context, "Wrong Batch", BatchType.Beer, frodo);
            BatchComment note3 = TestUtils.createBatchComment(context, batch2, frodo, "This is frodos comment two");

            BatchCommentService commentService = new BatchCommentService();
            IEnumerable<BatchComment> comments = commentService.GetAllForBatch(batch.BatchId);

            Assert.AreEqual(2, comments.Count());
            Assert.AreEqual(comment1.BatchCommentId, comments.ElementAt(0).BatchCommentId);
            Assert.AreEqual(comment2.BatchCommentId, comments.ElementAt(1).BatchCommentId);
            Assert.AreEqual("This is bilbos comment", comments.ElementAt(0).Comment);
            Assert.AreEqual("This is frodos comment", comments.ElementAt(1).Comment);
        }
    }
}
