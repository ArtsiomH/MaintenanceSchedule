using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.IO;
using MaintenanceScheduleDataLayer.Entities;
using MaintenanceScheduleDataLayer.Enum;
using Excel = Microsoft.Office.Interop.Excel;
using MaintenanceScheduleDataLayer.Repositories;


namespace MaintenanceScheduleDataLayer.EFContext
{
    class MaintenanceScheduleInitializer : CreateDatabaseIfNotExists<MaintenanceScheduleContext>
    {
        protected override void Seed(MaintenanceScheduleContext context)
        {
            using (EFUnitOfWork work = new EFUnitOfWork("ProbLoc"))
            {
                ReadExcel read = new ReadExcel();
                read.Read(work);
            }         
        }
    }
}
