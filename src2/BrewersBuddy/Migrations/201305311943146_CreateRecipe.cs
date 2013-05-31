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
                    })
                .PrimaryKey(t => t.RecipeId);
            
            CreateTable(
                "dbo.RecipeIngredient",
                c => new
                    {
                        Recipe_RecipeId = c.Int(nullable: false),
                        Ingredient_IngredientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipe_RecipeId, t.Ingredient_IngredientId })
                .ForeignKey("dbo.Recipe", t => t.Recipe_RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.Ingredient", t => t.Ingredient_IngredientId, cascadeDelete: true)
                .Index(t => t.Recipe_RecipeId)
                .Index(t => t.Ingredient_IngredientId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RecipeIngredient", new[] { "Ingredient_IngredientId" });
            DropIndex("dbo.RecipeIngredient", new[] { "Recipe_RecipeId" });
            DropForeignKey("dbo.RecipeIngredient", "Ingredient_IngredientId", "dbo.Ingredient");
            DropForeignKey("dbo.RecipeIngredient", "Recipe_RecipeId", "dbo.Recipe");
            DropTable("dbo.RecipeIngredient");
            DropTable("dbo.Recipe");
        }
    }
}
