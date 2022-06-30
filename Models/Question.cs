using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }

        // added
        public List<Answer> Answers { get; set; }
        public Guid QuizId { get; set; } // for cascade delete
        //
        public Quiz Quiz { get; set; }
    }
}
