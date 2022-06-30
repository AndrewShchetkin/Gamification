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
    public class TeamAnswerController : ControllerBase
    {
        private readonly ITeamAnswerRepository _teamAnswerRepository;
        public TeamAnswerController(ITeamAnswerRepository teamAnswerRepository)
        {
            _teamAnswerRepository = teamAnswerRepository;
        }
        [HttpGet(template: "getTeamAnswerById")]
        public async Task<ActionResult> GetTeamAnswerById([FromQuery] Guid teamAnswerId)
        {
            var teamAnswer = await _teamAnswerRepository.GetTeamAnswerById(teamAnswerId);
            return Ok(teamAnswer);
        }
        [HttpGet(template: "getAllTeamAnswersByTeam")]
        public async Task<ActionResult> GetAllTeamAnswerByTeam(Team team)
        {
            var teamAnswers = await _teamAnswerRepository.GetAllTeamAnswersByTeam(team);
            return Ok(teamAnswers);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> NewTeamAnswer(TeamAnswer teamAnswer)
        {
            await _teamAnswerRepository.Create(teamAnswer);
            return Ok(teamAnswer.TeamAnswerId);
        }
    }
}
