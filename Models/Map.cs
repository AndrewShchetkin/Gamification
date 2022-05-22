using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models
{
    public class Map
    {
        public Map(List<Cell> cells)
        {
            Cells = cells;
        }

        public List<Cell> Cells { get; set; }
    }
}
