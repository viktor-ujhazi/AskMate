using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class TagModel
    {
        public int ID { get; set; }
        public string Url { get; set; }

        public TagModel() { }

        public TagModel(int id, string url)
        {
            ID = id;
            Url = url;
        }
    }
}
