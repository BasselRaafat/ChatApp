using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Repositories;

public class AppUserRepository : IAppUserRepository
{
    private readonly AppDbContext _dbContext;

    public AppUserRepository(AppDbContext dbcontext)
    {
        _dbContext = dbcontext;
    }

    public async Task<IEnumerable<string>> GetAllConnectionIdsForUserAsync(string id)
    {
        return await _dbContext
            .UserConnections.Where(C => C.UserId == id)
            .Select(C => C.ConnectionId)
            .ToListAsync();
    }

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
