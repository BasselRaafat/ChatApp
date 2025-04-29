namespace ChatApp.Core.Entities;

public class Chat
{
    public string User1Id { get; set; } = default!;
    public string User2Id { get; set; } = default!;
    public AppUser User1 { get; set; } = default!;
    public AppUser User2 { get; set; } = default!;
    public ICollection<ChatMessage> Messages { get; set; } = default!;
}
