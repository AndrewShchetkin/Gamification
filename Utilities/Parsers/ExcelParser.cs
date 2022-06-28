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
        private  readonly Quiz _quiz;
        public ExcelParser(IFormFile excel, Quiz quiz)
        {
            _excel = excel;
            _quiz = quiz;
        }

        public Dictionary<Question, List<Answer>> Parse()
        {
            Dictionary<Question, List<Answer>> questToAnswers = new Dictionary<Question, List<Answer>>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                _excel.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    while (reader.Read())
                    {
                        if (reader.GetValue(0) == null)
                        {
                            break;
                        }
                        Question question = new Question
                        {
                            QuestionNumber = Int32.Parse(reader.GetValue(0).ToString()),
                            QuestionText = reader.GetValue(1).ToString(),
                            Quiz = _quiz
                        };

                        List<Answer> answers = new List<Answer>();
                        Answer rightAnswer = new Answer { AnswerText = reader.GetValue(2).ToString(), RightAnswer = true, Question = question };
                        answers.Add(rightAnswer);

                        for (int i = 3; i <= 5; ++i)
                            answers.Add(new Answer { AnswerText = reader.GetValue(i).ToString(), RightAnswer = false, Question = question });

                        questToAnswers.Add(question, answers);
                        
                        
                    }
                }
            }

            return questToAnswers;
        }
    }
}
