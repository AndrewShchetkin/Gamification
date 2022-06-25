using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class Round
    {
        [Key]
        public Guid RoundId { get; set; }
        public Team Team { get; set; }
        public Quiz Quiz { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
