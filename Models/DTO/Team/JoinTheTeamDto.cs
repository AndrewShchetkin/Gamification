using System;

namespace Gamification.Models.DTO.Team
{
    public class JoinTheTeamDto
    {
        public Guid TeamId  { get; set; }
        public string Password { get; set; }
    }
}
