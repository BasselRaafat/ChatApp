using Microsoft.AspNetCore.Identity;

namespace ChatApp.Core.Entities;

public class AppUser : IdentityUser
{
    public ICollection<UserConnection> ConnectionId { get; set; } = [];
    public ICollection<AppUser> Groups { get; set; } = [];
}
