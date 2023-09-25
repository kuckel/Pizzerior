#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pizzerior.Models;
using Pizzerior.Services;
using Pizzerior.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Pizzerior.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
     
       
        private ObservableCollection<Pizzeria> _pizzerior { get; set; }
        private readonly IPizzeriaService _pizzeriaService;

        public ObservableCollection<Pizzeria> Pizzerior 
        {
            get { return _pizzerior; }
            set
            {
                if (_pizzerior != value)
                {
                    _pizzerior = value;
                    OnPropertyChanged(nameof(Pizzerior));
                }
            }
        }


        [ObservableProperty]
        Pizzeria _selectedPizzeria;

        public MainViewModel()
        {
            _selectedPizzeria = new Pizzeria();
            Pizzerior = new ObservableCollection<Pizzeria>();
            _pizzeriaService = new PizzeriaService();
            Pizzerior = _pizzeriaService.GetAllCollection();  
        }




        [RelayCommand]
        void Reload()
        {
            Pizzerior.Clear();
            foreach(Pizzeria pz in _pizzeriaService.GetAllCollection())
            {
                Pizzerior.Add(pz); 
            }
        }

        //public void ReloadCollection()
        //{
        //    pizzerior.Clear();
        //    foreach (Pizzeria pz in _pizzeriaService.GetAllCollection())
        //    {
        //        pizzerior.Add(pz);
        //    }
            
        //}



        [RelayCommand]
        void Open(Pizzeria pizzeria)
        {

            PizzeriaViewModel wvm = new PizzeriaViewModel();
            PizzeriaDetail win = new PizzeriaDetail();
            win.DataContext = wvm;
            wvm.SelectedPizzeria = pizzeria;
            win.MaxHeight = 400; ;
            win.MaxWidth = 600;
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            Pizzerior = _pizzeriaService.GetAllCollection();
        }
           


        [RelayCommand]
        void OpenAdd()
        {
            AddPizzeriaViewModel wvm = new AddPizzeriaViewModel();
            PizzeriaAdd win = new PizzeriaAdd();
            win.DataContext = wvm;
            win.MaxHeight = 400; ;
            win.MaxWidth = 600;
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            Pizzerior = _pizzeriaService.GetAllCollection();


        }

    }
}








