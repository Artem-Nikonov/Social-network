using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Reflection.Emit;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class GroupSubscriptionConfiguration : IEntityTypeConfiguration<GroupSubscription>
    {
        public void Configure(EntityTypeBuilder<GroupSubscription> builder)
        {
            builder.ToTable("GroupSubscriptions");
            builder.HasOne(gs => gs.Subscriber)
                .WithMany(u => u.SubscribedGroups)
                .HasForeignKey(gs => gs.SubscriberId);

            builder.HasOne(gs => gs.SubscribedToGroup)
                .WithMany(g => g.Subscribers)
                .HasForeignKey(gs => gs.SubscribedToGroupId);
        }
    }
}
