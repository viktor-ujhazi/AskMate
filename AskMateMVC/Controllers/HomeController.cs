using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMateMVC.Models;

namespace AskMateMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            QuestionModel q1 = new QuestionModel();
            q1.Title = "elso";
            CsvDatabase cs = new CsvDatabase();
            cs.listOfQuestions.Add(q1);

            QuestionModel q2 = new QuestionModel();
            q2.Title = "masodik";
            cs.listOfQuestions.Add(q2);
            return View("list",cs);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NewQuestion()
        {
            return View();
        }

        public IActionResult DisplayQuestion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewQuestion(QuestionModel model)
        {
            _logger.LogInformation($"{model.Title}\n{model.Message}\n{model.TimeOfQuestion}");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
