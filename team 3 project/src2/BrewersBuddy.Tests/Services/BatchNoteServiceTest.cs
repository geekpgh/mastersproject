using System;
using NUnit.Framework;
using BrewersBuddy.Models;
using BrewersBuddy.Services;
using BrewersBuddy.Tests.TestUtilities;
using System.Collections.Generic;

namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class BatchNoteServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, peter);
            BatchNoteService batchNoteService = new BatchNoteService(context);
            BatchNote note = new BatchNote();
            note.NoteId = 1;
            note.Text = "test";
            note.Title = "Test Note";
            note.AuthorDate = DateTime.Now;
            note.AuthorId = peter.UserId;
            note.BatchId = batch.BatchId;

            batchNoteService.Create(note);

            BatchNote foundNote = context.BatchNotes.Find(note.NoteId);

            Assert.IsNotNull(foundNote);
            Assert.AreEqual(note.NoteId, foundNote.NoteId);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "This is a test", bilbo);

            //See that the service can find it
            BatchNoteService noteService = new BatchNoteService(context);
            BatchNote foundNote = noteService.Get(note.NoteId);

            Assert.IsNotNull(foundNote);
            Assert.AreEqual(note.NoteId, foundNote.NoteId);
            Assert.AreEqual(note.Title, foundNote.Title);

            //Now delete it and see that it is gone
            noteService.Delete(foundNote);

            BatchNote foundNoteDelete = noteService.Get(foundNote.BatchId);
            Assert.IsNull(foundNoteDelete);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Hobbit Brew", BatchType.Beer, bilbo);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "This is a test", bilbo);

            BatchNoteService noteService = new BatchNoteService(context);
            BatchNote foundNote = noteService.Get(note.NoteId);

            Assert.IsNotNull(foundNote);
            Assert.AreEqual(note.NoteId, foundNote.NoteId);
            Assert.AreEqual(note.Title, foundNote.Title);
        }


        [Test]
        public void TestGetNonExistant()
        {
            BatchNoteService noteService = new BatchNoteService(context);
            BatchNote foundNote = noteService.Get(5);

            Assert.IsNull(foundNote);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "This is a test", peter);

            //Now change it
            note.Title = "Altered Note";
            BatchNoteService noteService = new BatchNoteService(context);
            noteService.Update(note);

            //Get it  and see it changed
            BatchNote alteredNote = context.BatchNotes.Find(note.NoteId);
            Assert.AreEqual("Altered Note", alteredNote.Title);
        }

        [Test]
        public void TestGetAllForBatch()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            BatchNote note = TestUtils.createBatchNote(context, batch, "Test Note", "This is a test", peter);
            BatchNote note2 = TestUtils.createBatchNote(context, batch, "Test Note2", "This is a test 2", peter);

            Batch batch2 = TestUtils.createBatch(context, "Wrong Batch", BatchType.Beer, peter);
            BatchNote note3 = TestUtils.createBatchNote(context, batch2, "Wrong Note", "This is a test", peter);

            BatchNoteService noteService = new BatchNoteService(context);
            IEnumerable<BatchNote> notesEnumerable = noteService.GetAllForBatch(batch.BatchId);

            int foundCount = 0;
            foreach (BatchNote foundNote in notesEnumerable)
            {
                if (foundNote.NoteId == note3.NoteId)
                {
                    Assert.Fail("Note found for wrong user");
                }

                if (foundNote.NoteId == note.NoteId || foundNote.NoteId == note2.NoteId)
                {
                    foundCount++;
                }
            }

            Assert.AreEqual(2, foundCount);
        }
    }
}
