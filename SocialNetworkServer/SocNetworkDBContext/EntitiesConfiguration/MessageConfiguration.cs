using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.Property(message=>message.UserId).IsRequired();
            builder.Property(message => message.ChatId).IsRequired();
            builder.Property(message => message.Content).IsRequired();
            builder.Property(message => message.SendingDate).IsRequired();
            builder.Property(message => message.SendingDate).HasColumnType("datetime");
            builder.Property(message => message.SendingDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(message => new { message.ChatId, message.IsDeleted });
        }
    }
}
