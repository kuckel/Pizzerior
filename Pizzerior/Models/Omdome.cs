#nullable disable 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzerior.Models
{
    public class Omdome
    {

        [Key]
        public string OmdomeID { get; set; }
        public string PizzeriaID_Ref { get; set; }
        public string Namn { get; set; }
        public string Epost { get; set; }
        public int Betyg { get; set; } = 0;
        public DateTime Skapad { get; set; }
        public DateTime Modifierad { get; set; }

    }
}
