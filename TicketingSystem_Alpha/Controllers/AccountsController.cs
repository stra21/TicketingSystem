using BL.Enums;
using BL.Interfaces;
using DL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketingSystem_Alpha.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUsersService _usersService;
        public AccountsController(IUsersService userService)
        {
            _usersService = userService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            await HttpContext.SignOutAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user, CancellationToken cancellationToken)
        {
            user.UserType = (int)SystemRolesEnum.Guest;
            var createdUser = await _usersService.CreateUser(user, cancellationToken);
            return RedirectToAction("Login", "Login");
        }
        public async Task<IActionResult> Users(CancellationToken cancellationToken)
        {
            var users = await _usersService.GetUsers(x => true, cancellationToken);
            return View(users);
        }
        public async Task<IActionResult> User(int id, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUser(x => x.Id == id, cancellationToken) ?? new();
            List<SelectListItem> userType = new List<SelectListItem>()
            {
                new SelectListItem(SystemRolesEnum.Guest.ToString(),((int)SystemRolesEnum.Guest).ToString(),user.UserType==(int)SystemRolesEnum.Guest),
                new SelectListItem(SystemRolesEnum.Admin.ToString(),((int)SystemRolesEnum.Admin).ToString(),user.UserType ==(int)SystemRolesEnum.Admin),
                new SelectListItem(SystemRolesEnum.ERPTeamMember.ToString(),((int)SystemRolesEnum.ERPTeamMember).ToString(), user.UserType==(int) SystemRolesEnum.ERPTeamMember),
            };
            ViewBag.userType = userType;
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> User(User user, CancellationToken cancellationToken)
        {
            if (user.Id == 0)
            {
                var createdUser = await _usersService.CreateUser(user, cancellationToken);
            }
            else
            {
                _ = await _usersService.UpdateUser(user, cancellationToken);
            }
            return RedirectToAction("Users");
        }
    }
}
