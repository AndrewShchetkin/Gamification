using System;

namespace Gamification.Models.DTO.Team
{
    public class UsersInTeam
    {
        public string UserName { get; set; }
        public Guid Id { get; set; }
        public int Points { get; set; }
    }
}
