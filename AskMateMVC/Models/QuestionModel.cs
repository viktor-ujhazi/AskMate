using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class QuestionModel
    {
        public int ID { get; set; }     //id: A unique identifier for the question
        public DateTime TimeOfAnswer { get; set; }  //submission_time: The UNIX timestamp when the question was posted
        public int ViewNumber { get; set; } //view_number: How many times this question was displayed in the single question view
        public int VoteNumber { get; set; } //vote_number: The sum of votes this question has received
        public string Title { get; set; }   //title: The title of the question
        public string Message { get; set; } //message: The question text
        public string Image { get; set; }   //image: The path to the image for this question
        
        
    }
}
