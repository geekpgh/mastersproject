namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class batchactionrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BatchAction", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.BatchAction", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BatchAction", "Description", c => c.String());
            AlterColumn("dbo.BatchAction", "Title", c => c.String());
        }
    }
}
