#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pizzerior.Models;
using Pizzerior.Services;
using Pizzerior.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pizzerior.ViewModels
{
    public partial class AddPizzeriaViewModel: ObservableObject
    {
        [ObservableProperty]
        public Pizzeria _selectedPizzeria;
        IPizzeriaService _pizzeriaService;

        public AddPizzeriaViewModel()
        {
            _pizzeriaService = new PizzeriaService();
        }



        [RelayCommand]
        void CloseWin()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is PizzeriaAdd)
                {
                    window.Close();
                    break;
                }
            }
        }
        

        [RelayCommand]
        void Save(Pizzeria pizzeria)
        {
            
        }


    }
}
