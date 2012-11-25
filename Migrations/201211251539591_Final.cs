namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Final : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FullName = c.String(nullable: false),
                        Comment = c.String(),
                        Bio = c.String(),
                        Location = c.String(),
                        Website = c.String(),
                        WebsiteURL = c.String(),
                        BackgroundImage = c.String(),
                        BackgroundColor = c.String(),
                        LinkColor = c.String(),
                        Tiled = c.Boolean(nullable: false),
                        Avatar = c.String(),
                        User_Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Username)
                .ForeignKey("Users", t => t.User_Username)
                .Index(t => t.User_Username);
            
            CreateTable(
                "Lists",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        ListName = c.String(nullable: false),
                        Description = c.String(),
                        Username = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "Tweets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        SenderId = c.Guid(nullable: false),
                        DateAndTime = c.DateTime(),
                        Sender_Username = c.String(maxLength: 128),
                        User_Username = c.String(maxLength: 128),
                        User_Username1 = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Users", t => t.Sender_Username)
                .ForeignKey("Users", t => t.User_Username)
                .ForeignKey("Users", t => t.User_Username1)
                .Index(t => t.Sender_Username)
                .Index(t => t.User_Username)
                .Index(t => t.User_Username1);
            
            CreateTable(
                "Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        RoleName = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "UserLists",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Username, t.ID })
                .ForeignKey("Users", t => t.Username, cascadeDelete: true)
                .ForeignKey("Lists", t => t.ID, cascadeDelete: true)
                .Index(t => t.Username)
                .Index(t => t.ID);
            
            CreateTable(
                "RoleUsers",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.Username })
                .ForeignKey("Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Users", t => t.Username, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.Username);
            
            CreateTable(
                "UserFollowers",
                c => new
                    {
                        User_Username = c.String(nullable: false, maxLength: 128),
                        User_Username1 = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.User_Username, t.User_Username1 })
                .ForeignKey("Users", t => t.User_Username)
                .ForeignKey("Users", t => t.User_Username1)
                .Index(t => t.User_Username)
                .Index(t => t.User_Username1);
            
        }
        
        public override void Down()
        {
            DropIndex("UserFollowers", new[] { "User_Username1" });
            DropIndex("UserFollowers", new[] { "User_Username" });
            DropIndex("RoleUsers", new[] { "Username" });
            DropIndex("RoleUsers", new[] { "RoleId" });
            DropIndex("UserLists", new[] { "ID" });
            DropIndex("UserLists", new[] { "Username" });
            DropIndex("Tweets", new[] { "User_Username1" });
            DropIndex("Tweets", new[] { "User_Username" });
            DropIndex("Tweets", new[] { "Sender_Username" });
            DropIndex("Lists", new[] { "Username" });
            DropIndex("Users", new[] { "User_Username" });
            DropForeignKey("UserFollowers", "User_Username1", "Users");
            DropForeignKey("UserFollowers", "User_Username", "Users");
            DropForeignKey("RoleUsers", "Username", "Users");
            DropForeignKey("RoleUsers", "RoleId", "Roles");
            DropForeignKey("UserLists", "ID", "Lists");
            DropForeignKey("UserLists", "Username", "Users");
            DropForeignKey("Tweets", "User_Username1", "Users");
            DropForeignKey("Tweets", "User_Username", "Users");
            DropForeignKey("Tweets", "Sender_Username", "Users");
            DropForeignKey("Lists", "Username", "Users");
            DropForeignKey("Users", "User_Username", "Users");
            DropTable("UserFollowers");
            DropTable("RoleUsers");
            DropTable("UserLists");
            DropTable("Roles");
            DropTable("Tweets");
            DropTable("Lists");
            DropTable("Users");
        }
    }
}
