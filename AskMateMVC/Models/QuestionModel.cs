using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class QuestionModel
    {

        public int ID { get; set; }     //id: A unique identifier for the question
        [Display(Name = "Posted: ")]
        public DateTime TimeOfQuestion { get; set; } = DateTime.Now; //submission_time: The UNIX timestamp when the question was posted
        public int ViewNumber { get; set; } = 0; //view_number: How many times this question was displayed in the single question view
        public int UserID { get; set; }
        public int VoteNumber { get; set; } = 0;//vote_number: The sum of votes this question has received
        [Display(Name = "Title of Question")]
        [Required(ErrorMessage = "You need to give a title.")]
        public string Title { get; set; }   //title: The title of the question
        [Display(Name = "Question")]
        [Required(ErrorMessage = "You need to ask something.")]
        public string Message { get; set; } //message: The question text
        [Display(Name = "Image for Question")]
        public string Image { get; set; }   //image: The path to the image for this question
        public int AcceptAnswerID { get; set; } = 0;

        public QuestionModel() { }

        public QuestionModel(int id, DateTime time, int viewNumber,
            int userID, int voteNumber, string title, string message, string image, int acceptAnswerId)
        {
            ID = id;
            TimeOfQuestion = time;
            ViewNumber = viewNumber;
            UserID = userID;
            VoteNumber = voteNumber;
            Title = title;
            Message = message;
            Image = image;
            AcceptAnswerID = acceptAnswerId;
        }

        public QuestionModel(int id, DateTime time, int viewNumber,
            int userID, int voteNumber, string title, string message, string image)
        {
            ID = id;
            TimeOfQuestion = time;
            ViewNumber = viewNumber;
            UserID = userID;
            VoteNumber = voteNumber;
            Title = title;
            Message = message;
            Image = image;
        }


        public override string ToString()
        {
            return ID + "," + TimeOfQuestion + "," + ViewNumber + "," + VoteNumber + "," + "\"" + Title + "\"" + "," + "\"" + Message + "\"" + "," + Image;
        }
        public void IncreaseViews()
        {
            ViewNumber++;
        }
    }
}
