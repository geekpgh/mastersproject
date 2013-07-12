using BrewersBuddy.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchCommentSerice : IBatchCommentService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

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