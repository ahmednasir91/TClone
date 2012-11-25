namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Favouritesconvertedtolist : DbMigration
    {
        public override void Up()
        {
            AddColumn("Tweets", "User_UserId", c => c.Guid());
            AddColumn("Tweets", "User_UserId1", c => c.Guid());
            AddForeignKey("Tweets", "User_UserId", "Users", "UserId");
            AddForeignKey("Tweets", "User_UserId1", "Users", "UserId");
            CreateIndex("Tweets", "User_UserId");
            CreateIndex("Tweets", "User_UserId1");
            DropColumn("Tweets", "IsFavourite");
        }
        
        public override void Down()
        {
            AddColumn("Tweets", "IsFavourite", c => c.Boolean(nullable: false));
            DropIndex("Tweets", new[] { "User_UserId1" });
            DropIndex("Tweets", new[] { "User_UserId" });
            DropForeignKey("Tweets", "User_UserId1", "Users");
            DropForeignKey("Tweets", "User_UserId", "Users");
            DropColumn("Tweets", "User_UserId1");
            DropColumn("Tweets", "User_UserId");
        }
    }
}
