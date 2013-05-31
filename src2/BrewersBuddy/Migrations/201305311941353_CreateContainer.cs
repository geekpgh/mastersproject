namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateContainer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Container",
                c => new
                    {
                        ContainerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Volume = c.Double(nullable: false),
                        Batch_BatchId = c.Int(),
                        Cellar_CellarId = c.Int(),
                    })
                .PrimaryKey(t => t.ContainerId)
                .ForeignKey("dbo.Batch", t => t.Batch_BatchId)
                .ForeignKey("dbo.Cellar", t => t.Cellar_CellarId)
                .Index(t => t.Batch_BatchId)
                .Index(t => t.Cellar_CellarId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Container", new[] { "Cellar_CellarId" });
            DropIndex("dbo.Container", new[] { "Batch_BatchId" });
            DropForeignKey("dbo.Container", "Cellar_CellarId", "dbo.Cellar");
            DropForeignKey("dbo.Container", "Batch_BatchId", "dbo.Batch");
            DropTable("dbo.Container");
        }
    }
}
