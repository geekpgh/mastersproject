using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly BrewersBuddyContext db;

        public MeasurementService(BrewersBuddyContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            db = context;
        }

        public void Create(Measurement @object)
        {
            db.Measurements.Add(@object);
            db.SaveChanges();
        }

        public void Delete(Measurement @object)
        {
            db.Measurements.Remove(@object);
            db.SaveChanges();
        }

        public Measurement Get(int id)
        {
            return db.Measurements.Find(id);
        }

        public IEnumerable<Measurement> GetAllForBatch(int batchId)
        {
            return db.Measurements.Where(note => note.BatchId == batchId);
        }

        public void Update(Measurement @object)
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