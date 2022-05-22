using Gamification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gamification.Data.Interfaces
{
    public interface IMapRepository
    {
        public Task<string> SaveMapAsync(Map newTeam, CancellationToken token);
        public Task<Map> LoadMapAsync(CancellationToken token);
        public Task<bool> UpdateCell(Cell cell, CancellationToken token);
    }
}
