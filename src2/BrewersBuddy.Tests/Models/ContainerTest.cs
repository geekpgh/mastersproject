using System;
using System.Data.Entity;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using NUnit.Framework;

namespace BrewersBuddy.Tests.Models
{
    [TestFixture]
    public class ContainerTest : DbTestBase
    {
        [Test]
        public void TestCreateContainer()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);

            DbSet<Container> containers = context.Containers;
            Container foundContainer = containers.Find(container.ContainerId);

            //Verify it was properly created
            Assert.AreEqual(container.ContainerId, foundContainer.ContainerId);


        }

        [Test]
        public void TestCanRetrieveAssociatedBatch()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Container container = TestUtils.createContainer(context, batch, ContainerType.Bottle, bob);

            DbSet<Container> containers = context.Containers;
            Container foundContainer = containers.Find(container.ContainerId);

            Assert.IsNotNull(foundContainer.Batch);
            Assert.AreEqual(batch.BatchId, foundContainer.Batch.BatchId);
        }

        [Test]
        public void TestContainerMayHaveOptionalInformation()
        {
            UserProfile bob = TestUtils.createUser(context, "Bob", "Smith");
            Batch batch = TestUtils.createBatch(context, "Test", BatchType.Mead, bob);
            Container container = new Container();

            container.Batch = batch;
            container.Type = ContainerType.Bottle;
            container.Name = "Test Name";
            container.Quantity = 25;
            container.Units = ContainerVolumeUnits.Ounce;
            container.Volume = 750;

            context.Containers.Add(container);
            context.SaveChanges();

            DbSet<Container> containers = context.Containers;
            Container foundContainer = containers.Find(container.ContainerId);

            Assert.AreEqual(ContainerType.Bottle, foundContainer.Type);
            Assert.AreEqual("Test Name", foundContainer.Name);
            Assert.AreEqual(25, foundContainer.Quantity);
            Assert.AreEqual(ContainerVolumeUnits.Ounce, foundContainer.Units);
            Assert.AreEqual(750.0, foundContainer.Volume);
        }


    }
}
