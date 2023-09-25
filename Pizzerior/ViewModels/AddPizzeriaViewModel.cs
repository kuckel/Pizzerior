#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pizzerior.Models;
using Pizzerior.Services;
using Pizzerior.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Pizzerior.ViewModels
{
    
    public partial class AddPizzeriaViewModel: ObservableObject, IDataErrorInfo
    {
        [ObservableProperty]
        Pizzeria _pizzeria;
        IPizzeriaService _pizzeriaService;

        public MainViewModel MainVM => ViewModelLocator.Instance.MainViewModel;


        public AddPizzeriaViewModel()
        {
            _pizzeriaService = new PizzeriaService();
            _pizzeria= new Pizzeria();
             
        }



        [RelayCommand]
        void CloseWin()
        {
             
            CloseWindow();
        }

        private void CloseWindow()
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

        private string _namn;
        [Required]
        [MinLength(5, ErrorMessage = "Minst 5 tecken")]
        [MaxLength(40)]
        
        
        public string Namn
        {
            get { return _namn; }
            set
            {
                SetProperty(ref _namn, value);
            }
        }
        private string _adress;
        public string Adress
        {
            get { return _adress; }
            set
            {
                SetProperty(ref _adress, value);
            }
        }
        private string _postNr;
        public string PostNr
        {
            get { return _postNr; }
            set
            {
                SetProperty(ref _postNr, value);
            }
        }
        private string _postOrt;
        public string PostOrt
        {
            get { return _postOrt; }
            set
            {
                SetProperty(ref _postOrt, value);
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                var context = new ValidationContext(this, null, null) { MemberName = columnName };
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this, null), context, results))
                {
                    return results[0].ErrorMessage;
                }
                return null;
            }
        }



        [RelayCommand]
        void CreatePizzeria()
        {

            if (string.IsNullOrEmpty(Namn))
            {
                MessageBox.Show("Pizzerians namn måste anges", "", MessageBoxButton.OK, MessageBoxImage.Error);
                
                return;
            }

            if (_pizzeriaService.IsPizzeriaUnique(Namn) == false)
            {
                MessageBox.Show($"Pizzerians namn {Namn} finns redan", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _pizzeria = new Pizzeria
            {
                PizzeriaID = Guid.NewGuid().ToString(),
                Namn = Namn,
                Adress = Adress,
                PostNr = PostNr,
                PostOrt = PostOrt,
                Skapad = DateTime.Now,
                Modifierad = DateTime.Now,
            };

            bool result = _pizzeriaService.Create(_pizzeria);
            if (result)
            {
                 
                CloseWin();
            }
            else
            {
                MessageBox.Show("Ett fel uppstod när pizzerian skulle skapas", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }



        }




    }
}
