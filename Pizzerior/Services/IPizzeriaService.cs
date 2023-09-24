#nullable disable 
using Pizzerior.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzerior.Services
{
    public interface IPizzeriaService
    {
        public List<Pizzeria> GetAll();
        public Pizzeria Get(string id);
        public bool Create(List<Pizzeria> pizzerior);
        public Pizzeria Update(Pizzeria pizzeria);
        public bool Delete(string id);
        ObservableCollection<Pizzeria> GetAllCollection();
    }
}
