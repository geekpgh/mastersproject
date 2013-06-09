namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBatchComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchComment",
                c => new
                    {
                        BatchCommentId = c.Int(nullable: false, identity: true),
                        BatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.BatchCommentId)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BatchId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchComment", new[] { "UserId" });
            DropIndex("dbo.BatchComment", new[] { "BatchId" });
            DropForeignKey("dbo.BatchComment", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "BatchId", "dbo.Batch");
            DropTable("dbo.BatchComment");
        }
    }
}
