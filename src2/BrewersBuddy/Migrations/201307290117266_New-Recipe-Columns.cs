namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewRecipeColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipe", "Costs", c => c.String());
            AddColumn("dbo.Recipe", "Prep", c => c.String());
            AddColumn("dbo.Recipe", "Process", c => c.String());
            AddColumn("dbo.Recipe", "Finishing", c => c.String());
            DropColumn("dbo.Recipe", "Cost");
            DropColumn("dbo.Recipe", "HowtoMakeit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "HowtoMakeit", c => c.String());
            AddColumn("dbo.Recipe", "Cost", c => c.Double(nullable: false));
            DropColumn("dbo.Recipe", "Finishing");
            DropColumn("dbo.Recipe", "Process");
            DropColumn("dbo.Recipe", "Prep");
            DropColumn("dbo.Recipe", "Costs");
        }
    }
}
