namespace ChatApp.MVC.Hubs.Interfaces;

public interface IChatClient
{
    Task ReceiveMessage(
        string chatId,
        string userId,
        string message,
        DateTime timestamp,
        string displayName
    );
}
