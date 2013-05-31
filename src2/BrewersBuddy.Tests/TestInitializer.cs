using System;
using System.Data.Entity;
using BrewersBuddy.Models;
using BrewersBuddy.Tests.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrewersBuddy.Tests
{
    [TestClass]
    public class TestInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            Database.SetInitializer(new DatabaseInitializer());

            using (var dbContext = new BrewersBuddyContext())
            {
                dbContext.Database.Initialize(true);
            }
        }
    }
}
