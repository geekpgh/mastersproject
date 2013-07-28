namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Recipe_Content : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipe", "Howlongdoesittaketoprepare", c => c.String());
            AddColumn("dbo.Recipe", "WhatStepsToFollow", c => c.String());
            AddColumn("dbo.Recipe", "WhatingredientsdoIneed", c => c.String());
            AddColumn("dbo.Recipe", "Howlongwillitneedtoferment", c => c.String());
            AddColumn("dbo.Recipe", "WhencanIdrinkit", c => c.String());
            DropColumn("dbo.Recipe", "HowtoMakeit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Recipe", "HowtoMakeit", c => c.String());
            DropColumn("dbo.Recipe", "WhencanIdrinkit");
            DropColumn("dbo.Recipe", "Howlongwillitneedtoferment");
            DropColumn("dbo.Recipe", "WhatingredientsdoIneed");
            DropColumn("dbo.Recipe", "WhatStepsToFollow");
            DropColumn("dbo.Recipe", "Howlongdoesittaketoprepare");
        }
    }
}
