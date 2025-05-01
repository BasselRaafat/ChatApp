using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Repositories;

public class ChatRepository : GenericRepository<Chat>, IChatRepository
{
    public ChatRepository(AppDbContext dbContext)
        : base(dbContext) { }

    public async Task<IEnumerable<Chat>> GetAllUserChatsAsync(string userId)
    {
        // _=await _dbContext
        //     .Chats.Where(C => C.ChatParticipants.SingleOrDefault(CP => CP.UserId == userId) != null)
        //     .Include(C => C.LastMessageSent)
        //     .OrderByDescending(C => C.LastTimeActive)
        //     .ToListAsync();

        return await _dbContext
            .ChatParticipants.Where(cp => cp.UserId == userId)
            .Select(cp => cp.Chat)
            .Include(C => C.LastMessageSent)
            .OrderByDescending(C => C.LastTimeActive)
            .ToListAsync();
    }

    public async Task<Chat?> GetChatWithMessagesAsync(string id)
    {
        return await _dbContext
            .Chats.Include(C => C.Messages)
            .Where(C => C.Id == id)
            .FirstOrDefaultAsync();
    }
}
