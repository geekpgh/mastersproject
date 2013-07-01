namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBatchTypeValueToBatch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Batch", "BatchTypeValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Batch", "BatchTypeValue");
        }
    }
}
