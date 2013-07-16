namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixForPendingError : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Batch", "OwnerId", "dbo.UserProfile", "UserId", cascadeDelete: false);
            CreateIndex("dbo.Batch", "OwnerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Batch", new[] { "OwnerId" });
            DropForeignKey("dbo.Batch", "OwnerId", "dbo.UserProfile");
        }
    }
}
