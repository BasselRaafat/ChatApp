using System.Security.Claims;
using ChatApp.Core.Entities;
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

        public ChatController(IChatService chatService, IHubContext<ChatHub> chatHub)
        {
            _chatService = chatService;
            _chatHub = chatHub;
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
            ViewBag.OnlineUsers = new[]
            {
                new
                {
                    Name = "User1",
                    Mood = "Happy",
                    AvatarUrl = "https://bootdey.com/img/Content/avatar/avatar1.png",
                    Status = "online",
                },
                new
                {
                    Name = "User2",
                    Mood = "Busy",
                    AvatarUrl = "https://bootdey.com/img/Content/avatar/avatar2.png",
                    Status = "busy",
                },
            }; // Replace with real online user data later

            return View("Index", userChat); // Explicitly specify the view name
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
    }
}

