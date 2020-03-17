using AskMateMVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AskMateMVC.Services
{
    public class CsvHandler
    {
        IWebHostEnvironment WebHostEnvironment { get; }
        public CsvHandler(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }
        private string CsvFileName
        {
            get 
            {
                return Path.Combine(WebHostEnvironment.WebRootPath, "data", "Questions.csv");
            } 
        }
        public void SaveQuestions(QuestionModel model)
        {
            CsvDatabase.listOfQuestions.Add(model);
            var csv = new StringBuilder();
            //in your loop
            foreach (var item in CsvDatabase.listOfQuestions)
            {
                csv.AppendLine(item.ToString());
            }
            //after your loop
            File.WriteAllText(CsvFileName, csv.ToString());
            
        }
    }
}
