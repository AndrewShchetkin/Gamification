using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace Gamification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : Controller
    {
        private readonly IRoundRepository _roundRepository;
        public RoundController(IRoundRepository roundRepository)
        {
            _roundRepository = roundRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllRounds()
        {
            var allRounds = await _roundRepository.GetAllRounds();
            return Ok(allRounds);
        }
        [HttpGet(template: "getRoundById")]
        public async Task<ActionResult> GetRoundById([FromQuery] Guid roundId)
        {
            var round = await _roundRepository.GetRoundById(roundId);
            return Ok(round);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> NewRound(Round newRound)
        {
            await _roundRepository.Create(newRound);
            return Ok(newRound.RoundId);
        }
    }
}
