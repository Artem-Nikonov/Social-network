using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(chat => chat.ChatName).IsRequired();
            builder.Property(chat => chat.CreatorId).IsRequired();
        }
    }
}
