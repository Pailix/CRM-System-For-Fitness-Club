using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

namespace CRM_System_For_Fitness_Club
{
    class ExcelExport
    {
        public static void ExportIntoExcel(IQueryable<ReportElement> reportElements)
        {
            StringBuilder header = new StringBuilder();
            PropertyInfo[] rowPropertyInfos = reportElements.ElementType.GetProperties();
            foreach (PropertyInfo info in rowPropertyInfos)
            {
                if (info.CanRead)
                {
                    header.Append(info.Name + "\t");
                }
            }
            header.Append("\r\n");
            StringBuilder data = new StringBuilder();
            foreach (var myObject in reportElements)
            {
                foreach (PropertyInfo info in rowPropertyInfos)
                {
                    if (info.CanRead)
                    {
                        string tmp = Convert.ToString(info.GetValue(myObject, null));
                        if (!string.IsNullOrEmpty(tmp))
                        {
                            tmp.Replace(",", " ");
                        }
                        data.Append(tmp + "\t");
                    }
                }
                data.Append("\r\n");
            }
            string result = data.ToString();
            if (string.IsNullOrEmpty(result) == false)
            {
                header.Append(result);
                File.WriteAllText(@"d:\loge.doc", header.ToString());   
            }
        }
    }
}
