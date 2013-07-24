namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixcascades : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BatchRating", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile");
            DropIndex("dbo.BatchRating", new[] { "UserId" });
            DropIndex("dbo.BatchComment", new[] { "UserId" });
            DropIndex("dbo.BatchNote", new[] { "AuthorId" });
            DropIndex("dbo.BatchAction", new[] { "PerformerId" });
            AddColumn("dbo.BatchRating", "UserProfile_UserId", c => c.Int());
            AddColumn("dbo.BatchComment", "UserProfile_UserId", c => c.Int());
            AddForeignKey("dbo.BatchRating", "UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchRating", "UserProfile_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchComment", "UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchComment", "UserProfile_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.BatchRating", "UserId");
            CreateIndex("dbo.BatchRating", "UserProfile_UserId");
            CreateIndex("dbo.BatchComment", "UserId");
            CreateIndex("dbo.BatchComment", "UserProfile_UserId");
            CreateIndex("dbo.BatchNote", "AuthorId");
            CreateIndex("dbo.BatchAction", "PerformerId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchAction", new[] { "PerformerId" });
            DropIndex("dbo.BatchNote", new[] { "AuthorId" });
            DropIndex("dbo.BatchComment", new[] { "UserProfile_UserId" });
            DropIndex("dbo.BatchComment", new[] { "UserId" });
            DropIndex("dbo.BatchRating", new[] { "UserProfile_UserId" });
            DropIndex("dbo.BatchRating", new[] { "UserId" });
            DropForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchRating", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchRating", "UserId", "dbo.UserProfile");
            DropColumn("dbo.BatchComment", "UserProfile_UserId");
            DropColumn("dbo.BatchRating", "UserProfile_UserId");
            CreateIndex("dbo.BatchAction", "PerformerId");
            CreateIndex("dbo.BatchNote", "AuthorId");
            CreateIndex("dbo.BatchComment", "UserId");
            CreateIndex("dbo.BatchRating", "UserId");
            AddForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.BatchComment", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.BatchRating", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
        }
    }
}
