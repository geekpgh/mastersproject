namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class containerunitsenum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Container", "Batch_BatchId", "dbo.Batch");
            DropIndex("dbo.Container", new[] { "Batch_BatchId" });
            RenameColumn(table: "dbo.Container", name: "Batch_BatchId", newName: "BatchId");
            AddColumn("dbo.Container", "UnitValue", c => c.Int(nullable: false));
            AddForeignKey("dbo.Container", "BatchId", "dbo.Batch", "BatchId", cascadeDelete: true);
            CreateIndex("dbo.Container", "BatchId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Container", new[] { "BatchId" });
            DropForeignKey("dbo.Container", "BatchId", "dbo.Batch");
            DropColumn("dbo.Container", "UnitValue");
            RenameColumn(table: "dbo.Container", name: "BatchId", newName: "Batch_BatchId");
            CreateIndex("dbo.Container", "Batch_BatchId");
            AddForeignKey("dbo.Container", "Batch_BatchId", "dbo.Batch", "BatchId");
        }
    }
}
