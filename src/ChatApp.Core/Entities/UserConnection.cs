namespace ChatApp.Core.Entities;

public class UserConnection
{
    public string UserId { get; set; } = default!;
    public string ConnectionId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
}
