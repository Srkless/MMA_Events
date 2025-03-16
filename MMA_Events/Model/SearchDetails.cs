using MMA_Events.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Events.Model
{
    public class SearchDetails
    {
        public FighterDetails Fighter { get; set; }
        public EventDetails Event { get; set; }
    }
}
