using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchRatingService : IBatchRatingService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        public void Create(BatchRating @object)
        {
            throw new NotImplementedException();
        }

        public void Delete(BatchRating @object)
        {
            db.BatchRatings.Remove(@object);
            db.SaveChanges();
        }

        public BatchRating Get(int id)
        {
            return db.BatchRatings.Find(id);
        }

        public IEnumerable<BatchRating> GetAllForBatch(int batchId)
        {
            return db.BatchRatings
                .Where(r => r.BatchId == batchId);
        }

        public void Update(BatchRating @object)
        {
            db.Entry(@object).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}