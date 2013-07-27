namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixsomething : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BatchCollaborator", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchCollaborator", "UserId", "dbo.UserProfile");
            DropIndex("dbo.BatchCollaborator", new[] { "BatchId" });
            DropIndex("dbo.BatchCollaborator", new[] { "UserId" });
            AddForeignKey("dbo.BatchCollaborator", "BatchId", "dbo.Batch", "BatchId");
            AddForeignKey("dbo.BatchCollaborator", "UserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.BatchCollaborator", "BatchId");
            CreateIndex("dbo.BatchCollaborator", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchCollaborator", new[] { "UserId" });
            DropIndex("dbo.BatchCollaborator", new[] { "BatchId" });
            DropForeignKey("dbo.BatchCollaborator", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchCollaborator", "BatchId", "dbo.Batch");
            CreateIndex("dbo.BatchCollaborator", "UserId");
            CreateIndex("dbo.BatchCollaborator", "BatchId");
            AddForeignKey("dbo.BatchCollaborator", "UserId", "dbo.UserProfile", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.BatchCollaborator", "BatchId", "dbo.Batch", "BatchId", cascadeDelete: true);
        }
    }
}
