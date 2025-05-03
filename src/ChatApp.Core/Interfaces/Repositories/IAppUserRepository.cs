using ChatApp.Core.Entities;

namespace ChatApp.Core.Interfaces.Repositories;

public interface IAppUserRepository
{
    Task<IEnumerable<AppUser>> GetUsersNotInChatAsync(string chatId);
    Task<IEnumerable<string>> GetAllConnectionIdsForUserAsync(string id);
}
