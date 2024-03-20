using Microsoft.EntityFrameworkCore;
using System.Data;
using System;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext
{
    public class SocialNetworkDBContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
        public DbSet<GroupSubscription> GroupSubscriptions { get; set; } = null!;
        public SocialNetworkDBContext(DbContextOptions<SocialNetworkDBContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var MySQLConfigString = "server=localhost;user=root;password=1117;database=Social_Network_Database";
        //    optionsBuilder.UseMySql(MySQLConfigString, new MySqlServerVersion(new Version(8, 0, 34)));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User() { UserId=1, PasswordHash = "ohjhygui", Login = "kkkkk" });
            modelBuilder.Entity<User>().Property(user=>user.UserName).HasDefaultValue("New");
            modelBuilder.Entity<User>().Property(user=>user.UserSurname).HasDefaultValue("User");
            modelBuilder.Entity<User>()
        .Property(user => user.FormattedRegistrationDate)
        .HasComputedColumnSql($"DATE_FORMAT('{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm")}', '%Y-%m-%d %H:%i')");
        }

    }
}
