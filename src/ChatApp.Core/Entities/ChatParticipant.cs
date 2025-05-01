namespace ChatApp.Core.Entities;

public class ChatParticipant
{
    public AppUser User { get; set; } = default!;
    public string UserId { get; set; } = default!;
    public Chat Chat { get; set; } = default!;
    public string ChatId { get; set; } = default!;
}
