using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Repositories;

public class UserConnectionRepository : IUserConnectionRepository
{
    private readonly AppDbContext _dbContext;

    public UserConnectionRepository(AppDbContext dbContest)
    {
        _dbContext = dbContest;
    }

    public async Task AddAsync(UserConnection userConnection)
    {
        await _dbContext.UserConnections.AddAsync(userConnection);
    }

    public void Delete(UserConnection userConnection)
    {
        _dbContext.UserConnections.Remove(userConnection);
    }

    public async Task<IEnumerable<UserConnection>> GetAllUserConnections(string userId)
    {
        return await _dbContext.UserConnections.Where(uc => uc.UserId == userId).ToListAsync();
    }

    public async Task<UserConnection?> GetByConnectionIdAsync(string connectionId)
    {
        return await _dbContext.UserConnections.FirstOrDefaultAsync(c =>
            c.ConnectionId == connectionId
        );
    }

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
