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
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", jon);

            Assert.IsTrue(batch.Notes.Contains(note));
        }

        [Test]
        public void TestRemoveBatchNotes()
        {
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", jon);

            Assert.IsTrue(batch.Notes.Contains(note));

            batch.Notes.Remove(note);
            context.SaveChanges();

            Assert.IsFalse(batch.Notes.Contains(note));
        }

        [Test]
        public void TestEditBatchNotes()
        {
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", jon);

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
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);
           
            Assert.AreEqual(note.SummaryText, "I am a note!");
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

            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", longText, bob);

            //The 3 is for the ...
            Assert.True(note.SummaryText.Length == 203);
        }

        [Test]
        public void TestCanViewOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);

            //Verify the owner can view
            Assert.IsTrue(note.CanView(bob.UserId));
        }

        [Test]
        public void TestCanEditOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);

            //Verify the collaborator can edit
            Assert.IsTrue(note.CanEdit(bob.UserId));
        }

        [Test]
        public void TestCanViewCollaborator()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(note.CanView(fred.UserId));
        }

        [Test]
        public void TestCanEditCollaborator()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            Assert.IsTrue(note.CanEdit(fred.UserId));
        }

        [Test]
        public void TestCanViewFriend()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);

			Friend newFriend = new Friend();
			newFriend.UserId = bob.UserId;
			newFriend.FriendUserId = fred.UserId;
			newFriend.User = bob;

			bob.Friends.Add(newFriend);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(note.CanView(fred.UserId));
        }

        [Test]
        public void TestCannotEditFriend()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "I am a note!", bob);

			Friend newFriend = new Friend();
			newFriend.UserId = bob.UserId;
			newFriend.FriendUserId = fred.UserId;
			newFriend.User = bob;

			bob.Friends.Add(newFriend);
            context.SaveChanges();

            //Verify the owner can view
            Assert.IsFalse(note.CanEdit(fred.UserId));
        }
    }
}
