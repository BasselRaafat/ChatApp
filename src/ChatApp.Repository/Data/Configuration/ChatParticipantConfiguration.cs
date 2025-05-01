using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Data.Configuration;

public class ChatParticipantConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.HasKey(CP => new { CP.UserId, CP.ChatId });
        builder
            .HasOne(CP => CP.Chat)
            .WithMany(C => C.ChatParticipants)
            .HasForeignKey(CP => CP.ChatId);
    }
}
