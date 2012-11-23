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
    }
}
