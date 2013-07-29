namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipecontainerowner : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Recipe", "OwnerId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.Recipe", "OwnerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Recipe", new[] { "OwnerId" });
            DropForeignKey("dbo.Recipe", "OwnerId", "dbo.UserProfile");
        }
    }
}
