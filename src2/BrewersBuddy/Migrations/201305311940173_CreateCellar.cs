namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateCellar : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cellar",
                c => new
                    {
                        CellarId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Owner_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.CellarId)
                .ForeignKey("dbo.UserProfile", t => t.Owner_UserId)
                .Index(t => t.Owner_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Cellar", new[] { "Owner_UserId" });
            DropForeignKey("dbo.Cellar", "Owner_UserId", "dbo.UserProfile");
            DropTable("dbo.Cellar");
        }
    }
}
