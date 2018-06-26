using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MaintenanceScheduleDataLayer
{
    internal class UpdateEntityReflection
    {
        public static void Update(object oldObj, object newObj)
        {
            if (oldObj.GetType() != newObj.GetType()) return;
            foreach (PropertyInfo property in newObj.GetType().GetProperties())
            {
                if (property.PropertyType.Name == typeof(List<>).Name || property.Name.EndsWith("Id")) continue;
                property.SetValue(oldObj, property.GetValue(newObj));
            }            
        }
    }
}
