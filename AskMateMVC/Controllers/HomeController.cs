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
            return View(_datahandler.MostViewedQuestions());
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
                _datahandler.AddQuestion(model);
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
                _datahandler.AddAnswer(model);

                return RedirectToAction("AnswersForQuestion", new RouteValueDictionary(new { action = "AnswersForQuestion", Id = model.QuestionID }));
            }
            else
            {
                return View("NewAnswer");
            }
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
                q.Title = title;
                q.Message = message;
                _datahandler.RemoveQuestionById(id);
                _datahandler.AddQuestion(q);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("EditQuestion", q);
            }
        }
        public IActionResult EditAnswer(Guid id)
        {
            AnswerModel q = _datahandler.GetAnswerByID(id);
            return View("EditAnswer", q);
        }

        [HttpPost]

        public ActionResult EditAnswer(Guid id, [FromForm(Name = "Message")] string message)
        {
            AnswerModel q = _datahandler.GetAnswerByID(id);
            try
            {

                q.Message = message;
                _datahandler.RemoveAnswer(id);
                _datahandler.AddAnswer(q);
                return RedirectToAction("Index");
            }
            catch
            {
                return View("EditAnswer", q);
            }
        }
        public IActionResult List(List<QuestionModel> questions)
        {
            if (questions.Count == 0)
                questions = _datahandler.GetQuestions();
            return View(questions);
        }

        public IActionResult AnswersForQuestion(Guid id)
        {
            var questionView = _datahandler.GetQuestionByID(id);
            questionView.IncreaseViews();
            _datahandler.SaveQuestions();
            ViewBag.Question = questionView;
            ViewBag.Ans = _datahandler.GetAnswersForQuestion(id);
            return View();
        }

        public IActionResult DeleteAnswer(Guid id, Guid qid)
        {
            _datahandler.RemoveAnswer(id);
            _datahandler.SaveAnswers();
            return Redirect($"../AnswersForQuestion/{qid}");
        }

        public ActionResult SortingByTitle()
        {
            List<QuestionModel> list = _datahandler.GetQuestions();
            list = list.OrderBy(o => o.Title).ToList();
            return View("List", list);
        }

        public ActionResult SortingByTime()
        {
            List<QuestionModel> list = _datahandler.GetQuestions();
            list = list.OrderBy(o => o.TimeOfQuestion).ToList();
            list.Reverse();
            return View("List", list);
        }

        public ActionResult SortingByVote()
        {
            List<QuestionModel> list = _datahandler.GetQuestions();
            list = list.OrderBy(o => o.VoteNumber).ToList();
            return View("List", list);
        }

        public ActionResult SortingByView()
        {
            List<QuestionModel> list = _datahandler.GetQuestions();
            list = list.OrderBy(o => o.ViewNumber).ToList();
            return View("List", list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DeleteQuestion(Guid id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);

            return View("DeleteQuestion", q);
        }

        [HttpPost]
        public ActionResult DeleteQuestion(QuestionModel q)
        {
            try
            {
                _datahandler.RemoveQuestionById(q.ID);
                _datahandler.RemoveAnswersForQuestin(q.ID);
                _datahandler.SaveAnswers();
                _datahandler.SaveQuestions();
                return RedirectToAction("Index");
            }
            catch
            {
                return View("EditQuestion", q);
            }
        }
        public ActionResult QuestionVote(Guid id, int voteValue)
        {
            var questionToVote = _datahandler.GetQuestionByID(id);

            if (voteValue == 1)
            {
                questionToVote.VoteNumber++;
            }
            if (voteValue == 0)
            {
                questionToVote.VoteNumber--;
            }
            _datahandler.SaveQuestions();
            return Redirect($"../List/");
        }
        public ActionResult AnswerVote(Guid id, int voteValue)
        {
            var answerToVote = _datahandler.GetAnswerByID(id);

            if (voteValue == 1)
            {
                answerToVote.VoteNumber++;
            }
            if (voteValue == 0)
            {
                answerToVote.VoteNumber--;
            }
            _datahandler.SaveAnswers();
            return Redirect($"../AnswersForQuestion/{answerToVote.QuestionID}");
        }
    }

}
