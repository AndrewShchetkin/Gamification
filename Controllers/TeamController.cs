using AutoMapper;
using Gamification.Data;
using Gamification.Models;
using Gamification.Models.DTO.Team;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Controllers
{
    [Route(template: "api/team")]
    [ApiController]

    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TeamController(ITeamRepository teamRepository, IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        [HttpPost(template: "teamregister")]
        public async Task<ActionResult<Guid>> RegisterTeam(TeamRegisterDto teamDto)
        {
            var user = _userRepository.GetUserByUserName(User.Identity.Name);
            var newTeam = new Team
            {
                TeamName = teamDto.TeamName,
                Users = new List<User> { user }
            };
            var teamID = await _teamRepository.Create(newTeam);

            return Ok(teamID);
        }

        [HttpPost(template: "jointheteam")]
        public async Task<ActionResult<Guid>> JoinTheTeam(JoinTheTeamDto joinTheTeamDto)
        {
            var currentUser = _userRepository.GetUserByUserName(User.Identity.Name);
            var team = await _teamRepository.JoinToTheExistTeam(joinTheTeamDto.TeamId, currentUser);
            if(team == null)
            {
                return BadRequest("Такой команды не существует");
            }
            return Ok(currentUser.TeamId);
        }

        [HttpGet(template: "getallteams")]
        public async Task<ActionResult<List<TeamDto>>> GetAllTeams()
        {
            var teams = await _teamRepository.GetAllTeams();
            var teamsDto = _mapper.Map<List<Team>, List<TeamDto>>(teams);
            return Ok(teamsDto);
        }

        [HttpGet(template: "getteambyid")]
        public async Task<ActionResult<TeamDto>> GetTeamById(Guid teamId)
        {
            var teams = await _teamRepository.GetTeamById(teamId);
            var teamsDto = _mapper.Map<Team,TeamDto>(teams);
            return Ok(teamsDto);
        }

        [HttpGet(template: "getuserbyid")]
        public async Task<ActionResult<UsersInTeam>> GetUserById(Guid userId)
        {
            var user = await _teamRepository.GetUserById(userId);
            var userDto = _mapper.Map<User, UsersInTeam>(user);
            return Ok(userDto);
        }
    }
}
