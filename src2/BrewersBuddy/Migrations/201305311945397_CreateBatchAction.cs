namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBatchAction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchAction",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ActionDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Batch_BatchId = c.Int(),
                        Performer_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Batch", t => t.Batch_BatchId)
                .ForeignKey("dbo.UserProfile", t => t.Performer_UserId)
                .Index(t => t.Batch_BatchId)
                .Index(t => t.Performer_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchAction", new[] { "Performer_UserId" });
            DropIndex("dbo.BatchAction", new[] { "Batch_BatchId" });
            DropForeignKey("dbo.BatchAction", "Performer_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchAction", "Batch_BatchId", "dbo.Batch");
            DropTable("dbo.BatchAction");
        }
    }
}
