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
        Dictionary<string, bool> AscOrderings = new Dictionary<string, bool>();

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
                            Image = (string)reader["answer_image"]
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
                    cmd.Parameters.AddWithValue("image", model.Image == null ? DBNull.Value.ToString() : model.Image);

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
                    "answer_image ) Values (@time, @vote, @qid, @message, @image)", conn))
                {
                    cmd.Parameters.AddWithValue("time", model.TimeOfAnswer);
                    cmd.Parameters.AddWithValue("vote", model.VoteNumber);
                    cmd.Parameters.AddWithValue("qid", id);
                    cmd.Parameters.AddWithValue("message", model.Message);
                    cmd.Parameters.AddWithValue("image", model.Image == null ? DBNull.Value.ToString() : model.Image);

                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void AddComment(CommentModel model)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO comment_s " +
                    "(question_id, " +
                    "answer_id, " +
                    "comment_message, " +
                    "comment_time, " +
                    "edited_number) Values (@qid, @aid, @message, @time, @edit)", conn))
                {
                    if (model.Question_ID != null)
                    {
                        cmd.Parameters.AddWithValue("qid", model.Question_ID);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("qid", DBNull.Value);
                    }
                    if (model.Answer_ID != null)
                    {
                        cmd.Parameters.AddWithValue("aid", model.Answer_ID);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("aid", DBNull.Value);
                    }
                    cmd.Parameters.AddWithValue("message", model.Message);
                    cmd.Parameters.AddWithValue("time", model.SubmissionTime);
                    cmd.Parameters.AddWithValue("edit", model.EditedNumber);

                    cmd.ExecuteNonQuery();
                };
            };
        }

        public QuestionModel GetQuestionByID(int id)
        {
            foreach (var question in GetQuestions())
            {
                if (question.ID == id)
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
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"DELETE FROM questions WHERE question_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void EditQuestion(int id, QuestionModel question)
        {
            string sql = $"UPDATE questions " +
                    $"SET question_title = '{question.Title}'," +
                    $"question_message = '{question.Message}', " +
                    $"question_imageurl = '{question.Image}' " +
                    $"WHERE question_id={id} ";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void EditAnswer(int id, AnswerModel answer)
        {
            string sql = $"UPDATE answers " +
                    $"SET answer_message = '{answer.Message}', " +
                    $"answer_image = '{answer.Image}' " +
                    $"WHERE answer_id={id} ";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void RemoveAnswer(int id)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"DELETE FROM answers WHERE answer_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void ModifyQuestionVote(int id, int voteValue)
        {
            string op="-";
            if (voteValue == 1)
            {
                op = "+";
            }
            
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"UPDATE questions SET question_votenumber = question_votenumber {op} 1 WHERE question_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
            
        }

        public void ModifyAnswerVote(int id, int voteValue)
        {
            string op = "-";
            if (voteValue == 1)
            {
                op = "+";
            }

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"UPDATE answers SET answer_votenumber = answer_votenumber {op} 1 WHERE answer_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void IncreaseViews(int id)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"UPDATE questions SET question_viewnumber = question_viewnumber + 1 WHERE question_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };

        }

        public List<QuestionModel> MostViewedQuestions()
        {

            List<QuestionModel> mostViewedQuestions = new List<QuestionModel>();
            var sql = "SELECT * FROM questions ORDER BY question_viewnumber DESC LIMIT 5";
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
                        mostViewedQuestions.Add(q);
                    }

                };
            };
            return mostViewedQuestions;

            
        }

        public List<QuestionModel> SortedDatas(string attribute)
        {
            List<QuestionModel> questions = new List<QuestionModel>();


            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                if ((AscOrderings.ContainsKey(attribute) && AscOrderings[attribute] == false) || !AscOrderings.ContainsKey(attribute))
                {
                    using (NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM questions ORDER BY {attribute}", cn))
                    {
                        var reader = command.ExecuteReader();
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
                            questions.Add(q);
                        }
                    };
                    AscOrderings[attribute] = true;
                }
                else
                {
                    using (NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM questions ORDER BY {attribute} DESC", cn))
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                           
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
                            questions.Add(q);
                        }
                    };
                    AscOrderings[attribute] = false;
                }
            };
            return questions;
        }
        public List<QuestionModel> LatestQuestions()
        {

            List<QuestionModel> latestQuestions = new List<QuestionModel>();
            var sql = "SELECT * FROM questions ORDER BY question_time DESC LIMIT 5";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        
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
                        latestQuestions.Add(q);
                    }

                };
            };
            return latestQuestions;


        }
        public List<QuestionModel> SearchInData(string searchedWord)
        {

            List<QuestionModel> latestQuestions = new List<QuestionModel>();
            var sql = $"SELECT * FROM questions WHERE question_title LIKE '%{searchedWord}%' OR question_message LIKE '%{searchedWord}%';";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        
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
                        latestQuestions.Add(q);
                    }

                };
            };
            return latestQuestions;


        }
    }
}