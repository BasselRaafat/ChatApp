using System.Security.Claims;
using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Repositories;
using ChatApp.Core.Interfaces.Service;
using ChatApp.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.MVC.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly IUserConnectionRepository _userConnectionRepo;
    private readonly IChatRepository _chatRepo;

    public ChatHub(
        IChatService chatService,
        IUserConnectionRepository userConnectionRepo,
        IChatRepository chatRepo
    )
    {
        _chatService = chatService;
        _userConnectionRepo = userConnectionRepo;
        _chatRepo = chatRepo;
    }

    #region Send Message

    public async Task SendMessage(string chatId, string message)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not authenticated.");

        await _chatService.SendMessage(chatId, userId, message);

        var timestamp = DateTime.UtcNow;
        await Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, userId, message, timestamp);
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

        if (exists is not null)
        {
            await _userConnectionRepo.AddAsync(
                new UserConnection { UserId = userId, ConnectionId = connectionId }
            );
            await _userConnectionRepo.SaveChangesAsync();
        }

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
