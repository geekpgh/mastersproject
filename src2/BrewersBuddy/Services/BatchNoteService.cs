using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchNoteService : IBatchNoteService
    {
        private readonly BrewersBuddyContext db;

        public BatchNoteService(BrewersBuddyContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            db = context;
        }

        public void Create(BatchNote @object)
        {
            db.BatchNotes.Add(@object);
            db.SaveChanges();
        }

        public void Delete(BatchNote @object)
        {
            db.BatchNotes.Remove(@object);
            db.SaveChanges();
        }

        public BatchNote Get(int id)
        {
            return db.BatchNotes.Find(id);
        }

        public IEnumerable<BatchNote> GetAllForBatch(int batchId)
        {
            return db.BatchNotes.Where(note => note.BatchId == batchId);
        }

        public void Update(BatchNote @object)
        {
            var note = db.BatchNotes.Find(@object.NoteId);
            db.Entry(note).CurrentValues.SetValues(@object);
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}