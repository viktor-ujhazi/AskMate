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
    public class CsvLoader : IDataLoader
    {

        static string questionsFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Questions.csv");
        static string answersFileName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data", "Answers.csv");

        public CsvLoader()
        {
            LoadQuestion();
            
        }


        public void LoadQuestion()
        {
            string[] lines = System.IO.File.ReadAllLines(questionsFileName);
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
            
        }
        public void LoadAnswers()
        {
            string[] lines = System.IO.File.ReadAllLines(answersFileName);
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            
            try
            {
                foreach (var row in lines)
                {
                    String[] Fields = CSVParser.Split(row);
                    // clean up the fields (remove " and leading spaces)
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        Fields[i] = Fields[i].TrimStart(' ', '"');
                        Fields[i] = Fields[i].TrimEnd('"');

                    }
                    AnswerModel model = new AnswerModel
                    {
                        ID = Guid.Parse(Fields[0]),
                        TimeOfAnswer = DateTime.Parse(Fields[1]),
                        VoteNumber = int.Parse(Fields[2]),
                        QuestionID = Guid.Parse(Fields[3]),
                        Message = Fields[4],
                        Image = Fields[5]
                    };
                    CsvDatabase.listOfAnswers.Add(model);
                }
                foreach (var item in CsvDatabase.listOfAnswers)
                {
                    Console.WriteLine($"ID: {item.ID}");
                    Console.WriteLine($"TimeOfAnswer: {item.TimeOfAnswer}");
                    Console.WriteLine($"VoteNumber: {item.VoteNumber}");
                    Console.WriteLine($"QuestionID: {item.QuestionID}");
                    Console.WriteLine($"Message: {item.Message}");
                    Console.WriteLine($"Image: {item.Image}");
                    Console.WriteLine();
                }
            }
            catch (Exception)
            {
                Console.WriteLine();
            }
        }
    }
}
