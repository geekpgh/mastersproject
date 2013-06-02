namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class switchtoUserId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BatchNote", "Author_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchAction", "Performer_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Cellar", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.BatchNote", new[] { "Author_UserId" });
            DropIndex("dbo.BatchAction", new[] { "Performer_UserId" });
            DropIndex("dbo.Cellar", new[] { "Owner_UserId" });
            AddColumn("dbo.Batch", "OwnerId", c => c.Int(nullable: false));
            AddColumn("dbo.BatchNote", "AuthorId", c => c.Int(nullable: false));
            AddColumn("dbo.BatchAction", "PerformerId", c => c.Int(nullable: false));
            AddColumn("dbo.Cellar", "OwnerId", c => c.Int(nullable: false));
            DropColumn("dbo.Batch", "Owner_Email");
            DropColumn("dbo.Batch", "Owner_Comment");
            DropColumn("dbo.Batch", "Owner_IsApproved");
            DropColumn("dbo.Batch", "Owner_LastLoginDate");
            DropColumn("dbo.Batch", "Owner_LastActivityDate");
            DropColumn("dbo.BatchNote", "Author_UserId");
            DropColumn("dbo.BatchAction", "Performer_UserId");
            DropColumn("dbo.Cellar", "Owner_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cellar", "Owner_UserId", c => c.Int());
            AddColumn("dbo.BatchAction", "Performer_UserId", c => c.Int());
            AddColumn("dbo.BatchNote", "Author_UserId", c => c.Int());
            AddColumn("dbo.Batch", "Owner_LastActivityDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Batch", "Owner_LastLoginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Batch", "Owner_IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Batch", "Owner_Comment", c => c.String());
            AddColumn("dbo.Batch", "Owner_Email", c => c.String());
            DropColumn("dbo.Cellar", "OwnerId");
            DropColumn("dbo.BatchAction", "PerformerId");
            DropColumn("dbo.BatchNote", "AuthorId");
            DropColumn("dbo.Batch", "OwnerId");
            CreateIndex("dbo.Cellar", "Owner_UserId");
            CreateIndex("dbo.BatchAction", "Performer_UserId");
            CreateIndex("dbo.BatchNote", "Author_UserId");
            AddForeignKey("dbo.Cellar", "Owner_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchAction", "Performer_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.BatchNote", "Author_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
