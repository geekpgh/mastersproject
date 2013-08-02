using System;
using NUnit.Framework;
using BrewersBuddy.Services;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using System.Collections.Generic;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class BatchServiceTest : DbTestBase
    {
       
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            BatchService batchService = new BatchService();
            Batch batch = new Batch(); 
            batch.BatchId=1;
            batch.BatchTypeValue=1;
            batch.Description="test";
            batch.Name="Test Batch";
            batch.StartDate = DateTime.Now;
            batch.OwnerId=peter.UserId;

            batchService.Create(batch);

            Batch foundBatch = context.Batches.Find(batch.BatchId);

            Assert.IsNotNull(foundBatch);
            Assert.AreEqual(batch.BatchId,  foundBatch.BatchId);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);

            //Now change it
            batch.Name = "Altered Batch";
            BatchService batchService = new BatchService();
            batchService.Update(batch);

            //Get it  and see it changed
            Batch alteredBatch = context.Batches.Find(batch.BatchId);
            Assert.AreEqual("Altered Batch", alteredBatch.Name);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);

            BatchService batchService = new BatchService();
            Batch foundBatch = batchService.Get(batch.BatchId);

            Assert.IsNotNull(foundBatch);
            Assert.AreEqual(batch.BatchId, foundBatch.BatchId);
            Assert.AreEqual(batch.Name, foundBatch.Name);
        }


        [Test]
        public void TestGetNonExistant()
        {
            BatchService batchService = new BatchService();
            Batch foundBatch = batchService.Get(5);

            Assert.IsNull(foundBatch);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);

            //See that the service can find it
            BatchService batchService = new BatchService();
            Batch foundBatch = batchService.Get(batch.BatchId);

            Assert.IsNotNull(foundBatch);
            Assert.AreEqual(batch.BatchId, foundBatch.BatchId);
            Assert.AreEqual(batch.Name, foundBatch.Name);

            //Now delete it and see that it is gone
            batchService.Delete(foundBatch);

            Batch foundBatchDelete = batchService.Get(foundBatch.BatchId);
            Assert.IsNull(foundBatchDelete);
        }

        [Test]
        public void TestAddAction()
        {
            BatchService batchService = new BatchService();

            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);

            BatchAction action = new BatchAction();
            action.ActionDate = DateTime.Now;
            action.ActionId = 1;
            action.Title = "Test Action";
            action.BatchId = batch.BatchId;
            action.Description = "Test";
            action.PerformerId = bilbo.UserId;
            action.Type = ActionType.Additives;

            //Add the action
            batchService.AddAction(batch, action);

            //Verify it is found
            ICollection<BatchAction> actions = 
                context.Batches.Find(batch.BatchId).Actions;

            Assert.IsTrue(actions.Count == 1);
            Assert.IsTrue(actions.Contains(action));
        }


        [Test]
        public void TestAddNote()
        {
            BatchService batchService = new BatchService();

            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);

            BatchNote note = new BatchNote();
            note.AuthorId = bilbo.UserId;
            note.AuthorDate = DateTime.Now;
            note.BatchId = batch.BatchId;
            note.Title = "Test Note";
            note.Text = "One Ring to rule them all, One Ring to find them, One Ring to bring them all and in the darkness bind them.";

            //Add the action
            batchService.AddNote(batch, note);

            //Verify it is found
            ICollection<BatchNote> notes =
                context.Batches.Find(batch.BatchId).Notes;

            Assert.IsTrue(notes.Count == 1);
            Assert.IsTrue(notes.Contains(note));
        }

        [Test]
        public void TestAddMeasurement()
        {
            BatchService batchService = new BatchService();

            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);

            Measurement measurement = new Measurement();
            measurement.BatchId = batch.BatchId;
            measurement.Description = "A measurement of PH.";
            measurement.Measured = "PH";
            measurement.Value = 7.7;
            measurement.MeasurementDate = DateTime.Now;

            //Add the measurement
            batchService.AddMeasurement(batch, measurement);

            //Verify it is found
            ICollection<Measurement> measurements =
                context.Batches.Find(batch.BatchId).Measurements;

            Assert.IsTrue(measurements.Count == 1);
            Assert.IsTrue(measurements.Contains(measurement));
        }

        [Test]
        public void TestGetAllForUser()
        {
            UserProfile gandalf = TestUtils.createUser(context, "Gandalf", "TheGrey");
            UserProfile sauron = TestUtils.createUser(context, "Sauron", "EvilOne");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, gandalf);
            Batch batch2 = TestUtils.createBatch(context, "Wizard Brew", BatchType.Beer, gandalf);

            Batch batch4 = TestUtils.createBatch(context, "Evil Brew", BatchType.Beer, sauron);

            BatchService batchService = new BatchService();
            IEnumerable<Batch> batchesEnumerable = batchService.GetAllForUser(gandalf.UserId);

            int foundCount = 0;
            foreach (Batch foundBatch in batchesEnumerable)
            {
                if (foundBatch.BatchId == batch4.BatchId)
                {
                    Assert.Fail("Batch found for wrong user");
                }

                if (foundBatch.BatchId == batch.BatchId || foundBatch.BatchId == batch2.BatchId)
                {
                    foundCount++;
                }
            }

            Assert.AreEqual(2, foundCount);
        }
       
    }
}
