using Microsoft.AspNetCore.Identity;

namespace ChatApp.Core.Entities;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = default!;
    public bool IsOnline { get; set; }
    public ICollection<UserConnection> Connections { get; set; } = [];
    public ICollection<ChatParticipant> ParticipatedChats { get; set; } = [];
    // public ICollection<Group> Groups { get; set; } = [];
    // public ICollection<Chat> ChatsAsUser1 { get; set; } = [];
    // public ICollection<Chat> ChatsAsUser2 { get; set; } = [];
}
