namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomethingMissing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "UserProfile_UserId", c => c.Int());
            AddForeignKey("dbo.UserProfile", "UserProfile_UserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.UserProfile", "UserProfile_UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserProfile", new[] { "UserProfile_UserId" });
            DropForeignKey("dbo.UserProfile", "UserProfile_UserId", "dbo.UserProfile");
            DropColumn("dbo.UserProfile", "UserProfile_UserId");
        }
    }
}
