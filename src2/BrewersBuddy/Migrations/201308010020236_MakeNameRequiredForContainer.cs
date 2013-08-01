namespace BrewersBuddy.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class MakeNameRequiredForContainer : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Container", "Name", c => c.String(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Container", "Name", c => c.String());
        }
    }
}
