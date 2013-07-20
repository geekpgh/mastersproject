using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class ContainerService : IContainerService
    {
        private BrewersBuddyContext db = new BrewersBuddyContext();
        private BrewersBuddyContext db2 = new BrewersBuddyContext();

        public void Create(Container @object)
        {
            db.Containers.Add(@object);
            db.SaveChanges();
        }

        public void Delete(Container @object)
        {
            db.Containers.Remove(@object);
            db.SaveChanges();
        }

        public Container Get(int id)
        {
            return db.Containers.Find(id);
        }

        public IEnumerable<Container> GetAllForUser(int userId)
        {
            return db.Containers.Where(container => container.OwnerId == userId);
        }

        public void Update(Container @object)
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