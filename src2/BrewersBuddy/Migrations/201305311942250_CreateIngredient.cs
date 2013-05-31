namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIngredient : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ingredient",
                c => new
                    {
                        IngredientId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Cost = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IngredientId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Ingredient");
        }
    }
}
