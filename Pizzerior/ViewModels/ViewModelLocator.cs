#nullable disable 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pizzerior.ViewModels
{
    public class ViewModelLocator
    {
        // Singleton instance of ViewModelLocator
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator());

        // MainViewModel
        public MainViewModel MainViewModel => new MainViewModel();

        // PizzeriaViewModel
        public PizzeriaViewModel PizzeriaViewModel => new PizzeriaViewModel();

        // PizzeriaViewModel
        public AddPizzeriaViewModel AddPizzeriaViewModel => new AddPizzeriaViewModel();

    }
}



