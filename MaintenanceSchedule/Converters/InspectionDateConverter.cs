using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MaintenanceSchedule.Converters
{
    class InspectionDateConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string date = string.Empty;
            if (values[1] != null)
            {
                date = ((DateTime)values[0]).ToString("d MMMM yyyy");
            }
            else if ((bool)values[2] == true || (values[3] is int && (int)values[3] == 12))
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
