using AskMateMVC.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Services
{
    public class SQLHandler : IDataHandler
    {
        //static string questionsFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Questions.csv");
        //static string answersFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Answers.csv");

        string cs = "Host=localhost;Username=postgres;Password=admin;Database=AskMate";

        List<QuestionModel> Questions { get; set; } = new List<QuestionModel>();
        List<AnswerModel> Answers { get; set; } = new List<AnswerModel>();

        //IWebHostEnvironment WebHostEnvironment { get; }


        public SQLHandler()
        {

            //LoadQuestion();
            //LoadAnswers();
        }

        public List<QuestionModel> GetQuestions()
        {

            var sql = "SELECT * FROM questions";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var a = reader["question_id"];
                        QuestionModel q = new QuestionModel
                        {
                            ID = (int)reader["question_id"],
                            TimeOfQuestion = (DateTime)reader["question_time"],
                            ViewNumber = (int)reader["question_viewnumber"],
                            VoteNumber = (int)reader["question_votenumber"],
                            Title = (string)reader["question_title"],
                            Message = (string)reader["question_message"],
                            Image = (string)reader["question_imageurl"]
                        };
                        Questions.Add(q);
                    }

                };
            };
            return Questions;
        }



        public List<AnswerModel> GetAnswers()
        {
            //throw new NotImplementedException();
            return null;
        }

        public void AddQuestion(QuestionModel model)
        {
            
        }

        public void AddAnswer(AnswerModel model, int id)
        {
            throw new NotImplementedException();
        }

        public QuestionModel GetQuestionByID(int id)
        {
            throw new NotImplementedException();
        }

        public AnswerModel GetAnswerByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<AnswerModel> GetAnswersForQuestion(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveQuestionById(int id)
        {
            throw new NotImplementedException();
        }

        public void EditQuestion(int id, QuestionModel question)
        {
            throw new NotImplementedException();
        }

        public void EditAnswer(int id, AnswerModel answer)
        {
            throw new NotImplementedException();
        }

        public void RemoveAnswer(int id)
        {
            throw new NotImplementedException();
        }

        public void ModifyQuestionVote(int id, int voteValue)
        {
            throw new NotImplementedException();
        }

        public void ModifyAnswerVote(int id, int voteValue)
        {
            throw new NotImplementedException();
        }

        public void IncreaseViews(int id)
        {
            throw new NotImplementedException();
        }

        public List<QuestionModel> MostViewedQuestions()
        {
            //throw new NotImplementedException();
            return new List<QuestionModel>();
        }
    }
}