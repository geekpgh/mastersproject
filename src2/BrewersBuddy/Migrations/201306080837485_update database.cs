namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipe", "AddDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Recipe", "StartDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "StartDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Recipe", "AddDate");
        }
    }
}
