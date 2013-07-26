namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBatchCollaborators : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProfile", "Batch_BatchId", "dbo.Batch");
            DropIndex("dbo.UserProfile", new[] { "Batch_BatchId" });
            CreateTable(
                "dbo.BatchCollaborator",
                c => new
                    {
                        BatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BatchId, t.UserId })
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BatchId)
                .Index(t => t.UserId);
            
            DropColumn("dbo.UserProfile", "Batch_BatchId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfile", "Batch_BatchId", c => c.Int());
            DropIndex("dbo.BatchCollaborator", new[] { "UserId" });
            DropIndex("dbo.BatchCollaborator", new[] { "BatchId" });
            DropForeignKey("dbo.BatchCollaborator", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchCollaborator", "BatchId", "dbo.Batch");
            DropTable("dbo.BatchCollaborator");
            CreateIndex("dbo.UserProfile", "Batch_BatchId");
            AddForeignKey("dbo.UserProfile", "Batch_BatchId", "dbo.Batch", "BatchId");
        }
    }
}
