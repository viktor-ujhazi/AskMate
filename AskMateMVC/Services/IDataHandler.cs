﻿using AskMateMVC.Models;
using System;
using System.Collections.Generic;

namespace AskMateMVC.Services
{
    public interface IDataHandler
    {
        public List<QuestionModel> GetQuestions();
        public List<AnswerModel> GetAnswers();
        public List<CommentModel> GetComments();
        public void AddQuestion(QuestionModel model);
        public void AddAnswer(AnswerModel model, int id);
        public void AddComment(CommentModel model);
        public QuestionModel GetQuestionByID(int id);
        public AnswerModel GetAnswerByID(int id);
        public CommentModel GetCommentByID(int id);
        public List<AnswerModel> GetAnswersForQuestion(int id);
        public List<CommentModel> GetCommentsToAnswers(int id);
        public List<CommentModel> GetCommentsToQuestion(int id);
        public void RemoveQuestionById(int id);
        public void EditQuestion(int id, QuestionModel question);
        public void EditAnswer(int id, AnswerModel answer);
        public void EditComment(int id, CommentModel comment);
        public void RemoveAnswer(int id);
        public void RemoveComment(int id);
        public void ModifyQuestionVote(int id, int voteValue);
        public void ModifyAnswerVote(int id, int voteValue);
        public void IncreaseViews(int id);
        public List<QuestionModel> MostViewedQuestions();
        public List<QuestionModel> SortedDatas(string attribute);
        public List<QuestionModel> LatestQuestions();
        public List<QuestionModel> SearchInData(string searchedWord);
    }
}