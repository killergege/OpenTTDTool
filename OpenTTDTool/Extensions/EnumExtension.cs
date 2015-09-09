using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenTTDTool
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum myEnum)
        {
            FieldInfo fi = myEnum.GetType().GetField(myEnum.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return myEnum.ToString();

        }
    }
}
