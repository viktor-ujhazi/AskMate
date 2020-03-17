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
        private readonly IDataHandler _dataloader;
        //private CsvHandler _csvHandler;
        

        public HomeController(ILogger<HomeController> logger, CsvHandler_old csvHandler, IDataHandler dataloader)
        {
            _logger = logger;
            //_csvHandler = csvHandler;
            _dataloader = dataloader;
            
        }

        public IActionResult Index()
        {
            //QuestionModel q1 = new QuestionModel();
            //q1.Title = "elso";
            //CsvDatabase.listOfQuestions.Add(q1);

            //QuestionModel q2 = new QuestionModel();
            //q2.Title = "masodik";
            //CsvDatabase.listOfQuestions.Add(q2);

            return View(_dataloader.LoadQuestion());
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult NewQuestion()
        {
            return View();
        }

        public IActionResult QuestionDetails(Guid id)
        {
            QuestionModel q=new QuestionModel();
            foreach(var i in CsvDatabase.listOfQuestions)
            {
                if(i.ID.Equals(id))
                {
                    q = i;
                }
            }
            Console.WriteLine(id+"--------"+q.Title);
            return View(q);
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewQuestion(QuestionModel model)
        {

            _logger.LogInformation($"{model}");
            
            _dataloader.SaveQuestions(model);
            

            return View("list",_dataloader.GetQuestions());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
