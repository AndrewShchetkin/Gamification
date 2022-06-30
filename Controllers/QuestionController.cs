using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Gamification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }
        [HttpGet(template: "getAllQuestionsByQuiz")]
        public async Task<ActionResult> GetAllQuestionsByQuiz(Quiz quiz)
        {
            var allQuestions = await _questionRepository.GetAllQuestionsByQuiz(quiz);
            return Ok(allQuestions);
        }
        [HttpGet(template: "getQuestionById")]
        public async Task<ActionResult> GetQuestionById([FromQuery] Guid questionId)
        {
            var question = await _questionRepository.GetQuestionById(questionId);
            return Ok(question);
        }

    }
}
