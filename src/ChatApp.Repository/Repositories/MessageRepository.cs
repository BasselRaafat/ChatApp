using ChatApp.Core.Entities;
using ChatApp.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Repositories;

public class MessageRepository : GenericRepository<ChatMessage>
{
    public MessageRepository(AppDbContext dbContext)
        : base(dbContext) { }

    public async Task<IEnumerable<ChatMessage>> GetMessagesWithSenderAsync(
        string userId,
        string chatId
    )
    {
        return await _dbContext
            .ChatMessages.Where(c => c.SenderId == userId && c.ChatId == chatId)
            .Include(c => c.Sender)
            .ToListAsync();
    }
}
