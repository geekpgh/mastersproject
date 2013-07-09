namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMeasurement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Measurement",
                c => new
                    {
                        MeasurementId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MeasurementDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Value = c.Double(nullable: false),
                        Measured = c.String(),
                        Batch_BatchId = c.Int(),
                    })
                .PrimaryKey(t => t.MeasurementId)
                .ForeignKey("dbo.Batch", t => t.Batch_BatchId)
                .Index(t => t.Batch_BatchId);            
           
        }
        
        public override void Down()
        {
                      
            DropIndex("dbo.Measurement", new[] { "Batch_BatchId" });
            DropForeignKey("dbo.Measurement", "Batch_BatchId", "dbo.Batch");
            DropTable("dbo.Measurement");            
        }
    }
}
