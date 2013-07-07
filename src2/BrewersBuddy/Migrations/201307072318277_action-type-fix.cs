namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class actiontypefix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchAction", "ActionTypeValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchAction", "ActionTypeValue");
        }
    }
}
