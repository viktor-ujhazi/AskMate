using AskMateMVC.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AskMateMVC.Services
{
    public class CsvLoader
    {

        static string filename = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Questions.csv");
        public CsvLoader()
        {
           
            LoadQuestion();

        }
        
        
        public static void LoadQuestion()
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            foreach (var row in lines)
            {
                String[] Fields = CSVParser.Split(row);
                // clean up the fields (remove " and leading spaces)
                for (int i = 0; i < Fields.Length; i++)
                {
                    Fields[i] = Fields[i].TrimStart(' ', '"');
                    Fields[i] = Fields[i].TrimEnd('"');

                }
                QuestionModel model = new QuestionModel
                {
                    ID = Guid.Parse(Fields[0]),
                    TimeOfQuestion = DateTime.Parse(Fields[1]),
                    ViewNumber = int.Parse(Fields[2]),
                    VoteNumber = int.Parse(Fields[3]),
                    Title = Fields[4],
                    Message = Fields[5],
                    Image = Fields[6]
                };
                CsvDatabase.listOfQuestions.Add(model);
            }
            foreach (var item in CsvDatabase.listOfQuestions)
            {
                Console.WriteLine($"ID: {item.ID}");
                Console.WriteLine($"TimeOfQuestion: {item.TimeOfQuestion}");
                Console.WriteLine($"ViewNumber: {item.ViewNumber}");
                Console.WriteLine($"VoteNumber: {item.VoteNumber}");
                Console.WriteLine($"Title: {item.Title}");
                Console.WriteLine($"Message: {item.Message}");
                Console.WriteLine($"Image: {item.Image}");
                Console.WriteLine();

            }

            

        }
    }
}
