using AskMateMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Services
{
    public class Utility
    {
        IDataHandler _dataHandler;
        
        public Utility(IDataHandler datahandler)
        {
            _dataHandler = datahandler;
        }
        public AnswerModel CreateAnswer(int id, AnswerModel model)
        {
            model.QuestionID = id;
            //model.ID = Guid.NewGuid();
            return model;
        }
        public List<AnswerModel> GetAnswersForQuestion(int id)
        {
            List<AnswerModel> ResultAnswers = new List<AnswerModel>();
            foreach (var item in _dataHandler.GetAnswers())
            {
                if (item.QuestionID.Equals(id))
                {
                    ResultAnswers.Add(item);
                }
            }
            return ResultAnswers;
        }
        public void RemoveAnswersForQuestion(int id)
        {
            var answersToRemove = GetAnswersForQuestion(id);
            if (answersToRemove.Count > 0)
            {
                foreach (var answer in answersToRemove)
                {
                    _dataHandler.RemoveAnswer(answer.ID);
                    
                }
            }
        }
        public List<QuestionModel> MostViewedQuestions()
        {
            List<QuestionModel> list = _dataHandler.GetQuestions();
            List<QuestionModel> resultList = new List<QuestionModel>();
            list = list.OrderBy(o => o.ViewNumber).ToList();
            if (list.Count > 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    resultList.Add(list[i]);
                }
                resultList.Reverse();
                return resultList;
            }
            else
            {
                list.Reverse();
                return list;
            }

        }
    }
}
