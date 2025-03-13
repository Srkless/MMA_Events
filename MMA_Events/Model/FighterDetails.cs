using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Fights.Model
{
    public class FighterDetails
    {
        public int idFighter {  get; set; }
        public string? Nickname { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Country { get; set; }
        public string? CategoryName { get; set; }
        public string? BirthDate { get; set; }
        public float FightWeight { get; set; }
        public string? Image { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int KOs { get; set; }
        public int Submissions { get; set; }
        public string Score => Wins + "-" + Losses + "-" + Draws;
        public string Fullname => Name + " " + Surname;
        public DateTime? BirthDate_DateTime { get; set; }

    }
}
