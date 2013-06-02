namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addbatchdescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Batch", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Batch", "Description");
        }
    }
}
