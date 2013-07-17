namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addauthor : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            CreateIndex("dbo.BatchNote", "AuthorId");
            CreateIndex("dbo.BatchAction", "PerformerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchAction", new[] { "PerformerId" });
            DropIndex("dbo.BatchNote", new[] { "AuthorId" });
            DropForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile");
        }
    }
}
