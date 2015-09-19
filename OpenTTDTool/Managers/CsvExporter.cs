using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Managers
{
    public class CsvExporter
    {
        public static void ExportObjects<T>(string filePath, List<T> exportObjects, string delimiter, List<string> ignoredFields =null )
        {
            using (var writer = new StreamWriter(filePath, false))
            {
                var propList = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
                if (ignoredFields != null)
                    propList = propList.Where(p => !ignoredFields.Contains(p.Name)).ToList();
                    
                //Write header
                writer.WriteLine(String.Join(delimiter, propList.Select(p => p.Name)));

                var stringValueList = new List<string>();
                foreach (var obj in exportObjects)
                {
                    stringValueList.Clear();             
                    foreach (var prop in propList)
                    {
                        var value = prop.GetValue(obj);
                        if (value == null)
                            stringValueList.Add(String.Empty);
                        else
                            stringValueList.Add(value.ToString());
                    }
                    writer.WriteLine(String.Join(delimiter, stringValueList));
                }                
            }
        }
    }
}
