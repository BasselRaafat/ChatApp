namespace ChatApp.Core.Entities;

public class Message : BaseEntity
{
    public string SenderId { get; set; } = default!;
    public string ReceiverId { get; set; } = default!;
    public string MessageText { get; set; } = default!;
    public bool Seen { get; set; } = default!;
    public DateTime Date { get; set; } = DateTime.Now;
}
