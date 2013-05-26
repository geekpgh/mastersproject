using BrewersBuddy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrewersBuddy.Tests.Utils
{
    class TestUtils
    {
        public static UserProfile createUser(int userId, String firstName, String lastName)
        {
            UserProfile user = new UserProfile();
            user.UserId = userId;
            user.FirstName = firstName;
            user.LastName = lastName;

            //TODO should we register them somehow??

            return user;
        }

        public static Batch createBatch(String name, BatchType type, UserProfile owner)
        {
            BatchDBContext db = new BatchDBContext();

            Batch batch = new Batch();
            batch.Name = name;
            batch.Type = type;
            batch.Owner = owner;

            db.Batches.Add(batch);

            return batch;
        }


        public static ICollection<Batch> createBatches(int number, UserProfile owner)
        {
            List<Batch> batches = new List<Batch>();

            for (int i = 1; i <= number; i++)
            {
                batches.Add(createBatch("batch" + i, BatchType.Beer, owner));
            }

            return batches;
        }

    }
}
