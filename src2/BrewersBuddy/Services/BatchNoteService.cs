using BrewersBuddy.Models;
using System;
using System.Data;

namespace BrewersBuddy.Services
{
    public class BatchNoteService : IBatchNoteService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        public void Create(BatchNote @object)
        {
            throw new NotImplementedException();
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

        public void Update(BatchNote @object)
        {
            db.Entry(@object).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}