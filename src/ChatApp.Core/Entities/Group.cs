namespace ChatApp.Core.Entities;

public class Group : BaseEntity
{
    public string Name { get; set; } = default!;
    public DateTime LastTimeActive { get; set; } = DateTime.UtcNow;
    public ICollection<AppUser> Users { get; set; } = [];
    public ICollection<GroupMessage> GroupMessages { get; set; } = [];
}
