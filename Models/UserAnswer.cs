using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class UserAnswer
    {
        [Key]
        public Guid UserAnswerId { get; set; }
        public Answer Answer { get; set; }
        public User User { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}
