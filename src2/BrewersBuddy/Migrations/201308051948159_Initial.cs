namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Batch",
                c => new
                    {
                        BatchId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        BatchTypeValue = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BatchId)
                .ForeignKey("dbo.UserProfile", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        City = c.String(),
                        State = c.String(maxLength: 2),
                        Zip = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.BatchRating",
                c => new
                    {
                        BatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                        UserProfile_UserId = c.Int(),
                    })
                .PrimaryKey(t => new { t.BatchId, t.UserId })
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .Index(t => t.BatchId)
                .Index(t => t.UserId)
                .Index(t => t.UserProfile_UserId);
            
            CreateTable(
                "dbo.BatchComment",
                c => new
                    {
                        BatchCommentId = c.Int(nullable: false, identity: true),
                        BatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Comment = c.String(nullable: false, maxLength: 256),
                        PostDate = c.DateTime(),
                        UserProfile_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.BatchCommentId)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId)
                .Index(t => t.BatchId)
                .Index(t => t.UserId)
                .Index(t => t.UserProfile_UserId);
            
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        FriendId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FriendUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.UserProfile", t => t.FriendUserId)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .Index(t => t.FriendUserId)
                .Index(t => t.UserId);
            
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
                        BatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MeasurementId)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .Index(t => t.BatchId);
            
            CreateTable(
                "dbo.BatchNote",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AuthorDate = c.DateTime(nullable: false),
                        Text = c.String(),
                        BatchId = c.Int(nullable: false),
                        AuthorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.AuthorId)
                .Index(t => t.BatchId)
                .Index(t => t.AuthorId);
            
            CreateTable(
                "dbo.BatchAction",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ActionDate = c.DateTime(nullable: false),
                        Description = c.String(nullable: false),
                        ActionTypeValue = c.Int(nullable: false),
                        PerformerId = c.Int(nullable: false),
                        BatchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.PerformerId)
                .Index(t => t.BatchId)
                .Index(t => t.PerformerId);
            
            CreateTable(
                "dbo.Container",
                c => new
                    {
                        ContainerId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Volume = c.Double(nullable: false),
                        UnitValue = c.Int(nullable: false),
                        BatchId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ContainerTypeValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContainerId)
                .ForeignKey("dbo.Batch", t => t.BatchId, cascadeDelete: true)
                .Index(t => t.BatchId);
            
            CreateTable(
                "dbo.Recipe",
                c => new
                    {
                        RecipeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        Costs = c.String(),
                        Prep = c.String(),
                        Process = c.String(),
                        Finishing = c.String(),
                    })
                .PrimaryKey(t => t.RecipeId)
                .ForeignKey("dbo.UserProfile", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
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
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        CreateDate = c.DateTime(nullable: false),
                        ConfirmationToken = c.String(),
                        IsConfirmed = c.Boolean(nullable: false),
                        LastPasswordFailureDate = c.DateTime(nullable: false),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(),
                        PasswordChangeDate = c.DateTime(nullable: false),
                        PasswordSalt = c.String(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.BatchCollaborator",
                c => new
                    {
                        BatchId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BatchId, t.UserId })
                .ForeignKey("dbo.Batch", t => t.BatchId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.BatchId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RecipeIngredient",
                c => new
                    {
                        IngredientID = c.Int(nullable: false),
                        RecipeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IngredientID, t.RecipeID })
                .ForeignKey("dbo.Ingredient", t => t.IngredientID)
                .ForeignKey("dbo.Recipe", t => t.RecipeID)
                .Index(t => t.IngredientID)
                .Index(t => t.RecipeID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RecipeIngredient", new[] { "RecipeID" });
            DropIndex("dbo.RecipeIngredient", new[] { "IngredientID" });
            DropIndex("dbo.BatchCollaborator", new[] { "UserId" });
            DropIndex("dbo.BatchCollaborator", new[] { "BatchId" });
            DropIndex("dbo.Recipe", new[] { "OwnerId" });
            DropIndex("dbo.Container", new[] { "BatchId" });
            DropIndex("dbo.BatchAction", new[] { "PerformerId" });
            DropIndex("dbo.BatchAction", new[] { "BatchId" });
            DropIndex("dbo.BatchNote", new[] { "AuthorId" });
            DropIndex("dbo.BatchNote", new[] { "BatchId" });
            DropIndex("dbo.Measurement", new[] { "BatchId" });
            DropIndex("dbo.Friend", new[] { "UserId" });
            DropIndex("dbo.Friend", new[] { "FriendUserId" });
            DropIndex("dbo.BatchComment", new[] { "UserProfile_UserId" });
            DropIndex("dbo.BatchComment", new[] { "UserId" });
            DropIndex("dbo.BatchComment", new[] { "BatchId" });
            DropIndex("dbo.BatchRating", new[] { "UserProfile_UserId" });
            DropIndex("dbo.BatchRating", new[] { "UserId" });
            DropIndex("dbo.BatchRating", new[] { "BatchId" });
            DropIndex("dbo.Batch", new[] { "OwnerId" });
            DropForeignKey("dbo.RecipeIngredient", "RecipeID", "dbo.Recipe");
            DropForeignKey("dbo.RecipeIngredient", "IngredientID", "dbo.Ingredient");
            DropForeignKey("dbo.BatchCollaborator", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchCollaborator", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.Recipe", "OwnerId", "dbo.UserProfile");
            DropForeignKey("dbo.Container", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchAction", "PerformerId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchAction", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchNote", "AuthorId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchNote", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.Measurement", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.Friend", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Friend", "FriendUserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchComment", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.BatchRating", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchRating", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchRating", "BatchId", "dbo.Batch");
            DropForeignKey("dbo.Batch", "OwnerId", "dbo.UserProfile");
            DropTable("dbo.RecipeIngredient");
            DropTable("dbo.BatchCollaborator");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.Ingredient");
            DropTable("dbo.Recipe");
            DropTable("dbo.Container");
            DropTable("dbo.BatchAction");
            DropTable("dbo.BatchNote");
            DropTable("dbo.Measurement");
            DropTable("dbo.Friend");
            DropTable("dbo.BatchComment");
            DropTable("dbo.BatchRating");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Batch");
        }
    }
}
