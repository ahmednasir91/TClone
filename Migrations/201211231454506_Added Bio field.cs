namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedBiofield : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "Bio", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Users", "Bio");
        }
    }
}
