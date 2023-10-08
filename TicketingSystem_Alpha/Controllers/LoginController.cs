using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BL.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TicketingSystem_Alpha.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly IUsersService _usersService;
        public LoginController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, CancellationToken cancellationToken = default)
        {
            var claims = await _usersService.Authenticate(userName, password, cancellationToken);
            if(claims is not null && claims.Count>0)
            {
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}
