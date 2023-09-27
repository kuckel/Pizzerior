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

        private string searchFor;
        public string SearchFor
        {
            get { return searchFor; }
            set { SetProperty(ref searchFor, value); }
        }

        /// <summary>
        /// Sets the tool-status-bar at bottom with text
        /// </summary>
        private string statusText;
        public string StatusText
        {
            get { return statusText; }
            set { SetProperty(ref statusText, value); }
        }


        [ObservableProperty]
        Pizzeria _selectedPizzeria;

        public MainViewModel()
        {
            _selectedPizzeria = new Pizzeria();
            Pizzerior = new ObservableCollection<Pizzeria>();
            _pizzeriaService = new PizzeriaService();
            Pizzerior = _pizzeriaService.GetAllCollection();
            StatusText = "Antal pizzerior: " + Pizzerior.Count();
        }




        [RelayCommand]
        public void Reload()
        {
            Pizzerior.Clear();
            foreach(Pizzeria pz in _pizzeriaService.GetAllCollection())
            {
                Pizzerior.Add(pz); 
            }
            StatusText = "Antal pizzerior: " + Pizzerior.Count();
        }

        [RelayCommand]
        public void Search()
        {
            if(!string.IsNullOrEmpty(SearchFor) )
            {
                Pizzerior.Clear();
                foreach (Pizzeria pz in _pizzeriaService.GetAllCollection().Where(x=>x.Namn.ToLower().Contains(searchFor.ToLower()) || x.Adress!=null &&  x.Adress.ToLower().Contains(searchFor.ToLower())))
                {
                    Pizzerior.Add(pz);
                }
                StatusText = "Din sökning gav: " + Pizzerior.Count() + " pizzerior";
            }
            else
            {
                Reload();
            }

            
            SearchFor = "";

        }



        [RelayCommand]
        void Open(Pizzeria pizzeria)
        {
            StatusText = "Vald pizzeria: " + pizzeria.Namn; 
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
            StatusText = "Antal pizzerior: " + Pizzerior.Count();

        }
           


        [RelayCommand]
        void OpenAdd()
        {
            StatusText = "Lägg till Pizzeria";
            AddPizzeriaViewModel wvm = new AddPizzeriaViewModel();
            PizzeriaAdd win = new PizzeriaAdd();
            win.DataContext = wvm;
            win.MaxHeight = 400; ;
            win.MaxWidth = 600;
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            Pizzerior = _pizzeriaService.GetAllCollection();
            StatusText = "Antal pizzerior: " + Pizzerior.Count();



        }

    }
}








