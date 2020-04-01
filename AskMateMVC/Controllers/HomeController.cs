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
using System.IO;

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
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"); 
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{fileName}"), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                model.Image = fileName;
                            }

                        }
                    }
                }
                _datahandler.AddQuestion(model);
                return RedirectToAction("list", _datahandler.GetQuestions());
            }
            else
            {
                return View("NewQuestion");
            }
        }

        public IActionResult NewComment(int id, int ansID = 0)
        {
            var comment = new CommentModel();
            if (ansID == 0)
            {
                comment.Question_ID = id;
                return View(comment);
            }
            else
            {
                comment.Answer_ID = ansID;
                return View(comment);
            }
        }
        [HttpPost]
        public IActionResult NewComment(int id, CommentModel comment, int ansID = 0)
        {

            if (ansID == 0)
            {
                comment.Question_ID = id;
            }
            else
            {
                comment.Question_ID = id;
                comment.Answer_ID = ansID;
            }

            _datahandler.AddComment(comment);
            return RedirectToAction("AnswersForQuestion", new RouteValueDictionary(new { action = "AnswersForQuestion", Id = id }));

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
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{fileName}"), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                model.Image = fileName;
                            }

                        }
                    }
                }
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

        public IActionResult EditQuestion(int id, [FromForm(Name = "Title")] string title, [FromForm(Name = "Message")] string message, [FromForm(Name = "Image")] string image)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            try
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/images/{fileName}"), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                q.Image = fileName;
                            }

                        }
                    }
                }
                q.Title = title;
                q.Message = message;
                
                _datahandler.EditQuestion(id, q);

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
                _datahandler.EditAnswer(id, ans);
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
            ViewBag.CommentA = _datahandler.GetCommentsToAnswers(id);
            ViewBag.CommentQ = _datahandler.GetCommentsToQuestion(id);
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

        public IActionResult EditComment(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditComment(int id, [FromForm(Name = "Message")] string message)
        {
            CommentModel comment = _datahandler.GetCommentByID(id);
            try
            {
                comment.Message = message;
                _datahandler.EditComment(id, comment);
                return Redirect($"../AnswersForQuestion/{comment.Question_ID}");
            }
            catch
            {
                return View("EditComment", comment);
            }
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



        public IActionResult AddingTag(int id)
        {
            ViewBag.questionId = id;
            TagModel tag = new TagModel();
            return View(tag);
        }

        [HttpPost]
        public IActionResult AddingTag(TagModel tag,int questionId)
        {
            _datahandler.AddTag(questionId, tag.Url);
            return Redirect($"../AnswersForQuestion/{questionId}");
        }
        public IActionResult DeleteComment(int id, int qid)
        {

            _datahandler.RemoveComment(id);
            return Redirect($"../AnswersForQuestion/{qid}");
        }
    }
}
