namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedFullNametoUsers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Users", "FullName", c => c.String(nullable: false));
            DropColumn("Users", "FirstName");
            DropColumn("Users", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("Users", "LastName", c => c.String());
            AddColumn("Users", "FirstName", c => c.String());
            AlterColumn("Users", "FullName", c => c.String());
        }
    }
}
