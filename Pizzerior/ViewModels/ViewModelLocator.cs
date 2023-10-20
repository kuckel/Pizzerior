#nullable disable 
using Ninject;
using Pizzerior.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pizzerior.ViewModels
{
    public class ViewModelLocator
    {
        // Singleton instance of ViewModelLocator
        private static ViewModelLocator _instance;
        private  static IKernel _kernel;
      
       
        public static ViewModelLocator Instance => _instance ?? (_instance = new ViewModelLocator(_kernel));

        public MainViewModel MainViewModel =>  _kernel.Get<MainViewModel>();
        
        public PizzeriaViewModel PizzeriaViewModel => _kernel.Get<PizzeriaViewModel>();

        public AddPizzeriaViewModel AddPizzeriaViewModel => _kernel.Get<AddPizzeriaViewModel>();

        public ViewModelLocator(IKernel kernel)
        {
            _kernel = kernel;
        }

    }        

}







