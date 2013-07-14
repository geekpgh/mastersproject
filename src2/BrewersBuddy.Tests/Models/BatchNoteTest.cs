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

            batch.Notes.Remove(note);
            context.SaveChanges();

            Assert.IsFalse(batch.Notes.Contains(note));
        }

        [Test]
        public void TestEditBatchNotes()
        {
            UserProfile jon = TestUtils.createUser(111, "Jon", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(batch, "Test Note", "I am a note!", jon);

            BrewersBuddyContext db = new BrewersBuddyContext();

            var id = note.NoteId;
            var note1 = db.BatchNotes.Find(id);

            Assert.IsTrue(batch.Notes.Contains(note));
            Assert.AreEqual("Test Note", note1.Title);
            Assert.AreEqual("I am a note!", note1.Text);

            note.Title = "Test Title";

            // save changes
            db = new BrewersBuddyContext(); 
            db.Entry(note).State = System.Data.EntityState.Modified;
            db.SaveChanges();

            //clear context and reload
            db = new BrewersBuddyContext(); 
            note1 = db.BatchNotes.Find(id);

            // verify local context is not being used
            note.Text = "test text";

            Assert.AreEqual("Test Title", note1.Title);
            Assert.AreEqual("I am a note!", note1.Text);
        }

        [Test]
        //Test that it isn't truncated if short
        public void TestSummaryLengthShort()
        {
            UserProfile bob = TestUtils.createUser(999, "Bob", "Smith");
            Batch batch = TestUtils.createBatch("Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(batch, "Test Note", "I am a note!", bob);
           
            Assert.AreEqual(note.SummaryText, "I am a note!");
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

            BatchNote note = TestUtils.createBatchNote(batch, "Test Note", longText, bob);

            //The 3 is for the ...
            Assert.True(note.SummaryText.Length == 203);
        }
    }
}
