using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces.Repositories;

public interface IChatRepository : IGenericRepository<Chat>
{
    Task<IEnumerable<Chat>> GetAllUserChatsAsync(string userId);
    Task<Chat?> GetChatWithMessagesAsync(string id);

    Task MarkMessagesAsSeenAsync(string chatId, string userId);
}
