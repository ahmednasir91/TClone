namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Lists",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ListName = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropIndex("Lists", new[] { "UserId" });
            DropForeignKey("Lists", "UserId", "Users");
            DropTable("Lists");
        }
    }
}
