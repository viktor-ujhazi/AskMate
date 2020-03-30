using AskMateMVC.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AskMateMVC.Services
{
    public class CsvHandler : IDataHandler
    {

        static string questionsFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Questions.csv");
        static string answersFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Answers.csv");
        List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        List<AnswerModel> Answers { get; set; } = new List<AnswerModel>();

        //IWebHostEnvironment WebHostEnvironment { get; }


        public CsvHandler()
        {
            LoadQuestion();
            LoadAnswers();

        }
        public List<QuestionModel> GetQuestions()
        {
            return Questions;
        }
        public List<AnswerModel> GetAnswers()
        {
            return Answers;
        }
        public List<QuestionModel> LoadQuestion()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(questionsFileName);
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                foreach (var row in lines)
                {
                    String[] Fields = CSVParser.Split(row);
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        Fields[i] = Fields[i].TrimStart(' ', '"');
                        Fields[i] = Fields[i].TrimEnd('"');
                    }
                    QuestionModel model = new QuestionModel
                    {
                        ID = int.Parse(Fields[0]),
                        TimeOfQuestion = DateTime.Parse(Fields[1]),
                        ViewNumber = int.Parse(Fields[2]),
                        VoteNumber = int.Parse(Fields[3]),
                        Title = Fields[4],
                        Message = Fields[5],
                        Image = Fields[6]
                    };
                    Questions.Add(model);
                }
            }
            catch (Exception)
            {
                return new List<QuestionModel>();
            }
            return Questions;

        }
        public void LoadAnswers()
        {
            string[] lines = System.IO.File.ReadAllLines(answersFileName);
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            try
            {
                foreach (var row in lines)
                {
                    String[] Fields = CSVParser.Split(row);
                    // clean up the fields (remove " and leading spaces)
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        Fields[i] = Fields[i].TrimStart(' ', '"');
                        Fields[i] = Fields[i].TrimEnd('"');

                    }
                    AnswerModel model = new AnswerModel
                    {
                        ID = int.Parse(Fields[0]),
                        TimeOfAnswer = DateTime.Parse(Fields[1]),
                        VoteNumber = int.Parse(Fields[2]),
                        QuestionID = int.Parse(Fields[3]),
                        Message = Fields[4],
                        Image = Fields[5]
                    };
                    Answers.Add(model);
                }

            }
            catch (Exception)
            {
                Console.WriteLine();
            }
        }
        public void AddQuestion(QuestionModel model)
        {
            model.ID = CreateQuestionID();
            Questions.Add(model);
            SaveQuestions();
        }
        public void SaveQuestions()
        {
            var csv = new StringBuilder();

            foreach (var item in GetQuestions())
            {
                csv.AppendLine(item.ToString());
            }

            File.WriteAllText(questionsFileName, csv.ToString());

        }
        public void AddAnswer(AnswerModel model, int id)
        {
            model.ID = CreateAnswerID();
            model.QuestionID = id;
            Answers.Add(model);
            SaveAnswers();
        }
        public void SaveAnswers()
        {

            var csv = new StringBuilder();

            foreach (var item in GetAnswers())
            {
                csv.AppendLine(item.ToString());
            }

            File.WriteAllText(answersFileName, csv.ToString());

        }
        public QuestionModel GetQuestionByID(int id)
        {
            return Questions.Where(q => q.ID == id).FirstOrDefault();
        }
        public AnswerModel GetAnswerByID(int id)
        {
            return Answers.Where(q => q.ID == id).FirstOrDefault();
        }
        //public List<AnswerModel> GetAnswersForQuestion(Guid id)
        //{
        //    List<AnswerModel> ResultAnswers = new List<AnswerModel>();
        //    foreach (var item in Answers)
        //    {
        //        if (item.QuestionID.Equals(id))
        //        {
        //            ResultAnswers.Add(item);
        //        }
        //    }
        //    //ResultAnswers.AddRange(Answers.Where(a => a.QuestionID == id));
        //    return ResultAnswers;
        //}
        public void RemoveQuestionById(int id)
        {
            var questionToRemove = GetQuestionByID(id);
            Questions.Remove(questionToRemove);
        }
        //public void RemoveAnswersForQuestion(Guid id)
        //{
        //    var answersToRemove = GetAnswersForQuestion(id);
        //    if (answersToRemove.Count > 0)
        //    {
        //        foreach (var answer in answersToRemove)
        //        {
        //            Answers.Remove(answer);
        //        }
        //    }
        //}
        public void RemoveAnswer(int id)
        {
            var answerToRemove = Answers.Where(q => q.ID == id).FirstOrDefault();
            Answers.Remove(answerToRemove);
        }
        //public List<QuestionModel> MostViewedQuestions()
        //{
        //    List<QuestionModel> list = GetQuestions();
        //    List<QuestionModel> resultList = new List<QuestionModel>();
        //    list = list.OrderBy(o => o.ViewNumber).ToList();
        //    if (list.Count > 10)
        //    {
        //        for (int i = 0; i < 10; i++)
        //        {
        //            resultList.Add(list[i]);
        //        }
        //        resultList.Reverse();
        //        return resultList;
        //    }
        //    else
        //    {
        //        list.Reverse();
        //        return list;
        //    }

        //}

        private int CreateAnswerID()
        {
            List<AnswerModel> sortedList = GetAnswers();
            sortedList.Sort(delegate (AnswerModel a1, AnswerModel a2)
            {
                int compareID = a1.ID.CompareTo(a2.ID);
                if (compareID == 0)
                {
                    return a2.ID.CompareTo(a1.ID);
                }
                return compareID;
            });
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].ID != i)
                {
                    return i;
                }
            }
            return sortedList.Count;
        }
        private int CreateQuestionID()
        {
            List<QuestionModel> sortedList = GetQuestions();
            sortedList.Sort(delegate (QuestionModel q1, QuestionModel q2)
            {
                int compareID = q1.ID.CompareTo(q2.ID);
                if (compareID == 0)
                {
                    return q2.ID.CompareTo(q1.ID);
                }
                return compareID;
            });
            for (int i = 0; i < sortedList.Count; i++)
            {
                if (sortedList[i].ID != i)
                {
                    return i;
                }
            }
            return sortedList.Count;
        }
    }
}
