using System;

namespace Gamification.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        public Guid? RoleId { get; set; }
        public Role Role { get; set; }

        public Guid? TeamId { get; set; }
        public Team Team { get; set; }
        public int Points { get; set; }
    }
}
