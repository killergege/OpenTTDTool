using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    public abstract class Vehicle
    {
        protected static Dictionary<int, PropertyInfo> PropertyDefinition = new Dictionary<int, PropertyInfo>();

        public string Name { get; set; }
        public int DateOfIntroduction { get; set; }
        public int ReliabilityDecaySpeed { get; set; }
        public int VehicleLife { get; set; }
        public int ModelLife { get; set; }
        public int Climate { get; set; }
        public int LoadingSpeed { get; set; }

        public List<Tuple<string, int>> DebugInfo = new List<Tuple<string, int>>();

        static Vehicle()
        {
            PropertyDefinition.Add(0x00, new PropertyInfo() { Length = 2, PropertyName = nameof(DateOfIntroduction) });
            PropertyDefinition.Add(0x02, new PropertyInfo() { PropertyName = nameof(ReliabilityDecaySpeed) });
            PropertyDefinition.Add(0x03, new PropertyInfo() { PropertyName = nameof(VehicleLife) });
            PropertyDefinition.Add(0x04, new PropertyInfo() { PropertyName = nameof(ModelLife) });
            PropertyDefinition.Add(0x06, new PropertyInfo() { PropertyName = nameof(Climate) });
            PropertyDefinition.Add(0x07, new PropertyInfo() { PropertyName = nameof(LoadingSpeed) });
            PropertyDefinition.Add(Constants.PROPERTY_LABEL_CODE, new PropertyInfo() { PropertyName = nameof(Name) });
        }

        /// <summary>
        /// Gets the data length. 
        /// Returns Constants.PROPERTY_SPECIAL_LENGTH_VALUE if the length must be computed.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static int? GetDataLength(int code)
        {
            if (PropertyDefinition.ContainsKey(code))
            {
                return PropertyDefinition[code].Length;
            }
            else
            {
                return null;
            }
        }

        public void SetProperty(int rowNumber, int code, object value)
        {
            if (PropertyDefinition.ContainsKey(code))
            {
                var propertyName = PropertyDefinition[code].PropertyName;
                if (!String.IsNullOrWhiteSpace(propertyName))
                {
                    var type = this.GetType();
                    var prop = type.GetProperty(propertyName);
                    prop.SetValue(this, value);

                    DebugInfo.Add(new Tuple<string, int>(propertyName, rowNumber));
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Type: {this.GetType().Name}, Name: {Name}({this.DebugInfo.FirstOrDefault(d => d.Item1 == "Name")?.Item2})");
            sb.AppendLine($"\tRaw data :");
            foreach (var prop in GetType().GetProperties().OrderBy(p => p.Name).Where(p => p.CanWrite))
            {
                sb.AppendLine($"\t\t{prop.Name}: {prop.GetValue(this)} (row: {this.DebugInfo.FirstOrDefault(p=>p.Item1 == prop.Name)?.Item2 })");
            }
            sb.AppendLine($"\tCalculated :");
            foreach (var prop in GetType().GetProperties().OrderBy(p => p.Name).Where(p => !p.CanWrite))
            {
                sb.AppendLine($"\t\t{prop.Name}: {prop.GetValue(this)}");
            }

            sb.AppendLine();
            return sb.ToString();
        }
    }
}
