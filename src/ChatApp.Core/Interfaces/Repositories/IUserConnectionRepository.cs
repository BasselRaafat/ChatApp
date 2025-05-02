using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces.Repositories;

public interface IUserConnectionRepository
{
    Task<UserConnection?> GetByConnectionIdAsync(string connectionId);
    Task AddAsync(UserConnection userConnection);
    Task<IEnumerable<UserConnection>> GetAllUserConnections(string userId);
    void Delete(UserConnection userConnection);
    Task SaveChangesAsync();
}
