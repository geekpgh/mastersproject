namespace BrewersBuddy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class frienduser : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.Friend", "FriendUserId", "dbo.UserProfile", "UserId");
            CreateIndex("dbo.Friend", "FriendUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Friend", new[] { "FriendUserId" });
            DropForeignKey("dbo.Friend", "FriendUserId", "dbo.UserProfile");
        }
    }
}
