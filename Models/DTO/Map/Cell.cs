using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gamification.Models.DTO.Map
{
    public class Cell
    {
        public int id { get; set; }
        public int color { get; set; }
        public int elevation { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    }
}
