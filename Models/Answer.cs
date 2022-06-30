using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class Answer
    {
        [Key]
        public Guid AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool RightAnswer { get; set; }

        // added
        public Guid QuestionId { get; set; }
        //
        public Question Question { get; set; }
    }
}
