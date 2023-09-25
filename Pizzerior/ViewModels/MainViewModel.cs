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
     
       // public ObservableCollection<Pizzeria> pizzerior { get; set; }
        private ObservableCollection<Pizzeria> _pizzerior { get; set; }
        private readonly IPizzeriaService _pizzeriaService;

        //private ObservableCollection<YourItemType> _yourCollection;
        public ObservableCollection<Pizzeria> pizzerior { get; set; }
        //{
        //    //get { return _pizzerior; }
        //    //set
        //    //{
        //    //    if (_pizzerior != value)
        //    //    {
        //    //        _pizzerior = value;
        //    //        OnPropertyChanged(nameof(_pizzerior));
        //    //    }
        //    //}
        //}


        [ObservableProperty]
        Pizzeria _selectedPizzeria;

        public MainViewModel()
        {
            _selectedPizzeria = new Pizzeria();
            pizzerior = new ObservableCollection<Pizzeria>();
            _pizzeriaService = new PizzeriaService();
            pizzerior = _pizzeriaService.GetAllCollection();  
        }




        [RelayCommand]
        void Reload()
        {
            pizzerior.Clear();
            foreach(Pizzeria pz in _pizzeriaService.GetAllCollection())
            {
                pizzerior.Add(pz); 
            }
        }

        public void ReloadCollection()
        {
            pizzerior.Clear();
            foreach (Pizzeria pz in _pizzeriaService.GetAllCollection())
            {
                pizzerior.Add(pz);
            }
            
        }



        [RelayCommand]
        void Open(Pizzeria pizzeria)
        {
            MessageBox.Show(pizzeria.Namn);
            //var result = MessageBox.Show("Close application?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    var w = Application.Current.Windows[0];
            //    w.Close();
            //}


            PizzeriaViewModel wvm = new PizzeriaViewModel();
            PizzeriaDetail win = new PizzeriaDetail();
            win.DataContext = wvm;
            wvm.SelectedPizzeria = pizzeria;
            win.MaxHeight = 400; ;
            win.MaxWidth = 600;
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.Show();
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
            win.Show();

            Reload(); 
        }

    }
}








