using AutoMapper;
using Gamification.Data;
using Gamification.Models;
using Gamification.Models.DTO.Team;
using Gamification.Models.Errors;
using Microsoft.AspNetCore.Authorization;
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
            var team = await _teamRepository.GetTeamByName(teamDto.TeamName);
            if (team != null)
            {
                var errorResponse = new ErrorResponse();
                var error = new ErrorModel
                {
                    FieldName = "TeamName",
                    Message = "Команда с таким названием уже существует уже существует"
                };
                errorResponse.Errors.Add(error);
                return BadRequest(errorResponse);
            }

            var user = _userRepository.GetUserByUserName(User.Identity.Name);
            var newTeam = new Team
            {
                TeamName = teamDto.TeamName,
                Password = BCrypt.Net.BCrypt.HashPassword(teamDto.Password),
                Users = new List<User> { user }
            };
            var teamID = await _teamRepository.Create(newTeam);

            return Ok(teamID);
        }

        [HttpPost(template: "jointheteam")]
        public async Task<ActionResult<Guid>> JoinTheTeam(JoinTheTeamDto joinTheTeamDto)
        {
            var team = await _teamRepository.GetTeamById(joinTheTeamDto.TeamId, true);
            if (team == null)
            {
                return BadRequest("Такой команды не существует");
            }
            if (team.Users.Count > 4)
            {
                return BadRequest("Нет места в команде");
            }
            if(!BCrypt.Net.BCrypt.Verify(joinTheTeamDto.Password, team.Password))
            {
                return BadRequest("Не верный пароль");
            }
            var currentUser = _userRepository.GetUserByUserName(User.Identity.Name);
            await _teamRepository.JoinToTheExistTeam(team, currentUser);
            
            return Ok(currentUser.TeamId);
        }

        [Authorize]
        [HttpGet(template: "getallteams")]
        public async Task<ActionResult<TeamDto>> GetAllTeams()
        {
            var teams = await _teamRepository.GetAllTeams();
            var teamsDto = _mapper.Map<List<Team>, List<TeamDto>>(teams);
            return Ok(teamsDto);
        }

        [HttpGet(template: "getTeamByID")]
        public async Task<ActionResult<TeamDto>> GetTeamById([FromQuery] Guid teamID)
        {
            var team = await _teamRepository.GetTeamById(teamID, true);
            var teamsDto = _mapper.Map<Team, TeamDto>(team);
            return Ok(teamsDto);
        }
    }
}
