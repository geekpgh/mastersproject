namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class switchToMembershipUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Batch", "Owner_UserId", "dbo.UserProfile");
            DropIndex("dbo.Batch", new[] { "Owner_UserId" });
            AddColumn("dbo.Batch", "Owner_Email", c => c.String());
            AddColumn("dbo.Batch", "Owner_Comment", c => c.String());
            AddColumn("dbo.Batch", "Owner_IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Batch", "Owner_LastLoginDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Batch", "Owner_LastActivityDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Batch", "Owner_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Batch", "Owner_UserId", c => c.Int());
            DropColumn("dbo.Batch", "Owner_LastActivityDate");
            DropColumn("dbo.Batch", "Owner_LastLoginDate");
            DropColumn("dbo.Batch", "Owner_IsApproved");
            DropColumn("dbo.Batch", "Owner_Comment");
            DropColumn("dbo.Batch", "Owner_Email");
            CreateIndex("dbo.Batch", "Owner_UserId");
            AddForeignKey("dbo.Batch", "Owner_UserId", "dbo.UserProfile", "UserId");
        }
    }
}
