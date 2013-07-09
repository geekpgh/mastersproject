namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class containertypeenumfix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Container", "ContainerTypeValue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Container", "ContainerTypeValue");
        }
    }
}
