namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedLists : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Lists", "UserId", "Users");
            DropIndex("Lists", new[] { "UserId" });
            AddColumn("Lists", "Description", c => c.String());
            AddColumn("Lists", "User_UserId", c => c.Guid());
            AddForeignKey("Lists", "User_UserId", "Users", "UserId");
            CreateIndex("Lists", "User_UserId");
            DropColumn("Lists", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("Lists", "UserId", c => c.Guid(nullable: false));
            DropIndex("Lists", new[] { "User_UserId" });
            DropForeignKey("Lists", "User_UserId", "Users");
            DropColumn("Lists", "User_UserId");
            DropColumn("Lists", "Description");
            CreateIndex("Lists", "UserId");
            AddForeignKey("Lists", "UserId", "Users", "UserId", cascadeDelete: true);
        }
    }
}
