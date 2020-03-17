using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMateMVC.Models;
using AskMateMVC.Services;

namespace AskMateMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CsvHandler _csvHandler;
        

        public HomeController(ILogger<HomeController> logger, CsvHandler csvHandler)
        {
            _logger = logger;
            _csvHandler = csvHandler;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NewQuestion()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult NewQuestion(QuestionModel model)
        {

            _logger.LogInformation($"{model}");
            
            _csvHandler.SaveQuestions(model);
            

            return View("list");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
