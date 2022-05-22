using Gamification.Hubs.Clients;
using Gamification.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Hubs
{
    public class MapHub : Hub<IMapClient>
    {
        public async Task UpdateCell(Cell cell)
        {
            await Clients.All.UpdateCell(cell);
        }
    }
}
