using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Model
{

    public enum FightMethod
    {
        KO,
        Submission,
        Decision
    }
    public class Fight
    {
        public int id { get; set; }
        public int? roundEnded { get; set; }
        public int redCornedId { get; set; }
        public int blueCornedId { get;set; }
        public int? winnerId { get; set; }
        public int? idEvent { get; set; }
        public int? idCard { get; set; }
        public FightMethod? Method { get; set; }

    }
}
