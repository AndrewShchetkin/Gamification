using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Data
{
    public class TeamAnswerRepository:ITeamAnswerRepository
    {
        private readonly ApplicationContext db;
        public TeamAnswerRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(TeamAnswer newTeamAnswer)
        {
            db.TeamAnswers.Add(newTeamAnswer);
            await db.SaveChangesAsync();
            return newTeamAnswer.TeamAnswerId;
        }
        public async Task<TeamAnswer> GetTeamAnswerById(Guid teamAnswerId)
        {
            return await db.TeamAnswers.FirstOrDefaultAsync(ta => ta.TeamAnswerId == teamAnswerId);
        }
        public async Task<List<TeamAnswer>> GetAllTeamAnswersByTeam(Team team)
        {
            var teamAnswers = await (from ta in db.TeamAnswers where ta.Team == team select ta).ToListAsync();
            return teamAnswers;
        }
    }
}
