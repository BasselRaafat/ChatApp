namespace ChatApp.Core.Entities;

public class ChatMessage : BaseEntity
{
    public Chat Chat { get; set; } = default!;
    public string ChatId { get; set; } = default!;

    public AppUser Sender { get; set; } = default!;
    public string SenderId { get; set; } = default!;

    public string MessageText { get; set; } = default!;
    public bool Seen { get; set; } = false;
    public bool Delivered { get; set; } = false;
    public DateTime Date { get; set; } = DateTime.Now;
}
