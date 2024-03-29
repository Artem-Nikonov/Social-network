﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;
using System.Reflection.Emit;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.ToTable("UserSubscriptions");
            builder.HasOne(us => us.SubscribedToUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(us => us.SubscribedToUserId);

            builder.HasOne(us => us.Subscriber)
                .WithMany(u => u.Followings)
                .HasForeignKey(us => us.SubscriberId);
        }
    }
}
