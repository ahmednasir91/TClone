using System.Data.Entity;
using System.Web.Security;
using TwitterClone.Migrations;

namespace TwitterClone.Context
{
    public class DataContextInitializer : MigrateDatabaseToLatestVersion<DataContext, Configuration>
    {
        protected void Seed(DataContext context)
        {
        MembershipCreateStatus Status;
        System.Web.Security.Membership.CreateUser("Demo", "123456", "demo@demo.com", null, null, true, out Status);
        Roles.CreateRole("Admin");
        Roles.AddUserToRole("Demo", "Admin");
        }
    }
}