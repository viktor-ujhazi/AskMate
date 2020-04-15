using AskMateMVC.Models;
using System;
using System.Collections.Generic;

namespace AskMateMVC.Services
{
    public interface IDataHandler
    {
        public List<QuestionModel> GetQuestions();
        public List<AnswerModel> GetAnswers();
        public List<CommentModel> GetComments();
        public List<UserModel> GetUsers();
        public void AddQuestion(QuestionModel model);
        public void AddAnswer(AnswerModel model, int id);
        public void AddComment(CommentModel model);
        public void AddUser(UserModel model);
        public QuestionModel GetQuestionByID(int id);
        public AnswerModel GetAnswerByID(int id);
        public CommentModel GetCommentByID(int id);
        public List<AnswerModel> GetAnswersForQuestion(int id);
        public List<CommentModel> GetCommentsToQuestion(int id);
        public void RemoveQuestionById(int id);
        public void EditQuestion(int id, QuestionModel question);
        public void EditAnswer(int id, AnswerModel answer);
        public void EditComment(int id, CommentModel comment);
        public void RemoveAnswer(int id);
        public void RemoveComment(int id);
        public void ModifyQuestionVote(int id, int voteValue);
        public void ModifyAnswerVote(int id, int voteValue);
        public void IncreaseViews(int id);
        public List<QuestionModel> MostViewedQuestions();
        public List<QuestionModel> SortedDatas(string attribute);
        public List<QuestionModel> LatestQuestions();
        public List<QuestionModel> SearchInData(string searchedWord);
        public List<AnswerModel> SearchInAnswers(string searchedWord);
        public void IncreaseNumberOfEdits(int id);
        public void AddTag(int questionID, string url);
        public List<TagModel> GetTagUrl(int questionId);
        public void DeleteTag(string url, int questionID);
        public List<TagModel> GetTags();
        public bool TagAlreadyOrdered(int questionID, string url);
        public Dictionary<string, int> GetTagsWithCount();
        
        public void AcceptAnswer(int answerID, int questionID);
        public int GetUserIdForUsername(string username);
        public void IncreaseViewsCorrection(int id);
    }
}