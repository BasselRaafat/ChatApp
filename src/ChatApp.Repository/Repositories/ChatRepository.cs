using System.Runtime.CompilerServices;
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
        return await _dbContext
            .Chats.Where(C => C.ChatParticipants.SingleOrDefault(CP => CP.UserId == userId) != null)
            .Include(C => C.LastMessageSent)
            .Include(c => c.ChatParticipants)
            .ThenInclude(cp => cp.User)
            .OrderByDescending(C => C.LastTimeActive)
            .ToListAsync();

        // return await _dbContext
        //     .ChatParticipants.Where(cp => cp.UserId == userId)
        //     .Include(CP => CP.Chat)
        //     .Select(cp => cp.Chat)
        //     .Include(C => C.LastMessageSent)
        //     .OrderByDescending(C => C.LastTimeActive)
        //     .ToListAsync();
    }

    public async Task<Chat?> GetChatWithMessagesAsync(string id)
    {
        return await _dbContext
            .Chats.Include(C => C.Messages)
            .ThenInclude(m => m.Sender)
            .Where(C => C.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task MarkMessagesAsSeenAsync(string chatId, string userId)
    {
        var messages = _dbContext.ChatMessages.Where(m =>
            m.ChatId == chatId && m.SenderId != userId && !m.Seen
        );

        foreach (var msg in messages)
        {
            msg.Seen = true;
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetAllGroupIdsAsync(string userId)
    {
        return await _dbContext
            .Chats.Where(c => c.IsGroup)
            .Where(C => C.ChatParticipants.SingleOrDefault(CP => CP.UserId == userId) != null)
            .Select(C => C.Id)
            .ToListAsync();
        // return await _dbContext
        //     .ChatParticipants.Where(CP => CP.UserId == userId && CP.Chat.IsGroup)
        //     .Select(CP => CP.Chat.Id)
        //     .ToListAsync();
    }
}
