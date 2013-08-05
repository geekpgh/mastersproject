namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<BrewersBuddy.Models.BrewersBuddyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BrewersBuddy.Models.BrewersBuddyContext context)
        {
            if(WebSecurity.Initialized == false)
            {
                WebSecurity.InitializeDatabaseConnection(
                  connectionStringName: "DefaultConnection",
                  userTableName: "UserProfile",
                  userIdColumn: "UserID",
                  userNameColumn: "UserName",
                  autoCreateTables: true);
            }
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
