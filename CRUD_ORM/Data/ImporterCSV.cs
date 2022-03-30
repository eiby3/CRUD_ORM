using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CRUD_ORM.Data
{
    public class ImporterCSV
    {
        public List<KeyValuePair<string, string>> Mappings;

        public List<T> Import<T> (string file)
        {
            List<T> list = new List<T> ();
            List<string> lines = File.ReadAllLines(file).ToList();
            string headerLine = lines[0];
            var headerInfo = headerLine.Split(',').ToList().Select((v, i)=> new {ColName=v, ColIndex=i});

            Type type = typeof (T);
            var properties = type.GetProperties();

            var dataLines = lines.Skip(1);
            dataLines.ToList().ForEach(line =>
            {
                var values = line.Split(",");
                T obj = (T)Activator.CreateInstance(type);

                foreach (var prop in properties)
                {
                    var mapping = Mappings.SingleOrDefault(m => m.Value == prop.Name);
                    var colName = mapping.Key;
                    var colIndex = headerInfo.SingleOrDefault(s => s.ColName == colName).ColIndex;
                    var value = values[colIndex];
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(value, propType));
                }
                list.Add(obj);
            });
            
            return list;
        }
    }
}
