using System;
using NUnit.Framework;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using BrewersBuddy.Services;
using System.Collections.Generic;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class BatchActionServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, peter);
            BatchActionService batchActionService = new BatchActionService(context);
            BatchAction action = new BatchAction();
            action.ActionId = 1;
            action.Description = "test";
            action.Title = "Test Action";
            action.ActionDate = DateTime.Now;
            action.PerformerId = peter.UserId;
            action.BatchId = batch.BatchId;

            batchActionService.Create(action);

            BatchAction foundAction = context.BatchActions.Find(action.ActionId);

            Assert.IsNotNull(foundAction);
            Assert.AreEqual(action.ActionId, foundAction.ActionId);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchAction action = TestUtils.createBatchAction(context, batch, bilbo, "Test Action", "This is a test", ActionType.Bottle);

            //See that the service can find it
            BatchActionService actionService = new BatchActionService(context);
            BatchAction foundAction = actionService.Get(action.ActionId);

            Assert.IsNotNull(foundAction);
            Assert.AreEqual(action.ActionId, foundAction.ActionId);
            Assert.AreEqual(action.Title, foundAction.Title);

            //Now delete it and see that it is gone
            actionService.Delete(foundAction);

            BatchAction foundActionDelete = actionService.Get(foundAction.BatchId);
            Assert.IsNull(foundActionDelete);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchAction action = TestUtils.createBatchAction(context, batch, bilbo, "Test Action", "This is a test", ActionType.Bottle);

            BatchActionService actionService = new BatchActionService(context);
            BatchAction foundAction = actionService.Get(action.ActionId);

            Assert.IsNotNull(foundAction);
            Assert.AreEqual(action.ActionId, foundAction.ActionId);
            Assert.AreEqual(action.Title, foundAction.Title);
        }


        [Test]
        public void TestGetNonExistant()
        {
            BatchActionService actionService = new BatchActionService(context);
            BatchAction foundAction = actionService.Get(5);

            Assert.IsNull(foundAction);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            BatchAction action = TestUtils.createBatchAction(context, batch, peter, "Test Action", "This is a test", ActionType.Bottle);

            //Now change it
            action.Title = "Altered Action";
            BatchActionService actionService = new BatchActionService(context);
            actionService.Update(action);

            //Get it  and see it changed
            BatchAction alteredAction = context.BatchActions.Find(action.ActionId);
            Assert.AreEqual("Altered Action", alteredAction.Title);
        }

        [Test]
        public void TestGetAllForBatch()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            BatchAction action = TestUtils.createBatchAction(context, batch, peter, "Test Action", "This is a test", ActionType.Bottle);
            BatchAction action2  = TestUtils.createBatchAction(context, batch, peter, "Test Action 2", "This is a test 2", ActionType.Bottle);

            Batch batch2 = TestUtils.createBatch(context, "Wrong Batch", BatchType.Beer, peter);
            BatchAction action3 = TestUtils.createBatchAction(context, batch2, peter, "Test Action 3", "This is a test 3", ActionType.Bottle);

            BatchActionService actionService = new BatchActionService(context);
            IEnumerable<BatchAction> actionsEnumerable = actionService.GetAllForBatch(batch.BatchId);

            int foundCount = 0;
            foreach (BatchAction foundAction in actionsEnumerable)
            {
                if (foundAction.ActionId == action3.ActionId)
                {
                    Assert.Fail("Action found for wrong user");
                }

                if (foundAction.ActionId == action.ActionId || foundAction.ActionId == action2.ActionId)
                {
                    foundCount++;
                }
            }

            Assert.AreEqual(2, foundCount);
        }
    }
}
