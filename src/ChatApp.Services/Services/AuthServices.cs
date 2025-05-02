using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Service;
using ChatApp.Core.Params;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Service.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;

    public AuthService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task RegisterAsync(AuthParams parameters)
    {
        var user = new AppUser
        {
            DisplayName = parameters.DispalyName,
            UserName = parameters.UserName,
            Email = parameters.Email,
        };

        var result = await _userManager.CreateAsync(user, parameters.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }

    public async Task<AppUser> ValidateUserAsync(AuthParams parameters)
    {
        var user = await _userManager.FindByEmailAsync(parameters.Email);
        if (user == null)
        {
            throw new Exception("Invalid email or password");
        }

        var isValidPassword = await _userManager.CheckPasswordAsync(user, parameters.Password);
        if (!isValidPassword)
        {
            throw new Exception("Invalid email or password");
        }

        return user;
    }
}
