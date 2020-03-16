using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class AnswerModel
    {
        public int ID { get; set; }     //id: A unique identifier for the answer
        public DateTime TimeOfAnswer { get; set; }  //submission_time: The UNIX timestamp when the answer was posted
        
        public int VoteNumber { get; set; } //vote_number: The sum of votes this answer has received
        public int  QuestionID { get; set; }    //question_id: The id of the question this answer belongs to.
        public string Message { get; set; } //message: The answer text
        public string Image { get; set; }   //image: the path to the image for this answer

    }
}
