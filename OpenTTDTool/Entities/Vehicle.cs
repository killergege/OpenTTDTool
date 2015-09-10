using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    public abstract class Vehicle
    {
        protected static Dictionary<PropertyInfoId, PropertyInfo> PropertyDefinition = new Dictionary<PropertyInfoId, PropertyInfo>();
        protected const int MAX_COMMON_PROPERTY_ID = 0x07;

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
            PropertyDefinition.Add(new PropertyInfoId(0x00, null), new PropertyInfo() { Length = FieldSizes.Word, PropertyName = nameof(DateOfIntroduction) });
            PropertyDefinition.Add(new PropertyInfoId(0x02, null), new PropertyInfo() { PropertyName = nameof(ReliabilityDecaySpeed) });
            PropertyDefinition.Add(new PropertyInfoId(0x03, null), new PropertyInfo() { PropertyName = nameof(VehicleLife) });
            PropertyDefinition.Add(new PropertyInfoId(0x04, null), new PropertyInfo() { PropertyName = nameof(ModelLife) });
            PropertyDefinition.Add(new PropertyInfoId(0x06, null), new PropertyInfo() { PropertyName = nameof(Climate) });
            PropertyDefinition.Add(new PropertyInfoId(0x07, null), new PropertyInfo() { PropertyName = nameof(LoadingSpeed) });
            PropertyDefinition.Add(new PropertyInfoId(Constants.PROPERTY_LABEL_CODE, null), new PropertyInfo() { PropertyName = nameof(Name) });
        }

        /// <summary>
        /// Gets the data length. 
        /// Returns Constants.PROPERTY_SPECIAL_LENGTH_VALUE if the length must be computed.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static FieldSizes? GetDataLength(int code, Features? entityType)
        {
            if (code <= MAX_COMMON_PROPERTY_ID)
                entityType = null;

            var key = new PropertyInfoId(code, entityType);
            if (PropertyDefinition.ContainsKey(key))
            {
                return PropertyDefinition[key].Length;
            }
            else
            {
                return null;
            }
        }

        public void SetProperty(int rowNumber, int code, Features? entityType, object value)
        {
            if (code <= MAX_COMMON_PROPERTY_ID)
                entityType = null;

            var key = new PropertyInfoId(code, entityType);
            if (PropertyDefinition.ContainsKey(key))
            {
                var propertyName = PropertyDefinition[key].PropertyName;
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
            sb.AppendLine($"\tRaw data (unused hidden) :");
            foreach (var prop in GetType().GetProperties().OrderBy(p => p.Name).Where(p => p.CanWrite && this.DebugInfo.Any(d=>d.Item1 == p.Name)))
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
