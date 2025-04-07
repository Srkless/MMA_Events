using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Model
{
    public class Organizator
    {
        public int IdOrganizator { get; set; }

        public string? Name { get; set; }

        public string? Password { get; set; }

        public string? Image { get; set; }

        public string? Style { get; set; }

        public string? Language { get; set; }
    }
}
