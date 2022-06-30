using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }
        [HttpGet(template: "getAllAnswersByQuestion")]
        public async Task<ActionResult> GetAllAnswersByQuestion(Question question)
        {
            var answers = await _answerRepository.GetAllAnswersByQuestion(question);
            return Ok(answers.Select((answer) => new
            {
                AnswerId = answer.AnswerId,
                AnswerText = answer.AnswerText,
                QuestionId = answer.Question
            }));
        }
        [HttpGet(template: "getAnswerById")]
        public async Task<ActionResult> GetAnswerById([FromQuery] Guid answerId)
        {
            var answer = await _answerRepository.GetAnswerById(answerId);
            return Ok(new {
                AnswerId = answer.AnswerId,
                AnswerText = answer.AnswerText,
                QuestionId = answer.Question
            });
        }
    }
}
