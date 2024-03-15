using SocialNetworkServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System;

namespace SocialNetworkServer.SocNetworkDBContext
{
    public class SocialNetworkDBContext: DbContext
    {
        DbSet<UserAccount> Accounts { get; set; } = null!;
        DbSet<Page> Pages { get; set; } = null!;
        DbSet<UserPage> UserPages { get; set; } = null!;
        DbSet<CommunityPage> CommunityPages { get; set; } = null!;
        DbSet<Post> Posts { get; set; } = null!;
        DbSet<Subscription> Subscription { get; set; } = null!;

        //public SocialNetworkDBContext(DbContextOptions<SocialNetworkDBContext> options) : base(options)
        //{
        //    Database.EnsureCreated();
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var MySQLConfigString = "server=localhost;user=root;password=1117;database=Social_Network_Database";
            optionsBuilder.UseMySql(MySQLConfigString, new MySqlServerVersion(new Version(8,0,34)));
        }

    }
}
