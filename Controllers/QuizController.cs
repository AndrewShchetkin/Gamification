using ExcelDataReader;
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
using System.Diagnostics;
using Newtonsoft.Json.Linq;

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
            Quiz quiz = new Quiz {

                QuizName = quizName,
                QuizStartTime= dateBegin,
                QuizFinishTime = dateEnd
            };
            
            ExcelParser excelParser = new ExcelParser(excel, quiz);
            List<Question> parsedQuestions = excelParser.Parse();
            if (excelParser.Error != "")
            {
                return BadRequest(new { error = excelParser.Error });
            }

            quiz.Questions = parsedQuestions;
            await _quizRepository.Create(quiz);

            return Ok(new { name=quiz.QuizName, dateBegin=quiz.QuizStartTime, dateEnd=quiz.QuizFinishTime});
        }

        [HttpGet]
        public async Task<ActionResult> GetAllQuizzes()
        {
            var allQuizzes = await _quizRepository.GetAllQuizzes();
            return Ok(allQuizzes.Select((quiz) => new { name = quiz.QuizName,
                dateBegin = quiz.QuizStartTime,
                dateEnd = quiz.QuizFinishTime }));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteQuiz(string quizName)
        {
            Quiz quizToDelete = await _quizRepository.GetQuizByName(quizName);
            await _quizRepository.Remove(quizToDelete);
            return Ok(new { msg = "ok!" });
        }
    }
}
