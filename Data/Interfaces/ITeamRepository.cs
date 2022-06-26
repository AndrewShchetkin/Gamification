using Gamification.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gamification.Data
{
    public interface ITeamRepository
    {
        Task<Guid> Create(Team newTeam);
//<<<<<<< HEAD
//        Task<Team> GetTeamById(Guid teamId);
//        Task<User> GetUserById(Guid userId);
//=======
        Task<Team> GetTeamById(Guid teamId, bool isRetrieveUsers = false);
        Task<Team> GetTeamByName(string teamName);
        Task<List<Team>> GetAllTeams();
        Task JoinToTheExistTeam(Team team, User user);
    }
}