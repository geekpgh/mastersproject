namespace BrewersBuddy.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddPostDateToBatchComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BatchComment", "PostDate", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.BatchComment", "PostDate");
        }
    }
}
