using System;
using System.Collections.Generic;
using BrewersBuddy.Models;

namespace BrewersBuddy.Tests.TestUtilities
{
    class TestUtils
    {
        public static UserProfile createUser(BrewersBuddyContext db, String firstName, String lastName)
        {
            UserProfile user = new UserProfile();
            //user.UserId = userId;
            user.FirstName = firstName;
            user.LastName = lastName;

            //TODO should we register them somehow??
            db.UserProfiles.Add(user);
            db.SaveChanges();

            return user;
        }

        public static Batch createBatch(BrewersBuddyContext db, String name, BatchType type, UserProfile owner)
        {
            Batch batch = new Batch();
            batch.Name = name;
            batch.Type = type;
            batch.Owner = owner;
            batch.StartDate = DateTime.Now;

            db.Batches.Add(batch);
            
            db.SaveChanges();

            return batch;
        }

        public static BatchNote createBatchNote(BrewersBuddyContext db, Batch batch, String title, String text, UserProfile user)
        {
            BatchNote note = new BatchNote();
            note.Batch = batch;
            note.Text = text;
            note.Title = title;
            note.Author = user;
            note.AuthorDate = DateTime.Now;

            db.BatchNotes.Add(note);
            db.SaveChanges();

            return note;
        }

        public static Measurement createMeasurement(BrewersBuddyContext db, Batch batch, String name, String description, String measured, Double value)
        {
            Measurement measurment = new Measurement();
            measurment.Batch = batch;
            measurment.Name = name;
            measurment.Description = description;
            measurment.Measured = measured;
            measurment.MeasurementDate = DateTime.Now;
            measurment.Value = value;

            db.Measurements.Add(measurment);
            db.SaveChanges();

            return measurment;
        }

        public static BatchRating createBatchRating(BrewersBuddyContext db, Batch batch, UserProfile user, int rating, string comment)
        {
            BatchRating batchRating = new BatchRating();
            batchRating.Batch = batch;
            batchRating.User = user;
            batchRating.Rating = rating;
            batchRating.Comment = comment;

            db.BatchRatings.Add(batchRating);

            db.SaveChanges();

            return batchRating;
        }

        public static BatchComment createBatchComment(BrewersBuddyContext db, Batch batch, UserProfile user, string comment)
        {
            BatchComment batchComment = new BatchComment();
            batchComment.Batch = batch;
            batchComment.User = user;
            batchComment.Comment = comment;

            db.BatchComments.Add(batchComment);

            db.SaveChanges();

            return batchComment;
        }

        public static BatchAction createBatchAction(BrewersBuddyContext db, Batch batch, UserProfile user, string title, string description, 
                                                    ActionType type)
        {
            BatchAction batchAction = new BatchAction();
            batchAction.Batch = batch;
            batchAction.PerformerId = user.UserId;
            batchAction.Performer = user;
            batchAction.Title = title;
            batchAction.Description = description;
            batchAction.Type = type;
            batchAction.ActionDate = DateTime.Now;

            db.BatchActions.Add(batchAction);
            db.SaveChanges();

            return batchAction;
        }

        public static Container createContainer(BrewersBuddyContext db, Batch batch, ContainerType type, UserProfile user)
        {
            Container container = new Container();
            container.Batch = batch;
            container.Type = type;
            container.OwnerId = user.UserId;

            db.Containers.Add(container);

            db.SaveChanges();

            return container;
        }

        public static Friend createFriend(BrewersBuddyContext db, UserProfile friend, UserProfile user)
        {
            Friend newFriend = new Friend();
            newFriend.UserId = user.UserId;
            newFriend.FriendUserId = friend.UserId;
            newFriend.User = user;

            db.Friends.Add(newFriend);
            db.SaveChanges();

            return newFriend;
        }

        public static Recipe createRecipe(BrewersBuddyContext db, String name, UserProfile owner)
        {
            Recipe recipe = new Recipe();
            recipe.Name = name;
            recipe.AddDate = DateTime.Now;
            recipe.Costs = "1.00";
            recipe.Description = "Easy Recipe";
            recipe.Prep = "Buy the booze";
            recipe.Process = "Make the booze";
            recipe.Finishing = "Takes Forever";
            
            recipe.OwnerId = owner.UserId;

            db.Recipes.Add(recipe);

            db.SaveChanges();

            return recipe;
        }
        
        public static ICollection<Batch> createBatches(BrewersBuddyContext db, int number, UserProfile owner)
        {
            List<Batch> batches = new List<Batch>();

            for (int i = 1; i <= number; i++)
            {
                batches.Add(createBatch(db, "batch" + i, BatchType.Beer, owner));
            }

            return batches;
        }


    }
}
