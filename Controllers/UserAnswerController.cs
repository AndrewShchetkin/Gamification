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
    public class UserAnswerController : ControllerBase
    {
        private readonly IUserAnswerRepository _userAnswerRepository;
        public UserAnswerController(IUserAnswerRepository userAnswerRepository)
        {
            _userAnswerRepository = userAnswerRepository;
        }
        [HttpGet(template: "getAllAnswersByUser")]
        public async Task<ActionResult> GetAllAnswersByUser(User user)
        {
            var userAnswers = await _userAnswerRepository.GetAllUserAnswersByUser(user);
            return Ok(userAnswers);
        }
        [HttpGet(template: "getUserAnswerById")]
        public async Task<ActionResult> GetAnswerById([FromQuery] Guid userId)
        {
            var userAnswer = await _userAnswerRepository.GetUserAnswerById(userId);
            return Ok(userAnswer);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> NewUserAnswer(UserAnswer userAnswer)
        {
            await _userAnswerRepository.Create(userAnswer);
            return Ok(userAnswer.UserAnswerId);
        }
    }
}
