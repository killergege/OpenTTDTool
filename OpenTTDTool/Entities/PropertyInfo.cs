using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Entities
{
    public class PropertyInfo
    {
        public const int DefaultLength = 1;

        public int Length { get; set; } = DefaultLength;
        public string PropertyName { get; set; }

        public bool IsSpecialLength
        {
            get
            {
                return Length == Constants.PROPERTY_SPECIAL_LENGTH_VALUE;
            }
        }
    }
}
