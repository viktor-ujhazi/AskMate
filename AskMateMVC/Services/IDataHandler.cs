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
        public void AddAnswer(AnswerModel model);
        void SaveQuestions();
        void SaveAnswers();
        QuestionModel GetQuestionByID(Guid id);
        public List<AnswerModel> GetAnswersForQuestion(Guid id);
        public void RemoveQuestionById(Guid id);
        public void RemoveAnswersForQuestin(Guid id);
    }
}