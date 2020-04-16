using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AskMateMVC.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Reputation { get; set; }
        public DateTime TimeOfRegistration { get; set; } = DateTime.Now;

        public UserModel() { }
        public UserModel(int id, string name, int reputation, DateTime time)
        {
            TimeOfRegistration = time;
            ID = id;
            Name = name;
            Password = "";
            Reputation = reputation;
        }
    }
}
