#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pizzerior.Models;
using Pizzerior.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Pizzerior.ViewModels
{
    public partial class PizzeriaViewModel : ObservableObject 
    {
        private readonly IPizzeriaService _pizzeriaService;
        

        [ObservableProperty]
        public Pizzeria _selectedPizzeria;        






        public PizzeriaViewModel()
        {
            _pizzeriaService = new PizzeriaService();

        }

        [RelayCommand]
        public void DoSave()
        {
                        
            Pizzeria upPizzeria = _pizzeriaService.Update(_selectedPizzeria);
            if(upPizzeria!=null)
            {
                var w = Application.Current.Windows[1];
                w.Close();
            }
            else
            {
                MessageBox.Show("Ett fel uppstod när pizzerian skulle sparas", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

    }
}
