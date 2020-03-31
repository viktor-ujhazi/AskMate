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

        public string cs = "Host=localhost;Username=postgres;Password=admin;Database=AskMate";

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
            Questions.Clear();
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
            Answers.Clear();
            var sql = "SELECT * FROM answers";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var a = reader["question_id"];
                        AnswerModel q = new AnswerModel
                        {
                            ID = (int)reader["answer_id"],
                            TimeOfAnswer = (DateTime)reader["answer_time"],
                            VoteNumber = (int)reader["answer_votenumber"],
                            QuestionID = (int)reader["question_id"],
                            Message = (string)reader["answer_message"],
                            Image = (string)reader["answer_imageurl"]
                        };
                        Answers.Add(q);
                    }

                };
            };
            return Answers;
        }

        public void AddQuestion(QuestionModel model)
        {

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO questions " +
                    "(question_time, " +
                    "question_viewnumber, " +
                    "question_votenumber, " +
                    "question_title, " +
                    "question_message, " +
                    "question_imageurl ) Values (@time, @view, @vote, @title, @message, @image)", conn))
                {
                    cmd.Parameters.AddWithValue("time", model.TimeOfQuestion);
                    cmd.Parameters.AddWithValue("view", model.ViewNumber);
                    cmd.Parameters.AddWithValue("vote", model.VoteNumber);
                    cmd.Parameters.AddWithValue("title", model.Title);
                    cmd.Parameters.AddWithValue("message", model.Message);
                    cmd.Parameters.AddWithValue("image", model.Image == "" ? DBNull.Value.ToString() : "");

                    cmd.ExecuteNonQuery();
                };
            };
            
        }

        public void AddAnswer(AnswerModel model, int id)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO answers " +
                    "(answer_time, " +
                    "answer_votenumber, " +
                    "question_id, " +
                    "answer_message, " +
                    "answer_imageurl ) Values (@time, @vote, @qid, @message, @image)", conn))
                {
                    cmd.Parameters.AddWithValue("time", model.TimeOfAnswer);
                    cmd.Parameters.AddWithValue("vote", model.VoteNumber);
                    cmd.Parameters.AddWithValue("qid", model.QuestionID);
                    cmd.Parameters.AddWithValue("message", model.Message);
                    cmd.Parameters.AddWithValue("image", model.Image == "" ? DBNull.Value.ToString() : "");

                    cmd.ExecuteNonQuery();
                };
            };
        }
        public QuestionModel GetQuestionByID(int id)
        {
            foreach(var question in GetQuestions())
            {
                if(question.ID==id)
                {
                    return question;
                }
            }
            return null;
        }

        public AnswerModel GetAnswerByID(int id)
        {
            foreach (var answer in GetAnswers())
            {
                if (answer.ID == id)
                {
                    return answer;
                }
            }
            return null;
        }

        public List<AnswerModel> GetAnswersForQuestion(int id)
        {
            List<AnswerModel> ResultAnswers = new List<AnswerModel>();
            foreach (var item in GetAnswers())
            {
                if (item.QuestionID.Equals(id))
                {
                    ResultAnswers.Add(item);
                }
            }
            return ResultAnswers;
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