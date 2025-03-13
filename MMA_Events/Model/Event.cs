using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Fights.Model
{
    public class Event
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Date { get; set; }

        public int IdOrganization { get; set; }
    }
}
