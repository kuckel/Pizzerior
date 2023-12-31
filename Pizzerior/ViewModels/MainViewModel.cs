﻿#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pizzerior.Models;
using Pizzerior.Services;
using Pizzerior.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Pizzerior.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {

        
        private ObservableCollection<Pizzeria> _pizzerior { get; set; }
        private readonly IPizzeriaService _pizzeriaService;
        private readonly ILoggerService _loggerService;

        public MainViewModel(IPizzeriaService pizzeriaService, ILoggerService loggerservice )
        {
            
            CreteFoldersIfMissing();
            log4net.Config.XmlConfigurator.Configure();
            _loggerService= loggerservice;
            _selectedPizzeria = new Pizzeria();
            Pizzerior = new ObservableCollection<Pizzeria>();
            _pizzeriaService = pizzeriaService;
            Pizzerior = _pizzeriaService.GetAllCollection();
            StatusText = "Antal pizzerior: " + Pizzerior.Count();           



        }


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




        private void CreteFoldersIfMissing()
        {

            string ImageFolderPath = Directory.GetCurrentDirectory() + @"\Images";
            string DataFolderPath = Directory.GetCurrentDirectory() + @"\Data";
            if (!Directory.Exists(ImageFolderPath))
            {
                Directory.CreateDirectory(ImageFolderPath);  
            }
            if (!Directory.Exists(DataFolderPath))
            {
                Directory.CreateDirectory(DataFolderPath);
            }

        }


        [RelayCommand]
        public void Rate()
        {
            MessageBox.Show("Rate");  
            //Pizzeria pizzeria = _selectedPizzeria; 
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
            try
            {

                string pathToImages = Directory.GetCurrentDirectory() + @"\Images\";
                string pizzaImage = pizzeria.IntroBild ?? "Missing-image.png";
                var uri = new Uri(pathToImages + pizzaImage);
                StatusText = "Vald pizzeria: " + pizzeria.Namn;
                PizzeriaViewModel wvm = ViewModelLocator.Instance.PizzeriaViewModel; 
                PizzeriaDetail win = new PizzeriaDetail();
                win.DataContext = wvm;
                wvm.SelectedPizzeria = pizzeria;
                wvm.Namn = pizzeria.Namn;
                wvm.Adress = pizzeria.Adress;
                wvm.PostNr = pizzeria.PostNr;
                wvm.PostOrt = pizzeria.PostOrt;
                wvm.UploadedImage = new BitmapImage(uri);
                win.MaxHeight = 400;
                win.MaxWidth = 600;
                win.Owner = Application.Current.MainWindow;
                win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                win.ShowDialog();
                Pizzerior = _pizzeriaService.GetAllCollection();
                StatusText = "Antal pizzerior: " + Pizzerior.Count();
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

}
           


        [RelayCommand]
        void OpenAdd()
        {
            try
            {
                StatusText = "Lägg till Pizzeria";
                AddPizzeriaViewModel wvm = ViewModelLocator.Instance.AddPizzeriaViewModel;
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



        }

    }
}








