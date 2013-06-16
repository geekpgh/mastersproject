using System;
using NUnit.Framework;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class BatchNoteTest : DbTestBase
    {
        [Test]
        public void TestAddBatchNotes()
        {
            UserProfile jon = TestUtils.createUser(111, "Jon", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(batch, "Test Note", "I am a note!", jon);

            Assert.IsTrue(batch.Notes.Contains(note));
        }

        [Test]
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
