namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedListsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "List_ID", c => c.Long());
            AddColumn("Lists", "User_UserId1", c => c.Guid());
            AlterColumn("Lists", "ListName", c => c.String(nullable: false));
            AddForeignKey("Users", "List_ID", "Lists", "ID");
            AddForeignKey("Lists", "User_UserId1", "Users", "UserId");
            CreateIndex("Users", "List_ID");
            CreateIndex("Lists", "User_UserId1");
        }
        
        public override void Down()
        {
            DropIndex("Lists", new[] { "User_UserId1" });
            DropIndex("Users", new[] { "List_ID" });
            DropForeignKey("Lists", "User_UserId1", "Users");
            DropForeignKey("Users", "List_ID", "Lists");
            AlterColumn("Lists", "ListName", c => c.String());
            DropColumn("Lists", "User_UserId1");
            DropColumn("Users", "List_ID");
        }
    }
}
