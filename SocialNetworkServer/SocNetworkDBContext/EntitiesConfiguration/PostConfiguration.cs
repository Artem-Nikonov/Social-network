using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetworkServer.SocNetworkDBContext.Entities;

namespace SocialNetworkServer.SocNetworkDBContext.EntitiesConfiguration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(post => post.UserId).IsRequired();
            builder.Property(post => post.Content).IsRequired();
            builder.Property(post => post.CreationDate).IsRequired();
            builder.Property(post => post.CreationDate).HasColumnType("datetime");
            builder.Property(post => post.CreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.HasIndex(post => new { post.UserId, post.GroupId, post.IsDeleted });
        }
    }
}
