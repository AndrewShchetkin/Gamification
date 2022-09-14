using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class TeamAnswer
    {
        [Key]
        public Guid TeamAnswerId { get; set; }
        public Answer Answer { get; set; }
        public Team Team { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}
