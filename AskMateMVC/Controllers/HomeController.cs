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
using Npgsql;

namespace AskMateMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataHandler _datahandler;

        public HomeController(ILogger<HomeController> logger, IDataHandler datahandler)
        {
            _logger = logger;
            _datahandler = datahandler;
        }

        public IActionResult Index()
        {
            ViewBag.Answers = _datahandler.GetAnswers();
            ViewBag.Questions = _datahandler.LatestQuestions();
            return View();
        }
        [HttpPost]
        public IActionResult Index(string search)
        {
            List<QuestionModel> result = _datahandler.SearchInData(search);
            return View("List", result);
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

        public IActionResult NewComment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewComment(QuestionModel model, CommentModel comment)
        {
            comment.Question_ID = model.ID;

            _datahandler.AddComment(comment);
            return RedirectToAction("list", _datahandler.GetQuestions());

        }
        public IActionResult NewAnswer(int id)
        {
            AnswerModel ans = new AnswerModel();
            ans.QuestionID = id;

            return View(ans);
        }

        [HttpPost]
        public IActionResult NewAnswer(int id, AnswerModel model)
        {
            //model=_utility.CreateAnswer(id, model);

            if (ModelState.IsValid)
            {
                _datahandler.AddAnswer(model, id);

                return RedirectToAction("AnswersForQuestion", new RouteValueDictionary(new { action = "AnswersForQuestion", Id = id }));
            }
            else
            {
                return View("NewAnswer");
            }
        }


        public IActionResult EditQuestion(int id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            return View("EditQuestion", q);
        }

        [HttpPost]

        public IActionResult EditQuestion(int id, [FromForm(Name = "Title")] string title, [FromForm(Name = "Message")] string message,[FromForm(Name = "Image")] string image)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            try
            {
                q.Title = title;
                q.Message = message;
            q.Image = image;
                _datahandler.EditQuestion(id,q);
               
                return RedirectToAction("list");
            }
            catch
            {
                return View("EditQuestion", q);
            }
        }
        public IActionResult EditAnswer(int id)
        {
            AnswerModel q = _datahandler.GetAnswerByID(id);
            return View("EditAnswer", q);
        }

        [HttpPost]

        public IActionResult EditAnswer(int id, [FromForm(Name = "Message")] string message, [FromForm(Name = "Image")] string image)
        {
            AnswerModel ans = _datahandler.GetAnswerByID(id);
            try
            {
                ans.Message = message;
                _datahandler.EditAnswer(id,ans);
                return Redirect($"../AnswersForQuestion/{ans.QuestionID}");
            }
            catch
            {
                return View("EditAnswer", ans);
            }
        }
        public IActionResult List(List<QuestionModel> questions)
        {
            if (questions.Count == 0)
            {
                questions = _datahandler.GetQuestions();
            }
            return View(questions);
        }

        public IActionResult AnswersForQuestion(int id)
        {

            _datahandler.IncreaseViews(id);
            ViewBag.Question = _datahandler.GetQuestionByID(id);
            ViewBag.Ans = _datahandler.GetAnswersForQuestion(id);
            return View();
        }

        public IActionResult DeleteAnswer(int id, int qid)
        {
            _datahandler.RemoveAnswer(id);
            return Redirect($"../AnswersForQuestion/{qid}");
        }

        public IActionResult SortByAttribute(string attribute)
        {
            return View("List", _datahandler.SortedDatas(attribute));
        }



        //public IActionResult SortingByTitle()
        //{
        //    List<QuestionModel> list = _datahandler.GetQuestions();
        //    list = list.OrderBy(o => o.Title).ToList();
        //    return View("List", list);
        //}

        //public IActionResult SortingByTime()
        //{
        //    List<QuestionModel> list = _datahandler.GetQuestions();
        //    list = list.OrderBy(o => o.TimeOfQuestion).ToList();
        //    list.Reverse();
        //    return View("List", list);
        //}

        //public IActionResult SortingByVote()
        //{
        //    List<QuestionModel> list = _datahandler.GetQuestions();
        //    list = list.OrderBy(o => o.VoteNumber).ToList();
        //    return View("List", list);
        //}

        //public IActionResult SortingByView()
        //{
        //    List<QuestionModel> list = _datahandler.GetQuestions();
        //    list = list.OrderBy(o => o.ViewNumber).ToList();
        //    return View("List", list);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DeleteQuestion(int id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);

            return View("DeleteQuestion", q);
        }

        [HttpPost]
        public IActionResult DeleteQuestion(QuestionModel q)
        {
            try
            {
                _datahandler.RemoveQuestionById(q.ID);
                return RedirectToAction("list");
            }
            catch
            {
                return View("EditQuestion", q);
            }
        }
        public IActionResult QuestionVote(int id, int voteValue)
        {
            _datahandler.ModifyQuestionVote(id, voteValue);
            return Redirect($"../List/");
        }
        public IActionResult AnswerVote(int id, int voteValue)
        {
            _datahandler.ModifyAnswerVote(id, voteValue);
            return Redirect($"../AnswersForQuestion/{_datahandler.GetAnswerByID(id).QuestionID}");
        }
    }
}
