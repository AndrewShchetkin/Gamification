using Gamification.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Data
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationContext db;
        public TeamRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(Team newTeam) 
        {
            db.Teams.Add(newTeam);
            await db.SaveChangesAsync();
            return newTeam.Id;
        }

        public async Task<List<Team>> GetAllTeams()
        {
            var teams = await db.Teams.Include(t => t.Users).ToListAsync();

            return teams;
        }

        public async Task<Team> GetTeamByName(string teamName)
        {
            return await db.Teams.FirstOrDefaultAsync(t => t.TeamName == teamName);
        }

        public async Task<Team> GetTeamById(Guid teamId, bool isRetrieveUsers = false)
        {
            if (isRetrieveUsers)
            {
                return await db.Teams.Include(t => t.Users).FirstOrDefaultAsync(t => t.Id == teamId);
            }
            return  await db.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task JoinToTheExistTeam(Team team, User user)
        {
            user.Team = team;
            await db.SaveChangesAsync();
        }
    }
}
