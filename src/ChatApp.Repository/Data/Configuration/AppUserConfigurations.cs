using ChatApp.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.Repository.Data.Configuration;

public class AppUserConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasMany(U => U.Connections).WithOne(C => C.User).HasForeignKey(C => C.UserId);

        // builder
        //     .HasMany(U => U.ChatsAsUser1)
        //     .WithOne(C => C.User1)
        //     .HasForeignKey(C => C.User1Id)
        //     .OnDelete(DeleteBehavior.Restrict);
        // builder
        //     .HasMany(U => U.ChatsAsUser2)
        //     .WithOne(C => C.User2)
        //     .HasForeignKey(C => C.User2Id)
        //     .OnDelete(DeleteBehavior.Restrict);

        // builder.HasMany(U => U.Groups).WithMany(C => C.Users);

        builder
            .HasMany(U => U.ParticipatedChats)
            .WithOne(P => P.User)
            .HasForeignKey(P => P.UserId);

        builder.HasMany<ChatMessage>().WithOne(CM => CM.Sender).HasForeignKey(CM => CM.SenderId);
    }
}
