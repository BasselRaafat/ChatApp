namespace ChatApp.Core.Entities;

public class GroupMessage : BaseEntity
{
    public string SenderId { get; set; } = default!;
    public AppUser Sender { get; set; } = default!;

    public Group Group { get; set; } = default!;
    public string GroupId { get; set; } = default!;

    public string MessageText { get; set; } = default!;
    public bool Seen { get; set; } = default!;
    public DateTime Date { get; set; } = DateTime.Now;
}
