namespace ChatApp.Core.Entities;

public class Chat : BaseEntity
{
    // public string User1Id { get; set; } = default!;
    // public AppUser User1 { get; set; } = default!;
    //
    // public string User2Id { get; set; } = default!;
    // public AppUser User2 { get; set; } = default!;

    public bool IsGroup { get; set; } = false;
    public string? Name { get; set; }
    public DateTime LastTimeActive { get; set; } = DateTime.UtcNow;

    public string? LastMessageSentId { get; set; } = default!;
    public ChatMessage? LastMessageSent { get; set; } = default!;

    public ICollection<ChatMessage> Messages { get; set; } = [];
    public ICollection<ChatParticipant> ChatParticipants { get; set; } = [];
}
