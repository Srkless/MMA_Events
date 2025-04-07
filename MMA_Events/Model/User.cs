using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Model
{
    public class User : Person
    {
        public int IdUser { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Style { get; set; }
        public string? Language { get; set; }

    }
}
