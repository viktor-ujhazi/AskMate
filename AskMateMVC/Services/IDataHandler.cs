using AskMateMVC.Models;
using System;
using System.Collections.Generic;

namespace AskMateMVC.Services
{
    public interface IDataHandler
    {
        List<QuestionModel> LoadQuestion();
        void LoadAnswers();
        List<QuestionModel> GetQuestions();
        List<AnswerModel> GetAnswers();
        public void AddQuestion(QuestionModel model);
        public void AddAnswer(AnswerModel model, int id);
        void SaveQuestions();
        void SaveAnswers();
        QuestionModel GetQuestionByID(int id);
        AnswerModel GetAnswerByID(int id);
        //public List<AnswerModel> GetAnswersForQuestion(Guid id);
        public void RemoveQuestionById(int id);
        //public void RemoveAnswersForQuestion(Guid id);
        public void RemoveAnswer(int id);
        //List<QuestionModel> MostViewedQuestions();
    }
}