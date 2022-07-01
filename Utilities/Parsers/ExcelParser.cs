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
using Gamification.Validators;

namespace Gamification.Utilities.Parsers
{
    public class ExcelParser
    {
        private readonly IFormFile _excel;
        private readonly Quiz _quiz;
        
        public ExcelParserValidator Validator { get; set; }
        public ExcelParser(IFormFile excel, Quiz quiz)
        {
            _excel = excel;
            _quiz = quiz;
            Validator = new ExcelParserValidator();
        }

        public List<Question> Parse()
        {
            List<Question> questionList = new List<Question>();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream())
            {
                _excel.CopyTo(stream);
                stream.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int row = 0;
                    while (reader.Read())
                    {

                        if (!Validator.ValidateColumns(reader.FieldCount))
                            return null;

                        Question question = new Question()
                        {
                            Quiz = _quiz
                        };
                        List<Answer> answers = new List<Answer>();

                        bool isCorrectAnswer = true;
                        for (int i = 0; i <= 5; ++i)
                        {

                            if (!Validator.ValidateColumnNullRef(reader.GetValue(i), i, row))
                                return null;


                            if (i == 0)
                            {
                                if (!Validator.ValidateColumnIsNum(reader.GetValue(i), i, row))
                                    return null;
                                question.QuestionNumber = Int32.Parse(reader.GetValue(i).ToString());
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

                        question.Answers = answers;
                        questionList.Add(question);

                        ++row;
                    }
                    if (!Validator.ValidateEmptyFile(row))
                        return null;
                }
            }

            return questionList;
        }
    }
}
