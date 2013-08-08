using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchCommentService : IBatchCommentService
    {
        private readonly BrewersBuddyContext db;

        public BatchCommentService(BrewersBuddyContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            db = context;
        }

        public void Create(BatchComment @object)
        {
            db.BatchComments.Add(@object);
            db.SaveChanges();
        }

        public void Delete(BatchComment @object)
        {
            db.BatchComments.Remove(@object);
            db.SaveChanges();
        }

        public BatchComment Get(int id)
        {
            return db.BatchComments.Find(id);
        }

        public IEnumerable<BatchComment> GetAllForBatch(int batchId)
        {
            return db.BatchComments
                .Where(r => r.BatchId == batchId);
        }

        public void Update(BatchComment @object)
        {
            var comment = db.BatchComments.Find(@object.BatchCommentId);
            db.Entry(comment).CurrentValues.SetValues(@object);
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}