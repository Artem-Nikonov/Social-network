using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext
{
    public class SocialNetworkDBContext: DbContext
    {
        public DbSet<UserAccount> Accounts { get; set; } = null!;
        public DbSet<Page> Pages { get; set; } = null!;
        public DbSet<UserPage> UserPages { get; set; } = null!;
        public DbSet<CommunityPage> CommunityPages { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Subscription> Subscription { get; set; } = null!;

        public SocialNetworkDBContext(DbContextOptions<SocialNetworkDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var MySQLConfigString = "server=localhost;user=root;password=1117;database=Social_Network_Database";
            optionsBuilder.UseMySql(MySQLConfigString, new MySqlServerVersion(new Version(8,0,34)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPage>().Property(UPage => UPage.UserName).HasDefaultValue("New");
            modelBuilder.Entity<UserPage>().Property(UPage => UPage.UserSurname).HasDefaultValue("User");
        }

    }
}
