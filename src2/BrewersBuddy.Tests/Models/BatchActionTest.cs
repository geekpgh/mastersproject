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
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(context, batch, bob, "my action", "This is an action", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();

            Assert.IsNotNull(action);
            Assert.AreEqual("my action", action.Title);
            Assert.AreEqual(ActionType.Bottle, action.Type);
            Assert.AreEqual("This is an action", action.Description);
            Assert.AreEqual(batch.BatchId, action.Batch.BatchId);
        }

        [Test]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestTitleCannotBeEmpty()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "", "desc", ActionType.Bottle);
        }

        [Test]
        [ExpectedException(typeof(DbEntityValidationException))]
        public void TestDescriptionCannotBeEmpty()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "title", "", ActionType.Bottle);
        }

        [Test]
        public void TestCanRetrieveAssociatedUser()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();

            Assert.IsNotNull(action.PerformerId);
            Assert.AreEqual(bob.UserId, action.PerformerId);
        }

        [Test]
        public void TestCanRetrieveAssociatedBatch()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();

            Assert.IsNotNull(action.Batch);
            Assert.AreEqual(batch.BatchId, action.Batch.BatchId);
        }

        [Test]
        //Test that it isn't truncated if short
        public void TestSummaryLengthShort()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            BatchAction action = context.BatchActions.First();
            Assert.AreEqual(action.SummaryText, "desc");
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

            TestUtils.createBatchAction(context, batch, bob, "my action", longText, ActionType.Bottle);

            BatchAction action = context.BatchActions.First();
            //The 3 is for the ...
            Assert.True(action.SummaryText.Length == 203);
        }

        [Test]
        public void TestCanViewOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            //Verify the owner can view
            Assert.IsTrue(action.CanView(bob.UserId));
        }

        [Test]
        public void TestCanEditOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            //Verify the collaborator can edit
            Assert.IsTrue(action.CanEdit(bob.UserId));
        }

        [Test]
        public void TestCanViewCollaborator()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(action.CanView(fred.UserId));
        }

        [Test]
        public void TestCanEditCollaborator()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            Assert.IsTrue(action.CanEdit(fred.UserId));
        }

        [Test]
        public void TestCanViewFriend()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            bob.Friends.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(action.CanView(fred.UserId));
        }

        [Test]
        public void TestCannotEditFriend()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchAction action = TestUtils.createBatchAction(context, batch, bob, "my action", "desc", ActionType.Bottle);

            bob.Friends.Add(fred);
            context.SaveChanges();

            //Verify the owner can view
            Assert.IsFalse(action.CanEdit(fred.UserId));
        }
    }
}
