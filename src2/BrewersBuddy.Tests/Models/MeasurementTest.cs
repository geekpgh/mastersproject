using System;
using NUnit.Framework;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class MeasurementTest : DbTestBase
    {
        [Test]
        public void TestAddMeasurement()
        {
            UserProfile jon = TestUtils.createUser(context, "Jon", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Beer, jon);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "Taking weekly PH measurement", "PH", 7.0);

            Assert.IsTrue(batch.Measurements.Contains(measurment));
        }

        [Test]
        //Test that it isn't truncated if short
        public void TestSummaryLengthShort()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            Assert.AreEqual(measurment.SummaryText, "measurement");
        }

        [Test]
        //test that it is truncated
        public void TestSummaryTruncate()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            string longText = "This is a very very very long string it is very long. This is a very very very long string it is very long. ";

            while (longText.Length < 200)
            {
                longText += "This is a very very very long string it is very long. This is a very very very long string it is very long. ";
            }

            //Make sure the string is setup correctly
            Assert.True(longText.Length >= 200);

            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", longText, "PH", 7.0);

            //The 3 is for the ...
			Assert.True(measurment.SummaryText.Length == 203);
        }

        [Test]
        public void TestCanViewOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            //Verify the owner can view
            Assert.IsTrue(measurment.CanView(bob.UserId));
        }

        [Test]
        public void TestCanEditOwned()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            //Verify the collaborator can edit
            Assert.IsTrue(measurment.CanEdit(bob.UserId));
        }

        [Test]
        public void TestCanViewCollaborator()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(measurment.CanView(fred.UserId));
        }

        [Test]
        public void TestCanEditCollaborator()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            batch.Collaborators.Add(fred);
            context.SaveChanges();

            Assert.IsTrue(measurment.CanEdit(fred.UserId));
        }

        [Test]
        public void TestCanViewFriend()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            bob.Friends.Add(fred);
            context.SaveChanges();

            //Verify the collaborator can view
            Assert.IsTrue(measurment.CanView(fred.UserId));
        }

        [Test]
        public void TestCannotEditFriend()
        {
            UserProfile fred = TestUtils.createUser(context, "Fred", "Smith");
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Measurement measurment = TestUtils.createMeasurement(context, batch, "Test Measurement", "measurement", "PH", 7.0);

            bob.Friends.Add(fred);
            context.SaveChanges();

            //Verify the owner can view
            Assert.IsFalse(measurment.CanEdit(fred.UserId));
        }
    }
}
