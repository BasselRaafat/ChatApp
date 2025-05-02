using System.Security.Claims;
using ChatApp.Core.Interfaces.Service;
using ChatApp.Repository.Data;
using ChatApp.Core.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Repository.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly AppDbContext _db;

    public ChatHub(IChatService chatService, AppDbContext db)
    {
        _chatService = chatService;
        _db = db;
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
        
        
        var exists = await _db.UserConnections
            .AnyAsync(c => c.ConnectionId == connectionId);

        if (!exists)
        {
            _db.UserConnections.Add(new UserConnection
            {
                UserId = userId,
                ConnectionId = connectionId
            });
            await _db.SaveChangesAsync();
        }

        await base.OnConnectedAsync();
    }

    #endregion

    #region On Disconnected

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var connectionId = Context.ConnectionId;

        var connection = await _db.UserConnections
            .FirstOrDefaultAsync(c => c.ConnectionId == connectionId);

        if (connection != null)
        {
            _db.UserConnections.Remove(connection);
            await _db.SaveChangesAsync();
        }

        await base.OnDisconnectedAsync(exception);
    }

    #endregion

    #region Join Chat

    public async Task JoinChat(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);

        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            throw new UnauthorizedAccessException("User not authenticated.");

        await _chatService.JoinChat(chatId, userId);
    }

    #endregion
}
