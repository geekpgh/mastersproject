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

        public static BatchNote createBatchNote(Batch batch, String title, String text, UserProfile user)
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            BatchNote note = new BatchNote();
            note.Batch = batch;
            note.Text = text;
            note.Title = title;
            note.AuthorId = user.UserId;
            note.AuthorDate = DateTime.Now;

            db.BatchNotes.Add(note);
            db.SaveChanges();

            return note;
        }

        public static BatchRating createBatchRating(Batch batch, UserProfile user, int rating, string comment)
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            BatchRating batchRating = new BatchRating();
            batchRating.Batch = batch;
            batchRating.User = user;
            batchRating.Rating = rating;
            batchRating.Comment = comment;

            db.BatchRatings.Add(batchRating);

            db.SaveChanges();

            return batchRating;
        }

        public static BatchComment createBatchComment(Batch batch, UserProfile user, string comment)
        {
            BrewersBuddyContext db = new BrewersBuddyContext();

            BatchComment batchComment = new BatchComment();
            batchComment.Batch = batch;
            batchComment.User = user;
            batchComment.Comment = comment;

            db.BatchComments.Add(batchComment);

            db.SaveChanges();

            return batchComment;
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
