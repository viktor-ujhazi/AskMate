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

        public UserModel() { }
        public UserModel(int id,string name,int reputation)
        {
            ID = id;
            Name = name;
            Password = "";
            Reputation = reputation;
        }
    }
}
