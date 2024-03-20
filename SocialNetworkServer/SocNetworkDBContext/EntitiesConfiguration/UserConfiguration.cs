using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Reflection.Emit;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.Property(user => user.Login).IsRequired();
            builder.Property(user => user.PasswordHash).IsRequired();
            builder.Property(user => user.UserName).IsRequired();
            builder.Property(user => user.UserSurname).IsRequired();
            builder.Property(user => user.RegistrationDate).IsRequired();
            builder.Property(u => u.RegistrationDate).HasColumnType("datetime");
            builder.Property(user => user.RegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(user => user.UserName).HasDefaultValue("New");
            builder.Property(user => user.UserSurname).HasDefaultValue("User");
        }
    }
}
