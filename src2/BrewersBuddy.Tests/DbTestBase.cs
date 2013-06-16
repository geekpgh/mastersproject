using BrewersBuddy.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WebMatrix.WebData;

namespace BrewersBuddy.Tests
{
    public class DbTestBase
    {
        protected BrewersBuddyContext context;

        [SetUp]
        public virtual void TestInitialize()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            AppDomain.CurrentDomain.SetData("DataDirectory", path);
            Database.SetInitializer(new DatabaseInitializer());

            context = new BrewersBuddyContext();
            context.Database.Initialize(true);

            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection(
                  connectionStringName: "DefaultConnection",
                  userTableName: "UserProfile",
                  userIdColumn: "UserID",
                  userNameColumn: "UserName",
                  autoCreateTables: true);
            }
        }

        [TearDown]
        public virtual void TestCleanup()
        {
            context.Dispose();
        }
    }
}
