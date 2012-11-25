namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedUsersModeladdedtiledfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "Tiled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Users", "Tiled");
        }
    }
}
