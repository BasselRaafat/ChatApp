using ChatApp.Core.Entities;
using ChatApp.Repository.Data;

namespace ChatApp.Repository.Repositories;

public class ChatParticipantRepository
{
    private readonly AppDbContext _dbContext;

    public ChatParticipantRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ChatParticipant model)
    {
        await _dbContext.AddAsync(model);
    }

    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();
}
