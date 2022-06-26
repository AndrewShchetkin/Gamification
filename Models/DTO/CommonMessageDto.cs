using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models.DTO
{
    public class CommonMessageDto
    {
        public string Author { get; set; }
        public string Text { get; set; }

        public DateTime? DispatchTime { get; set; }
    }
}
