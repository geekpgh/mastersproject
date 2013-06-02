using System;
using System.Collections.Generic;
using BrewersBuddy.Models;

namespace BrewersBuddy.Tests.TestUtilities
{
    class TestUtils
    {
        public static UserProfile createUser(int userId, String firstName, String lastName)
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            UserProfile user = new UserProfile();
            user.UserId = userId;
            user.FirstName = firstName;
            user.LastName = lastName;

            //TODO should we register them somehow??
            db.UserProfiles.Add(user);
            db.SaveChanges();

            return user;
        }

        public static Batch createBatch(String name, BatchType type, UserProfile owner)
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            Batch batch = new Batch();
            batch.Name = name;
            batch.Type = type;
            batch.OwnerId = owner.UserId;
            batch.StartDate = DateTime.Now;

            db.Batches.Add(batch);

            db.SaveChanges();

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
