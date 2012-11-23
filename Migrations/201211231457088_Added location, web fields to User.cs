namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedlocationwebfieldstoUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "Location", c => c.String());
            AddColumn("Users", "Website", c => c.String());
            AddColumn("Users", "WebsiteURL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Users", "WebsiteURL");
            DropColumn("Users", "Website");
            DropColumn("Users", "Location");
        }
    }
}
