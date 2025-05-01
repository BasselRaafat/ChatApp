using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Data.Configuration;

public class UserConnectionConfiguration : IEntityTypeConfiguration<UserConnection>
{
    public void Configure(EntityTypeBuilder<UserConnection> builder)
    {
        builder.HasKey(UC => new { UC.ConnectionId, UC.UserId });
    }
}
