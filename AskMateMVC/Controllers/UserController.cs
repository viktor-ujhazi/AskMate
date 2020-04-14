using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AskMateMVC.Models;
using AskMateMVC.Services;
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

        public IActionResult Login()
        {
            return View();
        }
    }
}