namespace ChatApp.Core.Interfaces.Repositories;

public interface IAppUserRepository
{
    Task<IEnumerable<string>> GetAllConnectionIdsForUserAsync(string id);
    Task<IEnumerable<string>> GetAllGroupIdsAsync(string id);
}
