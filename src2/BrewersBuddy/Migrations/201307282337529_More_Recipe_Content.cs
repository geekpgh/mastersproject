namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class More_Recipe_Content : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipe", "Preparation", c => c.String());
            AddColumn("dbo.Recipe", "Ingredient", c => c.String());
            AddColumn("dbo.Recipe", "Ferment", c => c.String());
            AddColumn("dbo.Recipe", "Finish", c => c.String());
            DropColumn("dbo.Recipe", "Preparation_Steps");
            DropColumn("dbo.Recipe", "Recipe_Ingredients");
            DropColumn("dbo.Recipe", "Ferment_Time");
            DropColumn("dbo.Recipe", "Finish_Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "Finish_Time", c => c.String());
            AddColumn("dbo.Recipe", "Ferment_Time", c => c.String());
            AddColumn("dbo.Recipe", "Recipe_Ingredients", c => c.String());
            AddColumn("dbo.Recipe", "Preparation_Steps", c => c.String());
            DropColumn("dbo.Recipe", "Finish");
            DropColumn("dbo.Recipe", "Ferment");
            DropColumn("dbo.Recipe", "Ingredient");
            DropColumn("dbo.Recipe", "Preparation");
        }
    }
}
