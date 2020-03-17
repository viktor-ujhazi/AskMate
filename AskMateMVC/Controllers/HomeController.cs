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
        private readonly IDataHandler _datahandler;
        //private CsvHandler _csvHandler;
        

        public HomeController(ILogger<HomeController> logger, IDataHandler datahandler)
        {
            _logger = logger;
            //_csvHandler = csvHandler;
            _datahandler = datahandler;
            
        }

        public IActionResult Index()
        {
            //QuestionModel q1 = new QuestionModel();
            //q1.Title = "elso";
            //CsvDatabase.listOfQuestions.Add(q1);

            //QuestionModel q2 = new QuestionModel();
            //q2.Title = "masodik";
            //CsvDatabase.listOfQuestions.Add(q2);

            return View(_datahandler.GetQuestions());
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NewQuestion()
        {
            return View();
        }
        //[HttpGet("{ID}")]
        public IActionResult QuestionDetails(Guid id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            return View("QuestionDetails",q);
        }

        public IActionResult List()
        {
            return View(_datahandler.GetQuestions());
        }
        

        [HttpPost]
        public IActionResult NewQuestion(QuestionModel model)
        {

            _logger.LogInformation($"{model}");
            
            _datahandler.SaveQuestions(model);
            

            return View("list",_datahandler.GetQuestions());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
