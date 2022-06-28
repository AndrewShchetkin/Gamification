using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.Models;

namespace Gamification.Data.Interfaces
{
    public interface ITeamAnswerRepository
    {
        Task<Guid> Create(TeamAnswer newTeamAnswer);
        Task<TeamAnswer> GetTeamAnswerById(Guid teamAnswerId);
        Task<List<TeamAnswer>> GetAllTeamAnswersByTeam(Team team);
    }
}
