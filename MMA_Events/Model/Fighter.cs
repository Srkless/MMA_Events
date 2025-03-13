using MMA_Fights.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Fights.Model
{
    public class Fighter : Person
    {
        public int IdFighter {  get; set; }
        public string? Nickname { get; set; }

        public int IdCategory { get; set; }

        public string? BirthDate { get; set; }

        public bool IsReady { get; set; }

        public int IdOrganization { get; set; }

        public float FightWeight { get; set; }

        public string? Image { get; set; }

    }
}
