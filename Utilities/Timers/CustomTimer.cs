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
    public class CustomTimer : Timer
    {
        public HubCallerContext callerContext { get; set; }
        public string team;
        public List<Question> questions;
        public List<List<Answer>> answers;
        public int currentQuiestion = 0;
        public static ConcurrentDictionary<string, CustomTimer> Timers = new ConcurrentDictionary<string, CustomTimer>();
        public IClientProxy hubCallerClients { get; set; }
        public CustomTimer(double interval)
        : base(interval)
        {
        }

    }
}
