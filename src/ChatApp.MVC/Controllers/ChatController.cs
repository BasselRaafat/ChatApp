using System.Security.Claims;
using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Service;
using ChatApp.MVC.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IChatRepository _chatRepository;
        private readonly IAppUserRepository _userRepository;

        public ChatController(IChatService chatService,
                              IHubContext<ChatHub> chatHub,
                              IChatRepository chatRepository,
                              IAppUserRepository userRepository)
        {
            _chatService = chatService;
            _chatHub = chatHub;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
        }

        #region Index
        public async Task<IActionResult> Index(string chatId)
        {
            if (string.IsNullOrEmpty(chatId))
                return RedirectToAction("List");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var userChats = await _chatService.GetUserChats(userId);
            var userChat = userChats.FirstOrDefault(c => c.Id == chatId);

            if (userChat == null)
                return NotFound();

            // Populate ViewBag with messages and online users
            ViewBag.Messages = await _chatService.GetMessages(chatId);
            var connectionIds = await _userRepository.GetAllConnectionIdsForUserAsync(userId);
            
            // Join the chat group (SignalR)
            foreach (var connectionId in connectionIds)
                await _chatHub.Groups.AddToGroupAsync(connectionId, chatId);
           
            await _chatService.JoinChat(chatId, userId);

            return View("Index", userChat);
        }
        #endregion

        #region Get Lists
        public async Task<IActionResult> List()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var chats = await _chatService.GetUserChats(userId);
            return View(chats);
        }
        #endregion

        #region Send Message
        [HttpPost]
        public async Task<IActionResult> SendMessage(string chatId, string messageText)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (string.IsNullOrEmpty(chatId) || string.IsNullOrEmpty(messageText))
                return BadRequest("Chat ID and message text are required.");

            var chat = await _chatRepository.GetByIdAsync(chatId);
            if (chat == null)
                return NotFound("Chat not found.");

            // Create the message
            var message = new ChatMessage
            {
                ChatId = chatId,
                SenderId = userId,
                MessageText = messageText,
                Date = DateTime.Now,
                Seen = false,
                Delivered = true
            };

            // Save the message using your service
            await _chatService.SendMessage(chatId, userId, messageText);

            // Send via SignalR
            if (chat.IsGroup)
            {
                await _chatHub.Clients.Group(chatId).SendAsync("ReceiveMessage", message);
            }
            else
            {
                var receiver = chat.ChatParticipants.FirstOrDefault(p => p.UserId != userId);
                if (receiver != null)
                {
                    var connectionIds = await _userRepository.GetAllConnectionIdsForUserAsync(receiver.UserId);
                  
                        foreach (var connectionId in connectionIds)
                        {
                            await _chatHub.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                        }
                }
            }

            return Ok();
        }
        #endregion

        #region Create Chat
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string userId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized();

            // Create the chat
            var chat = new Chat();
            
            // Add the current user as a participant
            var participants = new List<ChatParticipant>
            {
                new ChatParticipant
                {
                    ChatId = chat.Id,
                    UserId = currentUserId
                }
            };

            // Add selected users as participants
            // if (isGroup && userIds != null)
            // {
            //     foreach (var userId in userIds)
            //     {
            //         if (!string.IsNullOrEmpty(userId) && userId != currentUserId)
            //         {
            //             participants.Add(new ChatParticipant
            //             {
            //                 ChatId = chat.Id,
            //                 UserId = userId
            //             });
            //         }
            //     }
            // }
                // For one-on-one chat, add the selected user
                participants.Add(new ChatParticipant
                {
                    ChatId = chat.Id,
                    UserId = userId,
                });

            chat.ChatParticipants = participants;
            // Save the chat and participants
            await _chatRepository.AddAsync(chat);
            await _chatRepository.SaveChangesAsync();
            // Add participants to SignalR group if it's a group chat
            // if (chat.IsGroup)
            // {
            //     foreach (var participant in participants)
            //     {
            //         var connectionIds = await _userRepository.GetAllConnectionIdsForUserAsync(participant.UserId);
            //         if (connectionIds != null)
            //         {
            //             foreach (var connectionId in connectionIds)
            //             {
            //                 await _chatHub.Groups.AddToGroupAsync(connectionId, chat.Id);
            //             }
            //         }
            //     }
            //     await _chatHub.Clients.Group(chat.Id).SendAsync("UserJoined", currentUserId);
            // }
            //
            return RedirectToAction("Index", new { chatId = chat.Id });
        }
        #endregion
    }
}