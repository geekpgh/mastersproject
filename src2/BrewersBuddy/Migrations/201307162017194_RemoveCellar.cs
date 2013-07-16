namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveCellar : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Container", "Cellar_CellarId", "dbo.Cellar");
            DropIndex("dbo.Container", new[] { "Cellar_CellarId" });
            AddColumn("dbo.Container", "OwnerId", c => c.Int(nullable: false));
            AddColumn("dbo.Container", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Container", "Cellar_CellarId");
            DropTable("dbo.Cellar");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cellar",
                c => new
                    {
                        CellarId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CellarId);
            
            AddColumn("dbo.Container", "Cellar_CellarId", c => c.Int());
            DropColumn("dbo.Container", "Quantity");
            DropColumn("dbo.Container", "OwnerId");
            CreateIndex("dbo.Container", "Cellar_CellarId");
            AddForeignKey("dbo.Container", "Cellar_CellarId", "dbo.Cellar", "CellarId");
        }
    }
}
