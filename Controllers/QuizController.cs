﻿using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamification.Data;
using Gamification.Models;
using Gamification.Data.Interfaces;
using Gamification.Utilities.Parsers;

namespace Gamification.Controllers
{
    
    

    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {

        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;

        public QuizController(IQuizRepository quizRepository, IQuestionRepository questionRepository,
            IAnswerRepository answerRepository)
        {
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Import ([FromForm(Name = "file")] IFormFile excel, [FromForm(Name = "name")] string quizName,
            [FromForm(Name = "db")] DateTime dateBegin, [FromForm(Name = "de")] DateTime dateEnd)
        {
            Quiz quiz = new Quiz {QuizName = quizName, QuizStartTime= dateBegin, QuizFinishTime = dateEnd };
            ExcelParser excelParser = new ExcelParser(excel, quiz);
            Dictionary<Question, List<Answer>> questToAnswers = excelParser.Parse();

            await _quizRepository.Create(quiz);
            foreach (var question in questToAnswers.Keys)
                await _questionRepository.Create(question);

            foreach (var answers in questToAnswers.Values)
                foreach (var answer in answers)
                    await _answerRepository.Create(answer);

            return Ok(quiz);
        }
    }
}
