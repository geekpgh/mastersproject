using System;
using NUnit.Framework;
using BrewersBuddy.Services;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.TestUtilities;
using System.Collections.Generic;
namespace BrewersBuddy.Tests.Services
{
    [TestFixture]
    public class ContainerServiceTest : DbTestBase
    {
        [Test]
        public void TestCreate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            ContainerService containerService = new ContainerService(context);
            Container container = new Container();
            container.ContainerId = 1;
            container.ContainerTypeValue = 1;
            container.Quantity = 5;
            container.Name = "Test Container";
            container.UnitValue = 1;
            container.OwnerId = peter.UserId;
            container.BatchId = batch.BatchId;

            containerService.Create(container);

            Container foundContainer = context.Containers.Find(container.ContainerId);

            Assert.IsNotNull(foundContainer);
            Assert.AreEqual(container.ContainerId, foundContainer.ContainerId);
        }

        [Test]
        public void TestUpdate()
        {
            UserProfile peter = TestUtils.createUser(context, "peter", "parker");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, peter);
            Container container = TestUtils.createContainer(context, "Test Container", batch, ContainerType.Bottle, peter);

            //Now change it
            container.Name = "Altered Container";
            ContainerService containerService = new ContainerService(context);
            containerService.Update(container);

            //Get it  and see it changed
            Container alteredContainer = context.Containers.Find(container.ContainerId);
            Assert.AreEqual("Altered Container", alteredContainer.Name);
        }

        [Test]
        public void TestGet()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, bilbo);
            Container container = TestUtils.createContainer(context, "Test Container", batch, ContainerType.Bottle, bilbo);

            ContainerService containerService = new ContainerService(context);
            Container foundContainer = containerService.Get(container.ContainerId);

            Assert.IsNotNull(foundContainer);
            Assert.AreEqual(container.ContainerId, foundContainer.ContainerId);
            Assert.AreEqual(container.Name, foundContainer.Name);
        }


        [Test]
        public void TestGetNonExistant()
        {
            ContainerService containerService = new ContainerService(context);
            Container foundContainer = containerService.Get(5);

            Assert.IsNull(foundContainer);
        }

        [Test]
        public void TestDelete()
        {
            UserProfile bilbo = TestUtils.createUser(context, "bilbo", "baggins");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, bilbo);
            Container container = TestUtils.createContainer(context, "Test Container", batch, ContainerType.Bottle, bilbo);

            //See that the service can find it
            ContainerService containerService = new ContainerService(context);
            Container foundContainer = containerService.Get(container.ContainerId);

            Assert.IsNotNull(foundContainer);
            Assert.AreEqual(container.ContainerId, foundContainer.ContainerId);
            Assert.AreEqual(container.Name, foundContainer.Name);

            //Now delete it and see that it is gone
            containerService.Delete(foundContainer);

            Container foundContainerDelete = containerService.Get(foundContainer.ContainerId);
            Assert.IsNull(foundContainerDelete);
        }


        [Test]
        public void TestGetAllForUser()
        {
            UserProfile gandalf = TestUtils.createUser(context, "Gandalf", "TheGrey");
            UserProfile sauron = TestUtils.createUser(context, "Sauron", "EvilOne");
            Batch batch = TestUtils.createBatch(context, "Test Batch", BatchType.Beer, gandalf);
            Batch batch2 = TestUtils.createBatch(context, "Test Batch2", BatchType.Wine, gandalf);
            Container container = TestUtils.createContainer(context, "Test Container", batch, ContainerType.Bottle, gandalf);
            Container container2 = TestUtils.createContainer(context, "Test Container2", batch2, ContainerType.Bottle, gandalf);

            Batch batch3 = TestUtils.createBatch(context, "Test Batch2", BatchType.Wine, sauron);
            Container container3 = TestUtils.createContainer(context, "Test Container", batch3, ContainerType.Bottle, sauron);

            ContainerService containerService = new ContainerService(context);
            IEnumerable<Container> containeresEnumerable = containerService.GetAllForUser(gandalf.UserId);

            int foundCount = 0;
            foreach (Container foundContainer in containeresEnumerable)
            {
                if (foundContainer.ContainerId == container3.ContainerId)
                {
                    Assert.Fail("Container found for wrong user");
                }

                if (foundContainer.ContainerId == container.ContainerId || foundContainer.ContainerId == container2.ContainerId)
                {
                    foundCount++;
                }
            }

            Assert.AreEqual(2, foundCount);
        }
    }
}
