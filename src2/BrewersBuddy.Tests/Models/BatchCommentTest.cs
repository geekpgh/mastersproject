﻿using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class BatchCommentTest : DbTestBase
    {
        [Test]
        public void TestCreateBatchComment()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchComment(context, batch, bob, "my comment");

            BatchComment comment = context.BatchComments.First();

            Assert.IsNotNull(comment);
            Assert.AreEqual("my comment", comment.Comment);
            Assert.AreEqual(batch.BatchId, comment.BatchId);
        }

        [Test]
        public void TestCanRetrieveAssociatedUser()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchComment(context, batch, bob, "my comment");

            BatchComment comment = context.BatchComments.First();

            Assert.IsNotNull(comment.User);
            Assert.AreEqual(bob.UserId, comment.User.UserId);
        }

        [Test]
        public void TestCanRetrieveAssociatedBatch()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchComment(context, batch, bob, "my comment");

            BatchComment comment = context.BatchComments.First();

            Assert.IsNotNull(comment.Batch);
            Assert.AreEqual(batch.BatchId, comment.Batch.BatchId);
        }

        [Test]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestCommentCannotBeEmpty()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchComment comment = TestUtils.createBatchComment(context, batch, bob, "");
        }

        [Test]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestCommentCannotBeLongerThan256Characters()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= 256; i++)
            {
                sb.Append("a");
            }

            BatchComment comment = TestUtils.createBatchComment(context, batch, bob, sb.ToString());
        }

        [Test]
        public void TestCommentCanBeExactly256Characters()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 256; i++)
            {
                sb.Append("a");
            }

            BatchComment comment = TestUtils.createBatchComment(context, batch, bob, sb.ToString());

            Assert.IsNotNull(comment);
        }
    }
}
