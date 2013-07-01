namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeBatchNameMandatory : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Batch", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Batch", "Name", c => c.String());
        }
    }
}
