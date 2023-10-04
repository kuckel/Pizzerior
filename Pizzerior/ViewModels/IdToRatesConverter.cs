using Pizzerior.Models;
using Pizzerior.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Pizzerior.ViewModels
{


    public class IdToRatesConverter : IValueConverter
    {

        IPizzeriaService? _pizzeriaService;

        /// <summary>
        /// Converts a PizzeriaID to count of omdomen
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _pizzeriaService= new PizzeriaService();
            if (value is string pizzeriaId)
            {
                Pizzeria pz = _pizzeriaService.Get(pizzeriaId); 
                if (pz != null)
                {
                    return pz.Omdomen.Count;
                }
                
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("IdToRatesConverter is a one-way converter.");
        }
    }
}
