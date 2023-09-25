#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pizzerior.Models;
using Pizzerior.Services;
using Pizzerior.Views;
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
            _selectedPizzeria.Modifierad = DateTime.Now;
            Pizzeria upPizzeria = _pizzeriaService.Update(_selectedPizzeria);
            if(upPizzeria!=null)
            {
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Ett fel uppstod när pizzerian skulle sparas", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        [RelayCommand]
        void Delete(string id)
        {
          
            var result = MessageBox.Show("Är du säker på att du vill radera vald Pizzeria?", "Radera?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bool res = _pizzeriaService.Delete(id); 
                if (res)
                {
                    CloseWindow();
                }
                else
                {
                    MessageBox.Show("Ett fel uppstod när pizzerian skulle raderas", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }


        }

        [RelayCommand]
        void CloseWin()
        {
            //MainViewModel.LoadCategoryCommand.Execute();

            CloseWindow();
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is PizzeriaDetail)
                {
                    window.Close();
                    break;
                }

            }
        }

    }
}
