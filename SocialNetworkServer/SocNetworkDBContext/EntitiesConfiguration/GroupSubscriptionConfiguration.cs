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
            builder.ToTable("UserGroupSubscriptions");

            builder.HasKey(ugs => new { ugs.SubscriberId, ugs.SubscribedToGroupId });
            builder.HasIndex(ugs => ugs.SubscriberId);
            builder.HasIndex(ugs => ugs.SubscribedToGroupId);

            builder.HasOne(ugs => ugs.Subscriber)
                .WithMany(u => u.SubscribedGroups)
                .HasForeignKey(ugs => ugs.SubscriberId);

            builder.HasOne(ugs => ugs.SubscribedToGroup)
                .WithMany(g => g.Subscribers)
                .HasForeignKey(ugs => ugs.SubscribedToGroupId);
        }
    }
}
