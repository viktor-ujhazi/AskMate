using AskMateMVC.Models;
using System;
using System.Collections.Generic;

namespace AskMateMVC.Services
{
    public interface IDataHandler
    {
        public List<QuestionModel> GetQuestions();
        public List<AnswerModel> GetAnswers();
        public void AddQuestion(QuestionModel model);
        public void AddAnswer(AnswerModel model, int id);
        public QuestionModel GetQuestionByID(int id);
        public AnswerModel GetAnswerByID(int id);
        public List<AnswerModel> GetAnswersForQuestion(int id);
        public void RemoveQuestionById(int id);
        public void EditQuestion(int id, QuestionModel question);
        public void EditAnswer(int id, AnswerModel answer);
        public void RemoveAnswer(int id);
        public void ModifyQuestionVote(int id, int voteValue);
        public void ModifyAnswerVote(int id, int voteValue);
        public void IncreaseViews(int id);
        public List<QuestionModel> MostViewedQuestions();
        public List<QuestionModel> SortedDatas(string attribute);
        public List<QuestionModel> LatestQuestions();
        public List<QuestionModel> SearchInData(string searchedWord);
    }
}