using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaintenanceSchedule.Repositories
{
	class MonthRepository
	{
		private ObservableCollection<string> month;

		public MonthRepository()
		{
			List<string> list = new List<string>();
			list.Add("");
			list.AddRange(DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12));
			month = new ObservableCollection<string>(list);
		}

		public ObservableCollection<string> GetAllMonths()
		{
			return month;
		}
	}
}
