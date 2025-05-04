// using ChatApp.Core.Entities;
// using Microsoft.AspNetCore.Mvc;
// using ChatApp.MVC.Models;
// using ChatApp.MVC.Services;
// using Microsoft.AspNetCore.Identity;
//
// namespace ChatApp.MVC.Controllers
// {
//     public class ContactsController : Controller
//     {
//         private readonly IUserRoleStore<AppUser> _userRepo;
//
//
//         // Inject your service/repository to retrieve user data
//         public ContactsController( IUserRoleStore<AppUser> userRepo)
//         {
//             _userRepo = userRepo;
//         }
//
//         // Method to return all users in the system
//         public IActionResult Index()
//         {
//             // Fetch all users from the database via a service or repository
//             var users = _userRepo.Get();
//
//             // Pass the list of users to the view
//             return View("UserList", users);
//         }
//     }
// }