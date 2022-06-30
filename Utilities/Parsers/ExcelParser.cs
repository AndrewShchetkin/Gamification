using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Gamification.Data;
using Gamification.Models;
using Gamification.Data.Interfaces;

namespace Gamification.Utilities.Parsers
{
    public class ExcelParser
    {
        private readonly IFormFile _excel;
        private readonly Quiz _quiz;
        public string Error { get; set; }
        public ExcelParser(IFormFile excel, Quiz quiz)
        {
            _excel = excel;
            _quiz = quiz;
            Error = "";
        }

        public List<Question> Parse()
        {
            List<Question> questionList = new List<Question>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                try
                {
                    _excel.CopyTo(stream);
                }
                catch(NullReferenceException ex)
                {
                    Error = "Файл не передан";
                    return null;
                }
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int row = 0;
                    while (reader.Read())
                    {
                        //if (reader.GetValue(0) == null)
                        //{
                        //    break;
                        //}
                        //Question question = new Question
                        //{
                        //    QuestionNumber = Int32.Parse(reader.GetValue(0).ToString()),
                        //    QuestionText = reader.GetValue(1).ToString(),
                        //    Quiz = _quiz
                        //};


                        if (reader.FieldCount != 6) // max columns in whole table
                        {
                            Error = "В строке должно быть 6 колонок!";
                            return null;
                        }

                        Question question = new Question()
                        {
                            Quiz = _quiz
                        };
                        List<Answer> answers = new List<Answer>();

                        bool isCorrectAnswer = true;
                        for (int i = 0; i <= 5; ++i)
                        {
                            if (reader.GetValue(i) == null)
                            {
                                Error = $"({i+1},{row+1}) null column";
                                return null;
                            }

                            try
                            {
                                if (i == 0)
                                    question.QuestionNumber = Int32.Parse(reader.GetValue(i).ToString());
                            }
                            catch(FormatException ex)
                            {
                                Error = $"(1,{row+1}) column is not a number";
                                return null;
                            }

                            if (i == 1)
                                question.QuestionText = reader.GetValue(i).ToString();
                            if (i > 1)
                            {
                                answers.Add(new Answer { 
                                    AnswerText = reader.GetValue(i).ToString(),
                                    RightAnswer = isCorrectAnswer,
                                    Question = question
                                });
                                isCorrectAnswer = false;
                            }
                        }
                        //Answer rightAnswer = new Answer { AnswerText = reader.GetValue(2).ToString(), RightAnswer = true, Question = question };
                        //answers.Add(rightAnswer);

                        //for (int i = 3; i <= 5; ++i)
                        //    answers.Add(new Answer { AnswerText = reader.GetValue(i).ToString(), RightAnswer = false, Question = question });

                        question.Answers = answers;
                        questionList.Add(question);

                        ++row;
                    }
                }
            }

            return questionList;
        }
    }
}
