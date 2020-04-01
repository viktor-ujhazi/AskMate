using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class CommentModel
    {
        public int ID { get; set; }  //id: A unique identifier 
        public int? Answer_ID { get; set; } = null;

        public int? Question_ID { get; set; } = null;

        [Required(ErrorMessage = "Comment message can't be empty.")]
        public string Message { get; set; } = null;
        public DateTime SubmissionTime { get; set; } = DateTime.Now;
        public int EditedNumber { get; set; }

    }
}
