using AskMateMVC.Models;
using System.Collections.Generic;

namespace AskMateMVC.Services
{
    public interface IDataLoader
    {
        void LoadAnswers();
        void LoadQuestion();
    }
}