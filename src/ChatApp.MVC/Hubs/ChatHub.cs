using System.Security.Claims;
using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Service;
using ChatApp.MVC.Hubs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.MVC.Hubs;

[Authorize]
public class ChatHub : Hub<IChatHub>
{
    private readonly IChatService _chatService;
    private readonly IUserConnectionRepository _userConnectionRepo;
    private readonly IChatRepository _chatRepo;
    private readonly UserManager<AppUser> _userManager;

    public ChatHub(
        IChatService chatService,
        IUserConnectionRepository userConnectionRepo,
        IChatRepository chatRepo,
        UserManager<AppUser> userManager
    )
    {
        _chatService = chatService;
        _userConnectionRepo = userConnectionRepo;
        _chatRepo = chatRepo;
        _userManager = userManager;
    }

    #region Send Message

    public async Task SendMessage(string chatId, string message)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not authenticated.");

        // Send and persist the message
        await _chatService.SendMessage(chatId, userId, message);

        // Get display name of sender
        var user = await _userManager.FindByIdAsync(userId); // You must have this method
        var displayName = user?.DisplayName ?? "Unknown";

        var timestamp = DateTime.UtcNow;
        // var chat =
        //     await _chatRepo.GetChatWithMessagesAsync(chatId)
        //     ?? throw new Exception("chatnot found");
        // var reciverId =
        //     (chat.ChatParticipants.FirstOrDefault(cp => cp.UserId != userId)?.UserId)
        //     ?? throw new Exception("user not found");
        // var UserConnections = await _userConnectionRepo.GetAllUserConnections(reciverId);
        // Send message to group
        await Clients.Group(chatId).ReceiveMessage(chatId, userId, message, timestamp, displayName);
    }

    #endregion

    #region On Connected
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not authenticated.");

        var connectionId = Context.ConnectionId;

        var exists = await _userConnectionRepo.GetByConnectionIdAsync(connectionId);

        if (exists is null)
        {
            await _userConnectionRepo.AddAsync(
                new UserConnection { UserId = userId, ConnectionId = connectionId }
            );
        }
        await _userConnectionRepo.SaveChangesAsync();
        var groupsIds = await _chatRepo.GetAllGroupIdsAsync(userId);
        foreach (var groupId in groupsIds)
            await Groups.AddToGroupAsync(connectionId, groupId);

        await base.OnConnectedAsync();
    }

    #endregion

    #region On Disconnected

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;

        var connection = await _userConnectionRepo.GetByConnectionIdAsync(connectionId);

        if (connection != null)
        {
            _userConnectionRepo.Delete(connection);
            await _userConnectionRepo.SaveChangesAsync();
        }

        await base.OnDisconnectedAsync(exception);
    }

    #endregion

    #region Join Chat

    public async Task JoinChat(string chatId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not authenticated.");

        await _chatService.JoinChat(chatId, userId);

        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    #endregion
}
