using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamification.Models;
using Microsoft.AspNetCore.SignalR;

namespace Gamification.Hubs
{
    public class FirstHub : Hub
    {
        public async Task SendMessage(ChatMessage message)
        {
            
            //await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            
            await base.OnConnectedAsync();
        }
    }
}
