namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBatchNote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BatchNote",
                c => new
                    {
                        NoteId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AuthorDate = c.DateTime(nullable: false),
                        Text = c.String(),
                        Batch_BatchId = c.Int(),
                        Author_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.NoteId)
                .ForeignKey("dbo.Batch", t => t.Batch_BatchId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.Author_UserId)
                .Index(t => t.Batch_BatchId)
                .Index(t => t.Author_UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.BatchNote", new[] { "Author_UserId" });
            DropIndex("dbo.BatchNote", new[] { "Batch_BatchId" });
            DropForeignKey("dbo.BatchNote", "Author_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.BatchNote", "Batch_BatchId", "dbo.Batch");
            DropTable("dbo.BatchNote");
        }
    }
}
