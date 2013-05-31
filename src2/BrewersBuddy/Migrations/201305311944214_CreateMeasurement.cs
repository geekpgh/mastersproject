namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMeasurement : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RecipeIngredient", "Recipe_RecipeId", "dbo.Recipe");
            DropForeignKey("dbo.RecipeIngredient", "Ingredient_IngredientId", "dbo.Ingredient");
            DropIndex("dbo.RecipeIngredient", new[] { "Recipe_RecipeId" });
            DropIndex("dbo.RecipeIngredient", new[] { "Ingredient_IngredientId" });
            CreateTable(
                "dbo.Measurement",
                c => new
                    {
                        MeasurementId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MeasurementDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Value = c.Double(nullable: false),
                        Measured = c.String(),
                        Batch_BatchId = c.Int(),
                    })
                .PrimaryKey(t => t.MeasurementId)
                .ForeignKey("dbo.Batch", t => t.Batch_BatchId)
                .Index(t => t.Batch_BatchId);
            
            CreateTable(
                "dbo.IngredientRecipe",
                c => new
                    {
                        Ingredient_IngredientId = c.Int(nullable: false),
                        Recipe_RecipeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Ingredient_IngredientId, t.Recipe_RecipeId })
                .ForeignKey("dbo.Ingredient", t => t.Ingredient_IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Recipe", t => t.Recipe_RecipeId, cascadeDelete: true)
                .Index(t => t.Ingredient_IngredientId)
                .Index(t => t.Recipe_RecipeId);
            
            DropTable("dbo.RecipeIngredient");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RecipeIngredient",
                c => new
                    {
                        Recipe_RecipeId = c.Int(nullable: false),
                        Ingredient_IngredientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recipe_RecipeId, t.Ingredient_IngredientId });
            
            DropIndex("dbo.IngredientRecipe", new[] { "Recipe_RecipeId" });
            DropIndex("dbo.IngredientRecipe", new[] { "Ingredient_IngredientId" });
            DropIndex("dbo.Measurement", new[] { "Batch_BatchId" });
            DropForeignKey("dbo.IngredientRecipe", "Recipe_RecipeId", "dbo.Recipe");
            DropForeignKey("dbo.IngredientRecipe", "Ingredient_IngredientId", "dbo.Ingredient");
            DropForeignKey("dbo.Measurement", "Batch_BatchId", "dbo.Batch");
            DropTable("dbo.IngredientRecipe");
            DropTable("dbo.Measurement");
            CreateIndex("dbo.RecipeIngredient", "Ingredient_IngredientId");
            CreateIndex("dbo.RecipeIngredient", "Recipe_RecipeId");
            AddForeignKey("dbo.RecipeIngredient", "Ingredient_IngredientId", "dbo.Ingredient", "IngredientId", cascadeDelete: true);
            AddForeignKey("dbo.RecipeIngredient", "Recipe_RecipeId", "dbo.Recipe", "RecipeId", cascadeDelete: true);
        }
    }
}
