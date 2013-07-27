namespace BrewersBuddy.Migrations
{
    using System.Data.Entity.Migrations;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<BrewersBuddy.Models.BrewersBuddyContext>
    {
        public Configuration()
        {
			AutomaticMigrationsEnabled = false;
			//AutomaticMigrationsEnabled = true;
			AutomaticMigrationDataLossAllowed = true;
		}

        protected override void Seed(BrewersBuddy.Models.BrewersBuddyContext context)
        {
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
            SeedMembership();
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection(
                  connectionStringName: "DefaultConnection",
                  userTableName: "UserProfile",
                  userIdColumn: "UserID",
                  userNameColumn: "UserName",
                  autoCreateTables: true);
        }
    }
}
