using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Data.Configuration;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasMany(C => C.Messages).WithOne(M => M.Chat).HasForeignKey(M => M.ChatId);
        builder
            .HasOne(C => C.LastMessageSent)
            .WithOne()
            .HasForeignKey<Chat>(C => C.LastMessageSentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
