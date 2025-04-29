using ChatApp.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<UserConnection> UserConnections { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMessage> GroupMessages { get; set; }

    public AppDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
