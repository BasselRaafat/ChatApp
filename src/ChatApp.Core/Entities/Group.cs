namespace ChatApp.Core.Entities;

public class Group : BaseEntity
{
    public string Name { get; set; } = default!;
    public ICollection<AppUser> Users { get; set; } = [];
    public ICollection<GroupMessage> GroupMessages { get; set; } = [];
}
