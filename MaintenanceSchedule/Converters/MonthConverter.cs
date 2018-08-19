using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MaintenanceSchedule.Converters
{
	class MonthConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (DependencyProperty.UnsetValue != values[0] || DependencyProperty.UnsetValue != values[1])
			{
				if ((bool)values[1] == true && values[0] != null)
				{
					return DateTimeFormatInfo.CurrentInfo.GetMonthName(((DateTime)values[0]).Month);
				}
			}
			return "";
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return new[] { Array.IndexOf(DateTimeFormatInfo.CurrentInfo.MonthNames, (string)value).ToString() };
		}
	}
}
