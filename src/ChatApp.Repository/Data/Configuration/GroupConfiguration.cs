using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Data.Configuration;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .HasMany(G => G.GroupMessages)
            .WithOne(GM => GM.Group)
            .HasForeignKey(GM => GM.GroupId);

        builder.HasMany(G => G.Users).WithMany(GM => GM.Groups);
    }
}
