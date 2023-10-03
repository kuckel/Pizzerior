using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Pizzerior.ViewModels 
{
    public class IntToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            int intValue = (int)value;
            string imagePath = intValue switch
            {
                1 => "/Common/Resources/rate1.png",
                2 => "/Common/Resources/rate2.png",
                3 => "/Common/Resources/rate3.png",
                4 => "/Common/Resources/rate4.png",
                5 => "/Common/Resources/rate5.png",
                _ => "/Common/Resources/rate0.png", 
            };

            // Return a bitmap image 
            return new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
