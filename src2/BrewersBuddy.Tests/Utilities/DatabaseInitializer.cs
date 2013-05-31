using System;
using System.Data.Entity;
using BrewersBuddy.Models;
using WebMatrix.WebData;

namespace BrewersBuddy.Tests.Utilities
{
    class DatabaseInitializer : DropCreateDatabaseAlways<BrewersBuddyContext>
    {
        protected override void Seed(BrewersBuddyContext context)
        {
            base.Seed(context);

            WebSecurity.InitializeDatabaseConnection(
                  connectionStringName: "DefaultConnection",
                  userTableName: "UserProfile",
                  userIdColumn: "UserID",
                  userNameColumn: "UserName",
                  autoCreateTables: true);
        }
    }
}
