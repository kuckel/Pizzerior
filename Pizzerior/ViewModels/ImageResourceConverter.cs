using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.IO;

namespace Pizzerior.ViewModels
{
    public class ImageResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrEmpty(imagePath) )
            {
                string pathToImages = Directory.GetCurrentDirectory() + @"\Images\" + imagePath;
                var uri = new Uri(pathToImages);              
                return new System.Windows.Media.Imaging.BitmapImage(uri);
            }
            else
            {
                string pathToImages = Directory.GetCurrentDirectory() + @"\Images\Missing-image.png";
                var uri = new Uri(pathToImages);
                return new System.Windows.Media.Imaging.BitmapImage(uri);
            }
          

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}
