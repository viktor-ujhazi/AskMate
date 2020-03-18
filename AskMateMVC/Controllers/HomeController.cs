using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AskMateMVC.Models;
using AskMateMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
        [HttpPost]
        public IActionResult NewQuestion(QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                _datahandler.SaveQuestions(model);
                return RedirectToAction("list", _datahandler.GetQuestions());
            }
            else
            {
                return View("NewQuestion");
            }
        }

        public IActionResult NewAnswer(Guid id)
        {
            AnswerModel ans = new AnswerModel();
            ans.QuestionID = id;

            return View(ans);
        }

        [HttpPost]
        public IActionResult NewAnswer(Guid id, AnswerModel model)
        {
            model.QuestionID = id;
            model.ID = Guid.NewGuid();
            if (ModelState.IsValid)
            {
                _datahandler.SaveAnswers(model);
                return RedirectToAction("AnswersForQuestion", new RouteValueDictionary(new { action = "AnswersForQuestion", Id = id }) );
            }
            else
            {
                return View("NewAnswer");
            }
        }
       

        public IActionResult QuestionDetails(Guid id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            return View("QuestionDetails", q);
        }
        public IActionResult EditQuestion(Guid id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            return View("EditQuestion", q);
        }
        
        [HttpPost]

        public ActionResult EditQuestion(Guid id, [FromForm(Name = "Title")] string title, [FromForm(Name = "Message")] string message)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            try
            {
                Console.WriteLine();
                return RedirectToAction();
            }
            catch
            {
                return View("EditQuestion", q);
            }
        }
        public IActionResult List()
        {
            return View(_datahandler.GetQuestions());
        }

        public IActionResult AnswersForQuestion(Guid id)
        {
            Console.WriteLine("-------"+id);
            ViewBag.Question = _datahandler.GetQuestionByID(id);
            ViewBag.Ans = _datahandler.GetAnswersForQuestion(id);
            return View();
        }

        //[HttpPost]
        //public IActionResult AnswersForQuestion([FromForm(Name = "QuestionID")] Guid id)
        //{
        //    Console.WriteLine("-------" + id);
        //    ViewBag.Question = _datahandler.GetQuestionByID(id);
        //    ViewBag.Ans = _datahandler.GetAnswersForQuestion(id);
        //    return View();
        //}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
