namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateForeignKeyRelationsForRepositoryChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Measurement", "Batch_BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchNote", "Batch_BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchAction", "Batch_BatchId", "dbo.Batch");
            DropIndex("dbo.Measurement", new[] { "Batch_BatchId" });
            DropIndex("dbo.BatchNote", new[] { "Batch_BatchId" });
            DropIndex("dbo.BatchAction", new[] { "Batch_BatchId" });
            RenameColumn(table: "dbo.Measurement", name: "Batch_BatchId", newName: "BatchId");
            RenameColumn(table: "dbo.BatchNote", name: "Batch_BatchId", newName: "BatchId");
            RenameColumn(table: "dbo.BatchAction", name: "Batch_BatchId", newName: "BatchId");
            AddForeignKey("dbo.Measurement", "BatchId", "dbo.Batch", "BatchId", cascadeDelete: true);
            AddForeignKey("dbo.BatchNote", "BatchId", "dbo.Batch", "BatchId", cascadeDelete: true);
            AddForeignKey("dbo.BatchAction", "BatchId", "dbo.Batch", "BatchId", cascadeDelete: true);
            CreateIndex("dbo.Measurement", "BatchId");
            CreateIndex("dbo.BatchNote", "BatchId");
            CreateIndex("dbo.BatchAction", "BatchId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchAction", new[] { "BatchId" });
            DropIndex("dbo.BatchNote", new[] { "BatchId" });
            DropIndex("dbo.Measurement", new[] { "BatchId" });
            DropForeignKey("dbo.BatchAction", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchNote", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.Measurement", "BatchId", "dbo.Batch");
            RenameColumn(table: "dbo.BatchAction", name: "BatchId", newName: "Batch_BatchId");
            RenameColumn(table: "dbo.BatchNote", name: "BatchId", newName: "Batch_BatchId");
            RenameColumn(table: "dbo.Measurement", name: "BatchId", newName: "Batch_BatchId");
            CreateIndex("dbo.BatchAction", "Batch_BatchId");
            CreateIndex("dbo.BatchNote", "Batch_BatchId");
            CreateIndex("dbo.Measurement", "Batch_BatchId");
            AddForeignKey("dbo.BatchAction", "Batch_BatchId", "dbo.Batch", "BatchId");
            AddForeignKey("dbo.BatchNote", "Batch_BatchId", "dbo.Batch", "BatchId");
            AddForeignKey("dbo.Measurement", "Batch_BatchId", "dbo.Batch", "BatchId");
        }
    }
}
