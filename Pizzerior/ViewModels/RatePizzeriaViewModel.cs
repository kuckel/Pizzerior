#nullable disable 
using CommunityToolkit.Mvvm.ComponentModel;

using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using Pizzerior.Models;
using Pizzerior.Services;
using Pizzerior.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Pizzerior.ViewModels
{
    public partial class  RatePizzeriaViewModel : ObservableObject, IDataErrorInfo
    {

        private readonly IPizzeriaService _pizzeriaService;
        public MainViewModel MainVM => ViewModelLocator.Instance.MainViewModel;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        ILoggerService _loggerService;
        [ObservableProperty]
        public Pizzeria _selectedPizzeria;


        public RatePizzeriaViewModel() 
        {
            _pizzeriaService = new PizzeriaService();
            _loggerService = new LoggerService();
        }


        private string _namn;
        [Required(ErrorMessage = "Fältet är obligatoriskt.")]
        [MinLength(5, ErrorMessage = "Minst 5 tecken i namn")]
        [MaxLength(40)]
        public string Namn
        {
            get { return _namn ?? ""; }
            set
            {
                if (_namn != value)
                {
                    _namn = value;
                    OnPropertyChanged(nameof(Namn));
                }
            }
        }

        private string _betyg;

        public string Betyg
        {
            get { return _betyg ?? ""; }
            set
            {
                if (_betyg != value)
                {
                    _betyg = value;
                    OnPropertyChanged(nameof(Namn));
                }
            }
        }



        private string _epost;
        [Required(ErrorMessage = "Fältet är obligatoriskt.")]
        [MinLength(5, ErrorMessage = "Minst 5 tecken i Epostadress")]
        [MaxLength(40)]
        public string Epost
        {
            get { return _epost ?? ""; }
            set
            {
                if (_epost != value)
                {
                    _epost = value;
                    OnPropertyChanged(nameof(Epost));
                }
            }
        }

        [RelayCommand]
        void CloseWin()
        {
            CloseWindow();
        }



        [RelayCommand]
        public void Save()
        {
            string pizzaID = _selectedPizzeria.PizzeriaID; 
            Omdome omdome = new Omdome();
            omdome.PizzeriaID_Ref = pizzaID;
            omdome.Namn = Namn;
            omdome.Epost = Epost;
            string betyg = Betyg.ToUpper(); 


            Pizzeria upPizzeria = _pizzeriaService.Update(_selectedPizzeria);
            if (upPizzeria != null)
            {
                MainVM.StatusText = $"Pizzeria {upPizzeria.Namn} uppdaterad";
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Ett fel uppstod när pizzerian skulle sparas", "", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    if (_errors != null && _errors.Count > 0) { HasErrors = true; } else { HasErrors = false; }
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


        private void CloseWindow()
        {
            MainVM.ReloadCommand.Execute(null); // Reload the Main

            foreach (Window window in Application.Current.Windows)
            {
                if (window is RatePizzeria)
                {
                    window.Close();
                    break;
                }

            }
        }

    }


}

