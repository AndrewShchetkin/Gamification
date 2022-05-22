using System;
using System.Collections.Generic;

namespace Gamification.Models.DTO.Team
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public List<UsersInTeam> Users { get; set; }
        public int Points { get; set; }
    }
}
