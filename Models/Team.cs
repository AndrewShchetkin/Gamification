using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gamification.Models
{
    public class Team
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public int Points { get; set; }

        public List<User> Users { get; set; }
    }
}
