using Gamification.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Gamification.Data.Interfaces;


namespace Gamification.Utilities.Timers
{
    public class TeamTimer : Timer
    {
        public string teamId;
        public TeamTimer(double interval)
        : base(interval)
        {
        }

    }
}
