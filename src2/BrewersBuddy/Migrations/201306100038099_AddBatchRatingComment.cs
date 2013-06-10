namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBatchRatingComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchRating", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BatchRating", "Comment");
        }
    }
}
