namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRecipe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Recipe",
                c => new
                    {
                        RecipeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Cost = c.Double(nullable: false),
						OwnerId = c.Int(nullable:false),
						HowtoMakeit = c.String(),
                    })
                .PrimaryKey(t => t.RecipeId);         
        }
        
        public override void Down()
        {
            DropTable("dbo.Recipe");
        }
    }
}
