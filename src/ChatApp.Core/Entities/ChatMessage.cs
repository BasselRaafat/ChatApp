namespace ChatApp.Core.Entities;

public class ChatMessage : BaseEntity
{
    public string ChatId { get; set; } = default!;
    public string SenderId { get; set; } = default!;
    public string MessageText { get; set; } = default!;
    public bool Seen { get; set; } = default!;
    public DateTime Date { get; set; } = DateTime.Now;
    public AppUser Sender { get; set; } = default!;
    public Chat Chat { get; set; } = default!;
}
