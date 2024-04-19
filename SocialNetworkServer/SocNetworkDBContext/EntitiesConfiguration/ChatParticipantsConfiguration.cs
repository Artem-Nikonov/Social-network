using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class ChatParticipantsConfiguration : IEntityTypeConfiguration<ChatParticipants>
    {
        public void Configure(EntityTypeBuilder<ChatParticipants> builder)
        {
            builder.HasKey(cp => new {cp.ChatId, cp.UserId });
            builder.HasIndex(cp => cp.ChatId);
            builder.HasIndex(cp => cp.UserId);

            builder.HasOne(cp => cp.User)
                .WithMany(u => u.ChatParticipants)
                .HasForeignKey(cp => cp.UserId);
            
            builder.HasOne(cp => cp.Chat)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp=>cp.ChatId);
        }
    }
}
