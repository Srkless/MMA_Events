using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMA_Fights.Model
{
    public class Category
    {
        public int IdCategory { get; set; }

        public string Name { get; set; } = string.Empty;

        public float MinWeight { get; set; }
        public float MaxWeight { get; set; }
    }

}
