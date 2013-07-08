using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class BatchActionService : IBatchActionService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();

        public void Create(BatchAction @object)
        {
            throw new NotImplementedException();
        }

        public void Delete(BatchAction @object)
        {
            db.BatchActions.Remove(@object);
            db.SaveChanges();
        }

        public BatchAction Get(int id)
        {
            return db.BatchActions.Find(id);
        }

        public IEnumerable<BatchAction> GetAllForBatch(int batchId)
        {
            return db.BatchActions.Where(action => action.BatchId == batchId);
        }

        public void Update(BatchAction @object)
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