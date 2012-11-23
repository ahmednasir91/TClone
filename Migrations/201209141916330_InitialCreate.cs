namespace TwitterClone.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Comment = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        LastPasswordFailureDate = c.DateTime(),
                        LastActivityDate = c.DateTime(),
                        LastLockoutDate = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                        ConfirmationToken = c.String(),
                        CreateDate = c.DateTime(),
                        IsLockedOut = c.Boolean(nullable: false),
                        LastPasswordChangedDate = c.DateTime(),
                        PasswordVerificationToken = c.String(),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId);
            
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
                "Tweets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        SenderId = c.Guid(nullable: false),
                        IsFavourite = c.Boolean(nullable: false),
                        Sender_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Users", t => t.Sender_UserId)
                .Index(t => t.Sender_UserId);
            
            CreateTable(
                "RoleUsers",
                c => new
                    {
                        Role_RoleId = c.Guid(nullable: false),
                        User_UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_RoleId, t.User_UserId })
                .ForeignKey("Roles", t => t.Role_RoleId, cascadeDelete: true)
                .ForeignKey("Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Role_RoleId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "UserUsers",
                c => new
                    {
                        User_UserId = c.Guid(nullable: false),
                        User_UserId1 = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_UserId, t.User_UserId1 })
                .ForeignKey("Users", t => t.User_UserId)
                .ForeignKey("Users", t => t.User_UserId1)
                .Index(t => t.User_UserId)
                .Index(t => t.User_UserId1);
            
        }
        
        public override void Down()
        {
            DropIndex("UserUsers", new[] { "User_UserId1" });
            DropIndex("UserUsers", new[] { "User_UserId" });
            DropIndex("RoleUsers", new[] { "User_UserId" });
            DropIndex("RoleUsers", new[] { "Role_RoleId" });
            DropIndex("Tweets", new[] { "Sender_UserId" });
            DropForeignKey("UserUsers", "User_UserId1", "Users");
            DropForeignKey("UserUsers", "User_UserId", "Users");
            DropForeignKey("RoleUsers", "User_UserId", "Users");
            DropForeignKey("RoleUsers", "Role_RoleId", "Roles");
            DropForeignKey("Tweets", "Sender_UserId", "Users");
            DropTable("UserUsers");
            DropTable("RoleUsers");
            DropTable("Tweets");
            DropTable("Roles");
            DropTable("Users");
        }
    }
}
