using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamification.Models;

namespace Gamification.Data.Interfaces
{
    public interface IRoundRepository
    {
        Task<Guid> Create(Round newRound);
        Task<Round> GetRoundById(Guid roundId);
        Task<List<Round>> GetAllRounds();
        Task<Round> GetCurrentRoundByTeamName(string teamName);
        Task UpdataEndDate(Round round, DateTime dateTime);
    }
}
