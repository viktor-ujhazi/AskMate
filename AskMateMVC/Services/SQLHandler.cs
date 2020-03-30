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
            NpgsqlConnection con = new NpgsqlConnection(cs);
            con.Open();
            var sql = "SELECT version()";

            using var cmd = new NpgsqlCommand(sql, con);

            var version = cmd.ExecuteScalar().ToString();
            Console.WriteLine($"PostgreSQL version: {version}");



            //LoadQuestion();
            //LoadAnswers();
        }

        public List<QuestionModel> GetQuestions()
        {
            throw new NotImplementedException();
        }

        public List<AnswerModel> GetAnswers()
        {
            throw new NotImplementedException();
        }

        public void AddQuestion(QuestionModel model)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}