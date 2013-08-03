using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchRatingService : IBatchRatingService
    {
        private readonly BrewersBuddyContext db;

        public BatchRatingService(BrewersBuddyContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            db = context;
        }

        public void Create(BatchRating @object)
        {
            db.BatchRatings.Add(@object);
            db.SaveChanges();
        }

        public void Delete(BatchRating @object)
        {
            db.BatchRatings.Remove(@object);
            db.SaveChanges();
        }

        public BatchRating Get(int id)
        {
            throw new NotSupportedException("Cannot get by id");
        }

        public IEnumerable<BatchRating> GetAllForBatch(int batchId)
        {
            return db.BatchRatings
                .Where(r => r.BatchId == batchId);
        }

        public BatchRating GetUserRatingForBatch(int batchId, int userId)
        {
            return db.BatchRatings
                .Where(rating => rating.BatchId == batchId && rating.UserId == userId)
                .FirstOrDefault();
        }

        public void Update(BatchRating @object)
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