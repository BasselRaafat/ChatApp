using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces.Service;

public interface IChatService
{
    Task SendMessage(string chatId, string userId, string messageText);
    
    Task<IEnumerable<ChatMessage>> GetMessages(string chatId);
    
    Task<IEnumerable<Chat>> GetUserChats(string userId);
    
    Task JoinChat(string chatId, string userId);
}