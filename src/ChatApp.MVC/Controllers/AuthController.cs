using ChatApp.Core.Entities;
using ChatApp.Core.Interfaces.Service;
using ChatApp.Core.Params;
using ChatApp.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AuthController(
        IAuthService authService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager
    )
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LogInViewModel());
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LogInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var parameters = new AuthParams { Email = model.Email, Password = model.Password };

            var user = await _authService.ValidateUserAsync(parameters);

            // Perform sign-in using SignInManager
            var signInResult = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                false,
                false
            );

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var parameters = new AuthParams
            {
                Email = model.Email,
                Password = model.Password,
                UserName = model.UserName,
                DispalyName = model.FirstName + " " + model.LastName,
            };

            await _authService.RegisterAsync(parameters);

            // Sign in the user after registration
            var user = await _userManager.FindByEmailAsync(model.Email);
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }
}
