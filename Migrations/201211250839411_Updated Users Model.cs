namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUsersModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "BackgroundImage", c => c.String());
            AddColumn("Users", "BackgroundColor", c => c.String());
            AddColumn("Users", "LinkColor", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Users", "LinkColor");
            DropColumn("Users", "BackgroundColor");
            DropColumn("Users", "BackgroundImage");
        }
    }
}
