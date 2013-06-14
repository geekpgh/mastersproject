using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;

namespace BrewersBuddy.Tests.Models
{
    [TestClass]
    public class BatchNoteTest : DbTestBase
    {
        [TestMethod]
        public void TestAddBatchNotes()
        {
            UserProfile jon = TestUtils.createUser(111, "Jon", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(batch, "Test Note", "I am a note!", jon);

            Assert.IsTrue(batch.Notes.Contains(note));
        }

        [TestMethod]
        public void TestRemoveBatchNotes()
        {
            UserProfile jon = TestUtils.createUser(111, "Jon", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(batch, "Test Note", "I am a note!", jon);

            Assert.IsTrue(batch.Notes.Contains(note));

            context.BatchNotes.Remove(note);
            context.SaveChanges();

            Assert.IsFalse(batch.Notes.Contains(note));
        }
    }
}
