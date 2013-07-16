using BrewersBuddy.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchService : IBatchService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();
        private BrewersBuddyContext db2 = new BrewersBuddyContext();

        public void AddAction(Batch batch, BatchAction action)
        {
            db.Entry(action).State = EntityState.Added;
            batch.Actions.Add(action);
            db.SaveChanges();
        }

        public void AddNote(Batch batch, BatchNote note)
        {
            db.Entry(note).State = EntityState.Added;
            batch.Notes.Add(note);
            db.SaveChanges();
        }

        public void AddMeasurement(Batch batch, Measurement measurement)
        {
            db.Entry(measurement).State = EntityState.Added;
            batch.Measurements.Add(measurement);
            db.SaveChanges();
        }

/*
        public void AddToCellar(Batch batch, Container container)
        {
            db.Entry(container).State = EntityState.Added;
            batch.Container.Add(container);
            db.SaveChanges();
        }
 */ 
 
        public void Create(Batch @object)
        {
            db.Batches.Add(@object);
            db.SaveChanges();
        }

        public void Delete(Batch @object)
        {
            db.Batches.Remove(@object);
            db.SaveChanges();
        }

        public Batch Get(int id)
        {
            return db.Batches.Find(id);
        }

        public IEnumerable<Batch> GetAllForUser(int userId)
        {
            return from batch in db.Batches
                   where (batch.OwnerId.Equals(userId))
                   select batch;
        }

        public void Update(Batch @object)
        {
            db2.Entry(@object).State = EntityState.Modified;
            db2.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}