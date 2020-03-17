using AskMateMVC.Models;
using System.Collections.Generic;

namespace AskMateMVC.Services
{
    public interface IDataLoader
    {
        List<QuestionModel> LoadQuestion();
        void LoadAnswers();
        List<QuestionModel> GetQuestions();
        List<AnswerModel> GetAnswers();
        void SaveQuestions(QuestionModel model);
        void SaveAnswers(AnswerModel model);


    }
}