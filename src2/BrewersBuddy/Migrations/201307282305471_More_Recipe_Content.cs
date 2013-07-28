namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class More_Recipe_Content : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipe", "Preparation_Steps", c => c.String());
            AddColumn("dbo.Recipe", "Recipe_Ingredients", c => c.String());
            AddColumn("dbo.Recipe", "Ferment_Time", c => c.String());
            AddColumn("dbo.Recipe", "Finish_Time", c => c.String());
            AlterColumn("dbo.Recipe", "Cost", c => c.String());
            DropColumn("dbo.Recipe", "Howlongdoesittaketoprepare");
            DropColumn("dbo.Recipe", "WhatStepsToFollow");
            DropColumn("dbo.Recipe", "WhatingredientsdoIneed");
            DropColumn("dbo.Recipe", "Howlongwillitneedtoferment");
            DropColumn("dbo.Recipe", "WhencanIdrinkit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "WhencanIdrinkit", c => c.String());
            AddColumn("dbo.Recipe", "Howlongwillitneedtoferment", c => c.String());
            AddColumn("dbo.Recipe", "WhatingredientsdoIneed", c => c.String());
            AddColumn("dbo.Recipe", "WhatStepsToFollow", c => c.String());
            AddColumn("dbo.Recipe", "Howlongdoesittaketoprepare", c => c.String());
            AlterColumn("dbo.Recipe", "Cost", c => c.Double(nullable: false));
            DropColumn("dbo.Recipe", "Finish_Time");
            DropColumn("dbo.Recipe", "Ferment_Time");
            DropColumn("dbo.Recipe", "Recipe_Ingredients");
            DropColumn("dbo.Recipe", "Preparation_Steps");
        }
    }
}
