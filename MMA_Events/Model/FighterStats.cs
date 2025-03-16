using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Model
{
    public class FighterStats
    {
        public int IdFighter { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int KOs { get; set; }
        public int Submissions { get; set; }
    }
}
