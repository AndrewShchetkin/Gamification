using System;

namespace Gamification.Models.DTO.User
{
    [Serializable]
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
