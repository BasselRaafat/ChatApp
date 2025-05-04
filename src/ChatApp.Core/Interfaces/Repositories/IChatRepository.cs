using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces.Repositories;

public interface IChatRepository : IGenericRepository<Chat>
{
    Task<IEnumerable<Chat>> GetAllUserChatsAsync(string userId);
    Task<Chat?> GetChatWithMessagesAsync(string id);
    Task<IEnumerable<string>> GetAllGroupIdsAsync(string id);
    Task MarkMessagesAsSeenAsync(string chatId, string userId);
    Task<IEnumerable<Chat>> GetAllUserPrivateChat(string userId);
    Task<IEnumerable<Chat>> GetAllUserGroupChats(string userId);


}
