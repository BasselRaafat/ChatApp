using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Service;

namespace ChatApp.Service.Services
{
    public class ChatService : IChatService
    {
        private readonly IGenericRepository<ChatMessage> _chatMessageRepository;
        private readonly IChatRepository _chatRepository;

        public ChatService(
            IGenericRepository<ChatMessage> chatMessageRepository,
            IChatRepository chatRepository
        )
        {
            _chatMessageRepository =
                chatMessageRepository
                ?? throw new ArgumentNullException(nameof(chatMessageRepository));
            _chatRepository =
                chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }

        #region Send Message
        public async Task SendMessage(string chatId, string userId, string messageText)
        {
            if (string.IsNullOrWhiteSpace(chatId))
                throw new ArgumentException("Chat ID cannot be empty.", nameof(chatId));

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            if (string.IsNullOrWhiteSpace(messageText))
                throw new ArgumentException("Message cannot be empty.", nameof(messageText));

            var message = new ChatMessage
            {
                ChatId = chatId,
                SenderId = userId,
                MessageText = messageText,
                Date = DateTime.Now,
                Seen = false,
                Delivered = false,
            };

            await _chatMessageRepository.AddAsync(message);

            var chat = await _chatRepository.GetByIdAsync(message.ChatId);
            if (chat is null)
                throw new ArgumentNullException(nameof(chat), "Chat not found.");

            chat.LastMessageSentId = message.Id;
            chat.LastTimeActive = message.Date;
            _chatRepository.Update(chat);

            await _chatRepository.SaveChangesAsync();
            await _chatMessageRepository.SaveChangesAsync();
        }
        #endregion

        #region Get Chat Messages
        public async Task<IEnumerable<ChatMessage>> GetMessages(string chatId)
        {
            var chat = await _chatRepository.GetChatWithMessagesAsync(chatId);
            
            
            if (chat == null)
                throw new ArgumentNullException(nameof(chat), "Chat not found.");
            if (chat.Messages == null)
                throw new ArgumentNullException(nameof(chat.Messages), "Messages not found.");

            var messages = chat.Messages.OrderBy(m => m.Date);
            

            return messages;
        }
        #endregion

        #region GetUserChats
        public async Task<IEnumerable<Chat>> GetUserChats(string userId)
        {
            return await _chatRepository.GetAllUserChatsAsync(userId);
        }
        #endregion

        #region JoinChat
        public async Task JoinChat(string chatId, string userId)
        {
            await _chatRepository.MarkMessagesAsSeenAsync(chatId, userId);
        }
        #endregion
    }
}
