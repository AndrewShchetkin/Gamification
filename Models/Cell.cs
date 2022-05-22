using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public int Color { get; set; }
        public int Elevation { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Guid? Owner { get; set; }
    }
}
