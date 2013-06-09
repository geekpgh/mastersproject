namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBatchRating : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchRating",
                c => new
                    {
                        BatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BatchId, t.UserId })
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BatchId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchRating", new[] { "UserId" });
            DropIndex("dbo.BatchRating", new[] { "BatchId" });
            DropForeignKey("dbo.BatchRating", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchRating", "BatchId", "dbo.Batch");
            DropTable("dbo.BatchRating");
        }
    }
}
