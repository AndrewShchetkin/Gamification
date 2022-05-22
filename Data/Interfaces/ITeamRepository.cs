using Gamification.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamification.Data
{
    public interface ITeamRepository
    {
        Task<Guid> Create(Team newTeam);
        Task<Team> GetTeamById(Guid teamId);
        Task<User> GetUserById(Guid userId);
        Task<List<Team>> GetAllTeams();
        Task<Team> JoinToTheExistTeam(Guid teamId, User user);
    }
}