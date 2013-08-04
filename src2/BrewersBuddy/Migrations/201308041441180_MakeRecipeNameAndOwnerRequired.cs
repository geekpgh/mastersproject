namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeRecipeNameAndOwnerRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recipe", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Recipe", "Name", c => c.String());
        }
    }
}
