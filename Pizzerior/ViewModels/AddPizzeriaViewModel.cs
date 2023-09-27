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
using System.Windows.Documents;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using Application = System.Windows.Application;

namespace Pizzerior.ViewModels
{
    
    public partial class AddPizzeriaViewModel: ObservableObject, IDataErrorInfo
    {
        [ObservableProperty]
        Pizzeria _pizzeria;
        IPizzeriaService _pizzeriaService;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
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
        [Required(ErrorMessage = "Fältet är obligatoriskt.")]
        [MinLength(5, ErrorMessage = "Minst 5 tecken i namn")]
        [MaxLength(40)]       
        public string Namn
        {
            get { return _namn; }
            set
            {
                if (_namn != value)
                {
                    _namn = value;
                    OnPropertyChanged(nameof(Namn));
                }
            }
        }

        private string _adress;
        [Required(ErrorMessage = "Fältet är obligatoriskt.")]
        [MinLength(5, ErrorMessage = "Minst 5 tecken i Adressen")]
        [MaxLength(40)]
        public string Adress
        {
            get { return _adress; }
            set
            {
                if (_adress != value)
                {
                    _adress = value;
                    OnPropertyChanged(nameof(Adress));
                }
            }
        }


        private string _postNr;
        public string PostNr
        {
            get { return _postNr; }
            set
            {
                if (_postNr != value)
                {
                    _postNr = value;
                    OnPropertyChanged(nameof(PostNr));
                }
            }
        }
        private string _postOrt;
        public string PostOrt
        {
            get { return _postOrt; }
            set
            {
                if (_postOrt != value)
                {
                    _postOrt = value;
                    OnPropertyChanged(nameof(PostOrt));
                }
            }
        }


        #region " VALIDATION "

        public string this[string columnName]
        {
            get
            {
                var validationContext = new ValidationContext(this, null, null) { MemberName = columnName };
                var validationResults = new List<ValidationResult>();
                if (Validator.TryValidateProperty(GetType().GetProperty(columnName).GetValue(this), validationContext, validationResults))
                {
                    if (_errors.ContainsKey(columnName))
                        _errors.Remove(columnName);
                    if(_errors!=null && _errors.Count > 0) { HasErrors = true; } else { HasErrors = false; }
                    return null;
                }
                _errors[columnName] = validationResults.Select(r => r.ErrorMessage).ToList();
                if (_errors != null && _errors.Count > 0) { HasErrors = true; } else { HasErrors = false; }
                return validationResults.First().ErrorMessage;


            }
        }

        public string Error
        {
            get { return null; }
        }


        private bool _hasErrors;

        public bool HasErrors
        {
            get { return _hasErrors; }
            set
            {
                if (_hasErrors != value)
                {
                    _hasErrors = value;
                    OnPropertyChanged(nameof(HasErrors));
                }
            }
        }

        #endregion 



        [RelayCommand]
        void CreatePizzeria()
        {
            string id = Guid.NewGuid().ToString();

            MessageBox.Show(Error); 

            if (string.IsNullOrEmpty(Namn))
            {
                MessageBox.Show("Pizzerians namn måste anges", "", MessageBoxButton.OK, MessageBoxImage.Error);
                
                return;
            }

            if (_pizzeriaService.IsPizzeriaUnique(Namn, id) == false)
            {
                MessageBox.Show($"Pizzerians namn {Namn} eller id finns redan", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _pizzeria = new Pizzeria
            {
                PizzeriaID = id,
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
                MainVM.ReloadCommand.Execute(null) ;
                //MainVM.pizzerior.Add(_pizzeria);

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
