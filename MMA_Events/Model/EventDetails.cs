using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Model
{
    public class EventDetails
    {
        public int idEvent { get; set; }
        public int idFightCard { get; set; }
        public int idOrganization { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public int idRedCorner { get; set; }
        public int idBlueCorner { get; set; }
        public FighterDetails RedCorner { get; set; }
        public FighterDetails BlueCorner { get; set; }
    }
}
