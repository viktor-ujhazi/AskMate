using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AskMateMVC.Models;
using AskMateMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AskMateMVC.Controllers
{
    public class UserController : Controller
    {

        private readonly IDataHandler _datahandler;

        public UserController(IDataHandler datahandler)
        {
            _datahandler = datahandler;
        }
        public IActionResult Index()
        {
            return Redirect($"../Home/Index");
        }

        public IActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewUser(UserModel user)
        {
            try
            {
                _datahandler.AddUser(user);
            }
            catch (Npgsql.PostgresException)
            {

                return Redirect("InvalidUsername");
            }

            return Redirect($"../Home/Index");
        }

        public IActionResult InvalidUsername()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync([FromForm] string username, [FromForm] string password)
        {
            if (!_datahandler.IsValidUser(username, password))
            {
                return RedirectToAction("Index", "Home");
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return RedirectToAction("Index", "Home");
        }
    }
}