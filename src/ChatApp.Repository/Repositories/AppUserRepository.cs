using ChatApp.Core.Entities;
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

    public async Task<IEnumerable<AppUser>> GetUsersNotInChatAsync(string chatId)
    {
        return await _dbContext
            .Users.Where(u =>
                !_dbContext.ChatParticipants.Any(cp => cp.ChatId == chatId && cp.UserId == u.Id)
            )
            .Select(u => new AppUser
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Email = u.Email,
            })
            .ToListAsync();
    }

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
