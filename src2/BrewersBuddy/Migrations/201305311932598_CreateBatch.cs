namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Batch",
                c => new
                    {
                        BatchId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.BatchId)
                .ForeignKey("dbo.UserProfile", t => t.Owner_UserId)
                .Index(t => t.Owner_UserId);
            
            AddColumn("dbo.UserProfile", "Batch_BatchId", c => c.Int());
            AddForeignKey("dbo.UserProfile", "Batch_BatchId", "dbo.Batch", "BatchId");
            CreateIndex("dbo.UserProfile", "Batch_BatchId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserProfile", new[] { "Batch_BatchId" });
            DropIndex("dbo.Batch", new[] { "Owner_UserId" });
            DropForeignKey("dbo.UserProfile", "Batch_BatchId", "dbo.Batch");
            DropForeignKey("dbo.Batch", "Owner_UserId", "dbo.UserProfile");
            DropColumn("dbo.UserProfile", "Batch_BatchId");
            DropTable("dbo.Batch");
        }
    }
}
