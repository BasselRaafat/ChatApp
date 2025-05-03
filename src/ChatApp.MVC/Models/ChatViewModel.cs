using ChatApp.Core.Entities;

namespace ChatApp.MVC.Models;

public class ChatViewModel
{
    public string Id { get; set; } = default!;
    public bool IsGroup { get; set; } = false;
    public string? Name { get; set; }
    public DateTime LastTimeActive { get; set; } = DateTime.UtcNow;

    public string? LastMessageSentId { get; set; } = default!;
    public ChatMessage? LastMessageSent { get; set; } = default!;

    public ICollection<ChatMessage> Messages { get; set; } = [];
    public ICollection<ChatParticipant> ChatParticipants { get; set; } = [];
}
