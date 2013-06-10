namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWebpages_Membership : DbMigration
    {
        public override void Up()
        {
			//CreateTable(
			//	"dbo.webpages_Membership",
			//	c => new
			//		{
			//			UserId = c.Int(nullable: false, identity: true),
			//			CreateDate = c.DateTime(nullable: false),
			//			ConfirmationToken = c.String(),
			//			IsConfirmed = c.Boolean(nullable: false),
			//			LastPasswordFailureDate = c.DateTime(nullable: false),
			//			PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
			//			Password = c.String(),
			//			PasswordChangeDate = c.DateTime(nullable: false),
			//			PasswordSalt = c.String(),
			//			PasswordVerificationToken = c.String(),
			//			PasswordVerificationTokenExpirationDate = c.DateTime(nullable: false),
			//		})
			//	.PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.webpages_Membership");
        }
    }
}
