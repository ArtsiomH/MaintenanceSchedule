using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaintenanceSchedule.Converters
{
    public class RecordDateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string date = string.Empty;
            if (values[1] != null)
            {
                date = ((DateTime)values[0]).ToString("d MMMM yyyy");
            }
            else if ((bool)values[2] == true)
            {
                date = ((DateTime)values[0]).ToString("MMMM yyyy");
            }
            else
            {
                date = ((DateTime)values[0]).ToString("yyyy");
            }  
            return date;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
