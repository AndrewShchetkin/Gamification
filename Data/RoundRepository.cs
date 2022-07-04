using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Data.Interfaces;
using Gamification.Models;
using Microsoft.EntityFrameworkCore;

namespace Gamification.Data
{
    public class RoundRepository:IRoundRepository
    {
        private readonly ApplicationContext db;
        public RoundRepository(ApplicationContext context)
        {
            db = context;
        }
        public async Task<Guid> Create(Round newRound)
        {
            db.Rounds.Add(newRound);
            await db.SaveChangesAsync();
            return newRound.RoundId;
        }
        public async Task<Round> GetRoundById(Guid roundId)
        {
            return await db.Rounds.FirstOrDefaultAsync(r => r.RoundId == roundId);
        }
        public async Task<List<Round>> GetAllRounds()
        {
            return await db.Rounds.ToListAsync();
        }
        public async Task<Round> GetCurrentRoundByTeamName(string teamName)
        {
            var round = await db.Rounds.SingleOrDefaultAsync((r)=> r.Team.TeamName == teamName && r.EndTime.Year == DateTime.Parse("0001-01-01").Year);
            return round;
        }
        public async Task UpdataEndDate(Round round, DateTime dateTime)
        {
            round.EndTime = dateTime;
            await db.SaveChangesAsync();
        }
    }
}
