#nullable disable 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzerior.Models
{
    public class Pizzeria
    {

        [Key]
        [Required]
        
        public string PizzeriaID { get; set; }
        [Required]
        [MinLength(5,ErrorMessage="Minst 5 tecken!!")]
        [MaxLength(40)]
        public string Namn { get; set; }
        public string Adress { get; set; }
        public string PostNr { get; set; }
        public string PostOrt { get; set; }
        public string IntroBild { get; set; }
        public DateTime Skapad { get; set; }
        public DateTime Modifierad { get; set; }
        List<Omdome> Omdomen { get; set; }

    }


}
