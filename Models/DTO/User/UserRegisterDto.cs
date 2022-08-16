using System;

namespace Gamification.Models.DTO
{
    [Serializable]
    public class UserRegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
