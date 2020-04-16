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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

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
        [HttpPost]
        public IActionResult FancySearch(string fancysearch)
        {
            ViewBag.Answers = _datahandler.SearchInAnswers(fancysearch);

            ViewBag.Search = fancysearch;
            return View("FancySearch", _datahandler.SearchInData(fancysearch));
        }
        [Authorize]
        public IActionResult NewQuestion()
        {
            return View();
        }
        [Authorize]
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
                var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                model.UserID = _datahandler.GetUserIdForUsername(username);

                _datahandler.AddQuestion(model);
                return RedirectToAction("list", _datahandler.GetQuestions());
            }
            else
            {
                return View("NewQuestion");
            }
        }
        [Authorize]
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

        [Authorize]
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

            var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            comment.UserID = _datahandler.GetUserIdForUsername(username);
            _datahandler.AddComment(comment);
            return RedirectToAction("AnswersForQuestion", new RouteValueDictionary(new { action = "AnswersForQuestion", Id = id }));

        }

        [Authorize]
        public IActionResult NewAnswer(int id)
        {
            AnswerModel ans = new AnswerModel();
            ans.QuestionID = id;

            return View(ans);
        }

        [Authorize]
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
                var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                model.UserID = _datahandler.GetUserIdForUsername(username);

                _datahandler.AddAnswer(model, id);

                return RedirectToAction("AnswersForQuestion", new RouteValueDictionary(new { action = "AnswersForQuestion", Id = id }));
            }
            else
            {
                return View("NewAnswer");
            }
        }

        [Authorize]
        public IActionResult EditQuestion(int id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);
            return View("EditQuestion", q);
        }

        [Authorize]
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

        [Authorize]
        public IActionResult EditAnswer(int id)
        {
            AnswerModel q = _datahandler.GetAnswerByID(id);
            return View("EditAnswer", q);
        }

        [Authorize]
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
            try
            {
                var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                ViewBag.ActualUserID = _datahandler.GetUserIdForUsername(username);
            }
            catch { }

            if (questions.Count == 0)
            {
                questions = _datahandler.GetQuestions();
            }
            return View(questions);
        }

        public IActionResult AnswersForQuestion(int id)
        {
            _datahandler.IncreaseViews(id);
            try
            {
                var username = HttpContext.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
                ViewBag.ActualUserID = _datahandler.GetUserIdForUsername(username);
            }
            catch { }

            ViewBag.Question = _datahandler.GetQuestionByID(id);
            ViewBag.Ans = _datahandler.GetAnswersForQuestion(id);
            ViewBag.CommentQ = _datahandler.GetCommentsToQuestion(id);
            ViewBag.User = _datahandler.GetUsers();

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var tag in _datahandler.GetTagUrl(id))
            {
                items.Add(new SelectListItem { Text = tag.Url, Value = tag.Url });
            }

            ViewBag.Tag = items;
            ViewBag.Tags = _datahandler.GetTagUrl(id);
            return View();
        }

        [Authorize]
        public IActionResult DeleteAnswer(int id, int qid)
        {
            _datahandler.RemoveAnswer(id);
            return Redirect($"../AnswersForQuestion/{qid}");
        }

        public IActionResult SortByAttribute(string attribute)
        {
            return View("List", _datahandler.SortedDatas(attribute));
        }

        [Authorize]
        public IActionResult EditComment(int id)
        {
            CommentModel comment = _datahandler.GetCommentByID(id);
            return View(comment);
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditComment(int id, [FromForm(Name = "Message")] string message)
        {
            CommentModel comment = _datahandler.GetCommentByID(id);

            try
            {
                comment.Message = message;
                _datahandler.EditComment(id, comment);
                _datahandler.IncreaseNumberOfEdits(id);
                return Redirect($"../AnswersForQuestion/{comment.Question_ID}");
            }
            catch
            {
                return View("EditComment", comment);
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult DeleteQuestion(int id)
        {
            QuestionModel q = _datahandler.GetQuestionByID(id);

            return View("DeleteQuestion", q);
        }

        [Authorize]
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
        [Authorize]
        public IActionResult QuestionVote(int id, int voteValue, string currentpath)
        {
            _datahandler.ModifyQuestionVote(id, voteValue);
            return Redirect(currentpath);
        }
        [Authorize]
        public IActionResult AnswerVote(int id, int voteValue)
        {
            int questionID = _datahandler.GetAnswerByID(id).QuestionID;

            _datahandler.IncreaseViewsCorrection(questionID);
            _datahandler.ModifyAnswerVote(id, voteValue);
            return Redirect($"../AnswersForQuestion/{questionID}");
        }

        [Authorize]
        public IActionResult AddingTag(int id)
        {
            ViewBag.Tags = _datahandler.GetTags();

            ViewBag.questionId = id;

            TagModel tagModel = new TagModel();
            return View(tagModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddingTag([FromForm(Name = "Url")] string newTag, int questionId, string Url_i)
        {
            if (newTag == null || newTag == "")
            {
                newTag = Url_i;
            }

            if (!_datahandler.TagAlreadyOrdered(questionId, newTag))
            {
                _datahandler.AddTag(questionId, newTag);
            }
            return Redirect($"../AnswersForQuestion/{questionId}");
        }

        [Authorize]
        public IActionResult SelectFromTags([FromForm(Name = "Url_i")] string newTag, int questionId, string tag)
        {
            if (tag == null || tag == "")
            {
                tag = newTag;
            }

            if (!_datahandler.TagAlreadyOrdered(questionId, tag))
            {
                _datahandler.AddTag(questionId, tag);
            }
            return Redirect($"../AnswersForQuestion/{questionId}");

        }

        [Authorize]
        public IActionResult DeleteTag(string url, int questionID)
        {
            _datahandler.DeleteTag(url, questionID);
            return Redirect($"../Home/AnswersForQuestion/{questionID}");
        }

        [Authorize]
        public IActionResult DeleteComment(int id, int qid)
        {
            _datahandler.RemoveComment(id);
            return Redirect($"../AnswersForQuestion/{qid}");
        }

        public IActionResult Tags()
        {
            Dictionary<string, int> tagsWithVote = _datahandler.GetTagsWithCount();
            return View(tagsWithVote);
        }

        [Authorize]
        public IActionResult AcceptAnswer(int answerID, int questionID)
        {

            _datahandler.AcceptAnswer(answerID, questionID);
            return Redirect($"../Home/AnswersForQuestion/{questionID}");
        }

        public IActionResult AllUsers()
        {
            return View(_datahandler.GetAllUsers());
        }

        [Authorize]
        public IActionResult DetailsOfCurrentUser()
        {
            //collects all question that the user asked
            ViewBag.Questions = _datahandler.AllQuestionForUser(_datahandler.GetUserId(User.Identity.Name));

            //collects all answer that the user wrote
            ViewBag.Answers = _datahandler.AnswersWithQuestions(_datahandler.GetUserId(User.Identity.Name));

            //collects all comments that the user wrote
            ViewBag.Comments = _datahandler.CommentsWithQuestions(_datahandler.GetUserId(User.Identity.Name));

            return View();
        }
    }
}
