using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BrewersBuddy.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BrewersBuddyContext db;

        public ContainerService(BrewersBuddyContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            db = context;
        }

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
            var container = db.Containers.Find(@object.ContainerId);
            db.Entry(container).CurrentValues.SetValues(@object);
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}