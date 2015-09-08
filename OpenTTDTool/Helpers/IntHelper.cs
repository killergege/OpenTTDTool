using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool.Helpers
{
    public static class IntHelper
    {
        public static int ConvertFromHex(string hex)
        {
            return int.Parse(hex.ToString(), NumberStyles.HexNumber);
        }

        public static bool TryConvertFromHex(string hex, out int result)
        {
            return int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
        }
    }
}
