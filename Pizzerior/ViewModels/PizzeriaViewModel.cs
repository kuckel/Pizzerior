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
    public partial class PizzeriaViewModel : ObservableObject, IDataErrorInfo
    {
        private readonly IPizzeriaService _pizzeriaService;
        public MainViewModel MainVM => ViewModelLocator.Instance.MainViewModel;
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();
        ILoggerService _loggerService;

        [ObservableProperty]
        public Pizzeria _selectedPizzeria;
 
        public PizzeriaViewModel()
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
        [Required(ErrorMessage = "Fältet är obligatoriskt.")]
        [RegularExpression(@"^\d{3} \d{2}$", ErrorMessage = "Felaktigt format i PostNr.")]
        [MaxLength(6, ErrorMessage = "Max antal tecken är 6")]
        [MinLength(5, ErrorMessage = "Fel antal tecken i PostNr")]
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
        [MaxLength(6)]
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

          

        [RelayCommand]
        public void OpenRate()
        {
            
            RatePizzeriaViewModel wvm = new RatePizzeriaViewModel();
            RatePizzeria win = new RatePizzeria();
            win.DataContext = wvm;
            wvm._selectedPizzeria = _selectedPizzeria;
            win.MaxHeight = 400; ;
            win.MaxWidth = 600;
            win.Owner = Application.Current.MainWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
       }

        [RelayCommand]
        public void ReloadRate(Pizzeria pizzeria)
        {
            _selectedPizzeria = pizzeria;        
        }




        public string IntroBild { get; set; }

        [RelayCommand]
        public void DoSave()
        {
            _selectedPizzeria.Modifierad = DateTime.Now;
            _selectedPizzeria.Namn = Namn;
            _selectedPizzeria.Adress = Adress;
            _selectedPizzeria.PostOrt = PostOrt;
            _selectedPizzeria.PostNr = PostNr;
            if (!string.IsNullOrEmpty(IntroBild))
            {
             _selectedPizzeria.IntroBild = IntroBild;              
            }

            
            Pizzeria upPizzeria = _pizzeriaService.Update(_selectedPizzeria);
            if(upPizzeria!=null)
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
            CloseWindow();
        }            



        private void CloseWindow()
        {
            MainVM.ReloadCommand.Execute(null); // Reload the Main

            foreach (Window window in Application.Current.Windows)
            {
                if (window is PizzeriaDetail)
                {
                    window.Close();
                    break;
                }

            }
        }


        #region " BILDUPPLADDNING "

        private BitmapImage _uploadedImage;
        public BitmapImage UploadedImage
        {
            get => _uploadedImage;
            set => SetProperty(ref _uploadedImage, value);
        }

        [RelayCommand]
        private void UploadImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg;*.jpeg;*.gif)|*.png;*.jpg;*.jpeg;*.gif";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName));
                    UploadedImage = bitmapImage;
                    SaveImage(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    _loggerService.LogInfo(System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                    MessageBox.Show($"Error loading image: {ex.Message}"); 
                }
            }
        }

        private void SaveImage(string pathToImage)
        {

            try
            {
                BitmapEncoder encoder = null;
                string selectedFilter = Path.GetExtension(pathToImage);
                if (selectedFilter == ".png")
                    encoder = new PngBitmapEncoder();
                else if (selectedFilter == ".jpeg")
                    encoder = new JpegBitmapEncoder();
                else if (selectedFilter == ".jpg")
                    encoder = new JpegBitmapEncoder();
                else if (selectedFilter == ".gif")
                    encoder = new GifBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(UploadedImage));
                string pathToImages = Directory.GetCurrentDirectory() + @"\Images\";

                var fileNameToSave = _selectedPizzeria.PizzeriaID + selectedFilter;
                File.Copy(pathToImage, pathToImages + fileNameToSave, true);
                if (!File.Exists(pathToImages + fileNameToSave))
                {
                    MessageBox.Show("Bilden misslyckades att kopieras till utsatt mapp", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    IntroBild = fileNameToSave;
                }
            }
            catch (Exception ex)
            {
                _loggerService.LogInfo(System.Reflection.MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }               

        }

        #endregion 

    }
}
