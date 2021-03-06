﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class AnswerModel
    {
        public int ID { get; set; }  //id: A unique identifier for the answer
        public DateTime TimeOfAnswer { get; set; } = DateTime.Now; //submission_time: The UNIX timestamp when the answer was posted
        public int UserID { get; set; }

        public int VoteNumber { get; set; } = 0; //vote_number: The sum of votes this answer has received
        public int QuestionID { get; set; }     //question_id: The id of the question this answer belongs to.
        [Required(ErrorMessage = "Answer message can't be empty.")]
        public string Message { get; set; } = null; //message: The answer text
        public string Image { get; set; }   //image: the path to the image for this answer


        public AnswerModel() { }
        public AnswerModel(int id,DateTime timeOfAnswer,int userID, int voteNumber,int questionID,string message,string image)
        {
            ID = id;
            TimeOfAnswer = timeOfAnswer;
            UserID = userID;
            VoteNumber = voteNumber;
            QuestionID = questionID;
            Message = message;
            Image = image;
        }

        public override string ToString()
        {
            return ID + "," + TimeOfAnswer + "," + VoteNumber + "," + QuestionID + "," + "\"" + Message + "\"" + "," + Image;
        }
    }
}
