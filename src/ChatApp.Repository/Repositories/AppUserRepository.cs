using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _dbcontext;

    public AppUserRepository(AppDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    public async Task<IEnumerable<string>> GetAllConnectionIdsForUserAsync(string id)
    {
        return await _dbcontext
            .UserConnections.Where(C => C.UserId == id)
            .Select(C => C.ConnectionId)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetAllGroupIdsAsync(string id)
    {
        return await _dbcontext
            .ChatParticipants.Where(CP => CP.UserId == id && CP.Chat.IsGroup)
            .Select(CP => CP.Chat.Id)
            .ToListAsync();
    }
}
