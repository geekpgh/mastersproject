namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fissomething : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Recipe", "WhenCanIDrinkIt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "WhenCanIDrinkIt", c => c.String());
        }
    }
}
