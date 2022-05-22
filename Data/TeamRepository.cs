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

        public async Task<Team> GetTeamById(Guid teamId)
        {
            return  await db.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await db.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<Team> JoinToTheExistTeam(Guid teamId, User user)
        {
            var team = await GetTeamById(teamId);
            user.Team = team;
            await db.SaveChangesAsync();
            return team;
        }
    }
}
