using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace Common
{
    public class TableModel
    {
        public static T RowConvertModel<T>(DataRow row)
        {
            T t = default(T);
            t = Activator.CreateInstance<T>();
            PropertyInfo[] ps = t.GetType().GetProperties();
            foreach (var item in ps)
            {
                if (row.Table.Columns.Contains(item.Name))
                {
                    object v = row[item.Name];
                    if (v.GetType() == typeof(System.DBNull))
                        v = null;
                    item.SetValue(t, v, null);
                }
            }
            return t;
        }
    }
}
