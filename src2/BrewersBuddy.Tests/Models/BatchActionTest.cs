using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;


namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class BatchActionTest : DbTestBase
    {
        [Test]
        public void TestCreateBatchAction()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(batch, bob, "my action", "This is an action", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();

            Assert.IsNotNull(action);
            Assert.AreEqual("my action", action.Title);
            Assert.AreEqual(ActionType.Bottle, action.Type);
            Assert.AreEqual("This is an action", action.Description);
            Assert.AreEqual(batch.BatchId, action.Batch);
        }

        [Test]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestTitleCannotBeEmpty()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(batch, bob, "", "desc", ActionType.Bottle);
        }

        [Test]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestDescriptionCannotBeEmpty()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(batch, bob, "title", "", ActionType.Bottle);
        }

        [Test]
        public void TestCanRetrieveAssociatedUser()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(batch, bob, "my action", "desc", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();

            Assert.IsNotNull(action.PerformerId);
            Assert.AreEqual(bob.UserId, action.PerformerId);
        }

        [Test]
        public void TestCanRetrieveAssociatedBatch()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(batch, bob, "my action", "desc", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();

            Assert.IsNotNull(action.Batch);
            Assert.AreEqual(batch.BatchId, action.Batch.BatchId);
        }
    }
}
