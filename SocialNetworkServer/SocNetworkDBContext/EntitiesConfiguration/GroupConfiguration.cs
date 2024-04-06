using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.Property(group=>group.GroupName).IsRequired();
            builder.Property(group => group.Description).IsRequired();
            builder.Property(group => group.CreatorId).IsRequired();
            builder.Property(group => group.PostPermissions).IsRequired();
        }
    }
}
