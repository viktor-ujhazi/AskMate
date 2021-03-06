﻿using AskMateMVC.Models;
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
        List<CommentModel> Comments { get; set; } = new List<CommentModel>();
        List<TagModel> Tags { get; set; } = new List<TagModel>();
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
            var sql = "SELECT * FROM questions ORDER BY question_time DESC";
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
                            UserID = (int)reader["user_id"],
                            TimeOfQuestion = (DateTime)reader["question_time"],
                            ViewNumber = (int)reader["question_viewnumber"],
                            VoteNumber = (int)reader["question_votenumber"],
                            Title = (string)reader["question_title"],
                            Message = (string)reader["question_message"],
                            Image = (string)reader["question_imageurl"],
                            //AcceptAnswerID = (int)reader["accept_answer_id"]
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
                        AnswerModel q = new AnswerModel
                        {
                            ID = (int)reader["answer_id"],
                            UserID = (int)reader["user_id"],
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
        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();
            var sql = "SELECT * FROM users";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        UserModel u = new UserModel
                        {
                            ID = (int)reader["user_id"],
                            Name = (string)reader["user_name"],
                            Password = (string)reader["user_password"],
                            Reputation = (int)reader["user_reputation"],
                            TimeOfRegistration = (DateTime)reader["user_registration_date"]
                        };
                        users.Add(u);
                    }
                };
            };
            return users;
        }

        public List<TagModel> GetTags()
        {
            Tags.Clear();
            var sql = "SELECT * FROM tags";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TagModel t = new TagModel
                        {
                            ID = (int)reader["tag_id"],
                            Url = (string)reader["tag_name"],

                        };
                        Tags.Add(t);
                    }

                };
            };
            return Tags;
        }
        public List<CommentModel> GetComments()
        {
            Comments.Clear();
            var sql = "SELECT * FROM comment_s";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CommentModel c = new CommentModel
                        {
                            UserID = (int)reader["user_id"],
                            ID = (int)reader["comment_id"],
                            Message = (string)reader["comment_message"],
                            SubmissionTime = (DateTime)reader["comment_time"],
                            EditedNumber = (int)reader["edited_number"]
                        };
                        if (reader["question_id"] is DBNull)
                        {
                            c.Question_ID = null;
                        }
                        else
                        {
                            c.Question_ID = (int?)reader["question_id"];
                        }
                        if (reader["answer_id"] is DBNull)
                        {
                            c.Answer_ID = null;
                        }
                        else
                        {
                            c.Answer_ID = (int?)reader["answer_id"];
                        }

                        Comments.Add(c);
                    }
                };
            };
            return Comments;
        }


        public void AddQuestion(QuestionModel model)
        {

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO questions " +
                    "(user_id," +
                    "question_time, " +
                    "question_viewnumber, " +
                    "question_votenumber, " +
                    "question_title, " +
                    "question_message, " +
                    "question_imageurl ) Values (@user_id,@time, @view, @vote, @title, @message, @image)", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", model.UserID);
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
                    "(user_id," +
                    "answer_time, " +
                    "answer_votenumber, " +
                    "question_id, " +
                    "answer_message, " +
                    "answer_image ) Values (@user_id,@time, @vote, @qid, @message, @image)", conn))
                {
                    cmd.Parameters.AddWithValue("user_id", model.UserID);
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
                    "(user_id," +
                    "question_id, " +
                    "answer_id, " +
                    "comment_message, " +
                    "comment_time, " +
                    "edited_number) Values (@user_id,@qid, @aid, @message, @time, @edit)", conn))
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
                    cmd.Parameters.AddWithValue("user_id", model.UserID);
                    cmd.Parameters.AddWithValue("message", model.Message);
                    cmd.Parameters.AddWithValue("time", model.SubmissionTime);
                    cmd.Parameters.AddWithValue("edit", model.EditedNumber);
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void AddUser(UserModel model)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO users " +
                    "(user_name," +
                    "user_password," +
                    "user_registration_date, " +
                    "user_reputation) Values (@user_name,@user_password,@user_registration_date, @user_reputation)", conn))
                {
                    cmd.Parameters.AddWithValue("user_registration_date", model.TimeOfRegistration);
                    cmd.Parameters.AddWithValue("user_name", model.Name);
                    cmd.Parameters.AddWithValue("user_password", model.Password);
                    cmd.Parameters.AddWithValue("user_reputation", model.Reputation);

                    cmd.ExecuteNonQuery();
                };
            };
        }

        public QuestionModel GetQuestionByID(int id)
        {
            var sql = $"SELECT * FROM questions WHERE question_id = {id}";
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
                            UserID = (int)reader["user_id"],
                            TimeOfQuestion = (DateTime)reader["question_time"],
                            ViewNumber = (int)reader["question_viewnumber"],
                            VoteNumber = (int)reader["question_votenumber"],
                            Title = (string)reader["question_title"],
                            Message = (string)reader["question_message"],
                            Image = (string)reader["question_imageurl"],

                        };
                        if (reader["accept_answer_id"] is DBNull)
                        {
                            q.AcceptAnswerID = 0;
                        }
                        else
                        {
                            q.AcceptAnswerID = (int)reader["accept_answer_id"];
                        }

                        return q;
                    }
                };
            };
            return null;
        }

        public AnswerModel GetAnswerByID(int id)
        {
            var sql = $"SELECT * FROM answers WHERE answer_id = {id}";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AnswerModel q = new AnswerModel
                        {
                            ID = (int)reader["answer_id"],
                            UserID = (int)reader["user_id"],
                            TimeOfAnswer = (DateTime)reader["answer_time"],
                            VoteNumber = (int)reader["answer_votenumber"],
                            QuestionID = (int)reader["question_id"],
                            Message = (string)reader["answer_message"],
                            Image = (string)reader["answer_image"]
                        };
                        return q;
                    }
                };
            };
            return null;
        }

        public List<AnswerModel> GetAnswersForQuestion(int id)
        {
            List<AnswerModel> ResultAnswers = new List<AnswerModel>();
            var sql = $"SELECT * FROM answers WHERE question_id = {id} ORDER BY answer_time DESC";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AnswerModel q = new AnswerModel
                        {
                            ID = (int)reader["answer_id"],
                            UserID = (int)reader["user_id"],
                            TimeOfAnswer = (DateTime)reader["answer_time"],
                            VoteNumber = (int)reader["answer_votenumber"],
                            QuestionID = (int)reader["question_id"],
                            Message = (string)reader["answer_message"],
                            Image = (string)reader["answer_image"]
                        };
                        ResultAnswers.Add(q);
                    }

                };
            };
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
            int questionID = GetAnswerByID(id).QuestionID;
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"DELETE FROM answers WHERE answer_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
            
            IncreaseViewsCorrection(questionID);
        }

        public void ModifyQuestionVote(int id, int voteValue, string currentpath)
        {
            string op = "-";
            int userID = GetUserIDByQuestionID(id);
            if (voteValue == 1)
            {
                op = "+";
                ModifyReputation(userID, 5);
            }
            else
            {
                ModifyReputation(userID, -2);
            }


            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"UPDATE questions SET question_votenumber = question_votenumber {op} 1 WHERE question_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
            if (currentpath.Contains("AnswersForQuestion"))
            {
                IncreaseViewsCorrection(id);
            }

        }

        public void ModifyAnswerVote(int id, int voteValue)
        {
            string op = "-";
            int userID = GetUserIDByAnswerID(id);
            if (voteValue == 1)
            {
                op = "+";
                ModifyReputation(userID, 10);
            }
            else
            {
                ModifyReputation(userID, -2);
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
        public void IncreaseViewsCorrection(int id)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"UPDATE questions SET question_viewnumber = question_viewnumber - 1 WHERE question_id={id} ", conn))
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
                            UserID = (int)reader["user_id"],
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
                                UserID = (int)reader["user_id"],
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
                                UserID = (int)reader["user_id"],
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
                            UserID = (int)reader["user_id"],
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
            //var sql = $"SELECT * FROM questions WHERE question_title LIKE '%{searchedWord}%' OR question_message LIKE '%{searchedWord}%';";
            var sql = $"SELECT * " +
                $"FROM questions " +
                $"WHERE question_id IN" +
                $"( SELECT q.question_id" +
                $" FROM questions q " +
                $"FULL OUTER JOIN answers a " +
                $"ON q.question_id = a.question_id " +
                $"WHERE LOWER(q.question_title) LIKE LOWER('%{searchedWord}%') " +
                $"OR LOWER(q.question_message) LIKE LOWER('%{searchedWord}%') " +
                $"OR LOWER(a.answer_message) LIKE LOWER('%{searchedWord}%') GROUP BY q.question_id, a.question_id)";
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
                            UserID = (int)reader["user_id"],
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

        public List<AnswerModel> SearchInAnswers(string searchedWord)
        {
            var resultAnswers = new List<AnswerModel>();
            var sql = $"SELECT * " +
                $"FROM answers " +
                $"WHERE LOWER(answer_message) LIKE LOWER('%{searchedWord}%')";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        AnswerModel q = new AnswerModel
                        {
                            ID = (int)reader["answer_id"],
                            UserID = (int)reader["user_id"],
                            TimeOfAnswer = (DateTime)reader["answer_time"],
                            VoteNumber = (int)reader["answer_votenumber"],
                            QuestionID = (int)reader["question_id"],
                            Message = (string)reader["answer_message"],
                            Image = (string)reader["answer_image"]
                        };
                        resultAnswers.Add(q);
                    }

                };
            };
            return resultAnswers;
        }
        public List<CommentModel> GetCommentsToQuestion(int id)
        {
            List<CommentModel> ResultComments = new List<CommentModel>();

            var sql = $"SELECT * FROM comment_s WHERE question_id = {id}";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        CommentModel c = new CommentModel
                        {
                            UserID = (int)reader["user_id"],
                            ID = (int)reader["comment_id"],
                            Message = (string)reader["comment_message"],
                            SubmissionTime = (DateTime)reader["comment_time"],
                            EditedNumber = (int)reader["edited_number"]
                        };
                        if (reader["question_id"] is DBNull)
                        {
                            c.Question_ID = null;
                        }
                        else
                        {
                            c.Question_ID = (int?)reader["question_id"];
                        }
                        if (reader["answer_id"] is DBNull)
                        {
                            c.Answer_ID = null;
                        }
                        else
                        {
                            c.Answer_ID = (int?)reader["answer_id"];
                        }

                        ResultComments.Add(c);
                    }
                };
            };
            return ResultComments;

        }

        public void EditComment(int id, CommentModel comment)
        {
            var upDate = DateTime.Now;
            string sql = $"UPDATE comment_s " +
                    $"SET comment_message = '{comment.Message}' , " +
                    $"comment_time = '{upDate}' " +
                    $"WHERE comment_id='{id}'";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }
        public CommentModel GetCommentByID(int id)
        {
            string sql = $"SELECT * FROM comment_s " +
                $"WHERE comment_id = {id}";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    CommentModel c = new CommentModel
                    {
                        UserID = (int)reader["user_id"],
                        ID = (int)reader["comment_id"],
                        Message = (string)reader["comment_message"],
                        SubmissionTime = (DateTime)reader["comment_time"],
                        EditedNumber = (int)reader["edited_number"]
                    };
                    if (reader["question_id"] is DBNull)
                    {
                        c.Question_ID = null;
                    }
                    else
                    {
                        c.Question_ID = (int?)reader["question_id"];
                    }
                    if (reader["answer_id"] is DBNull)
                    {
                        c.Answer_ID = null;
                    }
                    else
                    {
                        c.Answer_ID = (int?)reader["answer_id"];
                    }
                    return c;


                };
            }; throw new Exception();
        }
        public void RemoveComment(int id)
        {
            string sql = $"DELETE FROM comment_s " +
                   $"WHERE comment_id='{id}'";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }
        public void IncreaseNumberOfEdits(int id)
        {
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand($"UPDATE comment_s SET edited_number = edited_number + 1 WHERE comment_id={id} ", conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        public void AddTag(int questionID, string url)
        {
            int tagId;

            try 
            { 
                using (NpgsqlConnection cn = new NpgsqlConnection(cs))
                {
                    cn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT tag_id FROM tags WHERE tag_name='{url}'", cn))
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        tagId = (int)reader["tag_id"];
                    };
                };
            }
            catch
            { 
                using (NpgsqlConnection cn = new NpgsqlConnection(cs))
                {
                    cn.Open();

                    using (NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO tags(tag_name) VALUES('{url}')", cn))
                    {
                        cmd.ExecuteNonQuery();
                    };
                }

                using (NpgsqlConnection cn = new NpgsqlConnection(cs))
                {
                    cn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT tag_id FROM tags WHERE tag_name='{url}'", cn))
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        tagId = (int)reader["tag_id"];
                    };
                };
            }

            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"INSERT INTO question_tags(question_id,tag_id) VALUES({questionID},{tagId})", cn))
                {
                    cmd.ExecuteNonQuery();
                }
            };
        }

        public List<TagModel> GetTagUrl(int questionId)
        {
            List<TagModel> listOfTags = new List<TagModel>();

            try
            {
                using (NpgsqlConnection cn = new NpgsqlConnection(cs))
                {
                    cn.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT filter1.tag_name, filter1.tag_id FROM questions " +
                        $"INNER JOIN (SELECT tags.tag_id, question_tags.question_id, tags.tag_name FROM tags " +
                        $"INNER JOIN question_tags ON tags.tag_id = question_tags.tag_id) as filter1 " +
                        $"ON filter1.question_id = questions.question_id WHERE questions.question_id = '{questionId}'", cn))
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TagModel tag = new TagModel((int)reader["tag_id"], (string)reader["tag_name"]);
                            listOfTags.Add(tag);
                        }
                    };
                };
            }
            catch (Exception) { }
            return listOfTags;
        }

        public void DeleteTag(string url, int questionID)
        {
            string searchedTagID;
            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                //NpgsqlCommand cmd = new NpgsqlCommand($"DELETE FROM tags WHERE tag_id='{tagId}'",cn)
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT tags.tag_id FROM question_tags INNER JOIN tags " +
                    $"ON tags.tag_id = question_tags.tag_id" +
                    $" WHERE question_tags.question_id = '{questionID}' and tags.tag_name = '{url}'", cn))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();

                    searchedTagID = reader["tag_id"].ToString();
                };
            };

            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"DELETE FROM question_tags WHERE tag_id = '{searchedTagID}' AND question_id = '{questionID}'", cn))
                {
                    cmd.ExecuteNonQuery();
                };

            }
            IncreaseViewsCorrection(questionID);
        }

        public bool TagAlreadyOrdered(int questionID, string url)
        {
            bool alreadyInIt = false;

            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT question_tags.question_id, tags.tag_name FROM tags " +
                    $"INNER JOIN question_tags ON question_tags.tag_id=tags.tag_id " +
                    $"WHERE question_tags.question_id='{questionID}'", cn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string tag = (string)reader["tag_name"];
                        if (tag.Equals(url))
                        {
                            alreadyInIt = true;
                            break;
                        }
                    }
                };
            };

            return alreadyInIt;
        }

        public Dictionary<string, int> GetTagsWithCount()
        {
            Dictionary<string, int> TagsWithCounts = new Dictionary<string, int>();
            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT tags.tag_name, COUNT(tags.tag_id) as amount FROM tags" +
                    $" INNER JOIN question_tags ON question_tags.tag_id = tags.tag_id GROUP BY tags.tag_id", cn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string key = reader["tag_name"].ToString();
                        TagsWithCounts[key] = Convert.ToInt32(reader["amount"]);
                    }
                };
            };
            return TagsWithCounts;
        }


        public void AcceptAnswer(int answerID, int questionID)
        {
            string sql = $"UPDATE questions " +
                    $"SET accept_answer_id = '{answerID}' " +
                    $"WHERE question_id='{questionID}'";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
            int userID = GetUserIDByAnswerID(answerID);
            IncreaseViewsCorrection(questionID);
            ModifyReputation(userID, 15);
        }

        private void ModifyReputation(int userID, int point)
        {
            string sql = $"UPDATE users " +
                    $"SET user_reputation = user_reputation + '{point}' " +
                    $"WHERE user_id='{userID}'";

            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                };
            };
        }

        private int GetUserIDByAnswerID(int id)
        {
            int result = 0;
            var sql = $"SELECT user_id FROM answers WHERE answer_id = {id}";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = (int)reader["user_id"];
                    }
                };
            };
            return result;
        }
        private int GetUserIDByQuestionID(int id)
        {
            int result = 0;
            var sql = $"SELECT user_id FROM questions WHERE question_id = {id}";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result = (int)reader["user_id"];
                    }
                };
            };
            return result;
        }

        public int GetUserIdForUsername(string username)
        {
            var sql = $"SELECT user_id  FROM users WHERE user_name = '{username}'";
            using (var conn = new NpgsqlConnection(cs))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    var result = (int)reader["user_id"];

                    return (int)result;
                };
            };
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> listOfUsers = new List<UserModel>();
            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM users", cn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["user_id"]);
                        string name = reader["user_name"].ToString();
                        int reputation = Convert.ToInt32(reader["user_reputation"]);
                        DateTime time = Convert.ToDateTime(reader["user_registration_date"]);
                        listOfUsers.Add(new UserModel(id, name, reputation, time));
                    }
                };
            };
            return listOfUsers;
        }

        public int GetUserId(string username)
        {
            int userID;
            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM users WHERE user_name LIKE '{username}'", cn))
                {
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    userID = Convert.ToInt32(reader["user_id"]);
                };
            };
            return userID;
        }


        public List<QuestionModel> AllQuestionForUser(int id)
        {
            List<QuestionModel> listOfQuestions = new List<QuestionModel>();
            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM questions WHERE user_id = '{id}'", cn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int questionId = Convert.ToInt32(reader["question_id"]);
                        DateTime time = Convert.ToDateTime(reader["question_time"]);
                        int viewNumber = Convert.ToInt32(reader["question_viewnumber"]);
                        int userId = Convert.ToInt32(reader["user_id"]);
                        int voteNumber = Convert.ToInt32(reader["question_votenumber"]);
                        string title = reader["question_title"].ToString();
                        string message = reader["question_message"].ToString();
                        string image = reader["question_imageurl"].ToString();
                        try
                        {
                            int acceptAnswerID = Convert.ToInt32(reader["accept_answer_id"]);
                            listOfQuestions.Add(new QuestionModel(questionId, time, viewNumber, userId,
                            voteNumber, title, message, image, acceptAnswerID));
                        }
                        catch
                        {
                            listOfQuestions.Add(new QuestionModel(questionId, time, viewNumber, userId,
                            voteNumber, title, message, image));
                        }
                    }
                };
            };
            return listOfQuestions;
        }




        public List<AnswerModel> AllAnswerForUser(int id)
        {
            List<AnswerModel> listOfAnswers = new List<AnswerModel>();

            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM answers WHERE user_id = '{id}'", cn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int answerId = Convert.ToInt32(reader["answer_id"]);
                        DateTime timeOfAnswer = Convert.ToDateTime(reader["answer_time"]);
                        int userID = Convert.ToInt32(reader["user_id"]);
                        int voteNumber = Convert.ToInt32(reader["answer_votenumber"]);
                        int questionID = Convert.ToInt32(reader["question_id"]);
                        string message = reader["answer_message"].ToString();
                        string image = reader["answer_image"].ToString();

                        listOfAnswers.Add(new AnswerModel(answerId, timeOfAnswer, userID, voteNumber,
                            questionID, message, image));
                    }
                };
            };
            return listOfAnswers;
        }


        public List<CommentModel> AllCommentForUser(int id)
        {
            List<CommentModel> listOfComments = new List<CommentModel>();

            using (NpgsqlConnection cn = new NpgsqlConnection(cs))
            {
                cn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM comment_s WHERE user_id = '{id}'", cn))
                {
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int commentId = Convert.ToInt32(reader["comment_id"]);
                        int answerId;
                        try
                        {
                            answerId = Convert.ToInt32(reader["answer_id"]);
                        }
                        catch
                        {
                            answerId = 0;
                        }
                        int userId = Convert.ToInt32(reader["user_id"]);
                        int questionId = Convert.ToInt32(reader["question_id"]);
                        string message = reader["comment_message"].ToString();
                        DateTime submissionTime = Convert.ToDateTime(reader["comment_time"]);
                        int editNumber = Convert.ToInt32(reader["edited_number"]);

                        listOfComments.Add(new CommentModel(commentId, answerId, userId, questionId,
                            message, submissionTime, editNumber));
                    }
                };
            };
            return listOfComments;
        }


        public Dictionary<QuestionModel, List<AnswerModel>> AnswersWithQuestions(int id)
        {
            Dictionary<QuestionModel, List<AnswerModel>> result = new Dictionary<QuestionModel, List<AnswerModel>>();

            List<QuestionModel> allQuestion = GetQuestions();
            List<AnswerModel> answersForUser = AllAnswerForUser(id);

            foreach (var question in allQuestion)
            {
                List<AnswerModel> answersForQuestion = new List<AnswerModel>();
                foreach (var answer in answersForUser)
                {
                    if (answer.QuestionID == question.ID)
                    {
                        answersForQuestion.Add(answer);
                    }
                }
                if (answersForQuestion.Count > 0)
                    result[question] = answersForQuestion;
            }

            return result;
        }


        public Dictionary<QuestionModel, List<CommentModel>> CommentsWithQuestions(int id)
        {
            Dictionary<QuestionModel, List<CommentModel>> result = new Dictionary<QuestionModel, List<CommentModel>>();

            List<QuestionModel> allQuestion = GetQuestions();
            List<CommentModel> commentsForUser = AllCommentForUser(id);

            foreach (var question in allQuestion)
            {
                List<CommentModel> commentsForQuestion = new List<CommentModel>();
                foreach (var comment in commentsForUser)
                {
                    if (comment.Question_ID == question.ID)
                    {
                        commentsForQuestion.Add(comment);
                    }
                }
                if (commentsForQuestion.Count > 0)
                    result[question] = commentsForQuestion;
            }

            return result;
        }
    }
}
