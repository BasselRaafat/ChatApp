using ChatApp.Core.Entities;
using ChatApp.Core.Params;

namespace ChatApp.Core.Interfaces.Service;

public interface IAuthService
{
    Task RegisterAsync(AuthParams parameters);
    Task<AppUser> ValidateUserAsync(AuthParams parameters);
}
