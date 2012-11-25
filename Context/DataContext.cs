using System.Data.Entity;
using TwitterClone.Entities;

namespace TwitterClone.Context
{
    public class DataContext : DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<List> Lists { get; set; }

        public DataContext() : base()
        {
            //Configuration.LazyLoadingEnabled = false;
    
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(e => e.Lists).WithMany(e => e.Members).Map(t => t.MapLeftKey("Username").MapRightKey("ID").ToTable("UserLists"));
            modelBuilder.Entity<Role>().HasMany(e => e.Users).WithMany(e => e.Roles).Map(t => t.MapLeftKey("RoleId").MapRightKey("Username").ToTable("RoleUsers"));
            modelBuilder.Entity<User>().HasMany(e => e.Followers).WithMany().Map(e => e.ToTable("UserFollowers"));
            modelBuilder.Entity<User>().HasMany(e => e.Following).WithMany().Map(e => e.ToTable("UserFollowing"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
