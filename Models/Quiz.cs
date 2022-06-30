using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    [Serializable]
    public class Quiz
    {
        [Key]
        public Guid QuizId { get; set; }
        public string QuizName { get; set; }
        public DateTime QuizStartTime { get; set; }
        public DateTime QuizFinishTime { get; set; }

        public List<Question> Questions { get; set; }
    }
}
